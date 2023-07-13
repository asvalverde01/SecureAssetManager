using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureAssetManager.Data;
using SecureAssetManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SecureAssetManager.Controllers
{
    public class AssetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssetController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Assets.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Threats = _context.Threats.ToList();
            ViewBag.Vulnerabilities = _context.Vulnerabilities.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodigoActivo,Nombre,Responsable,Ubicacion,Descripcion,Tipo,Categoria,Clasificacion,EtiquetaPrincipal,ValoracionConfidencialidad,ValoracionIntegridad,ValoracionDisponibilidad")] Asset asset, int[] selectedThreats, int[] selectedVulnerabilities)
        {

                asset.AssetThreats = selectedThreats.Select(threatId => new AssetThreat { ThreatId = threatId }).ToList();
                asset.AssetVulnerabilities = selectedVulnerabilities.Select(vulnerabilityId => new AssetVulnerability { VulnerabilityId = vulnerabilityId }).ToList();


                _context.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.AssetThreats)
                .Include(a => a.AssetVulnerabilities)
                .ThenInclude(av => av.Vulnerability)
                .FirstOrDefaultAsync(a => a.ID == id);

            if (asset == null)
            {
                return NotFound();
            }

            ViewBag.Threats = _context.Threats.ToList();
            ViewBag.Vulnerabilities = _context.Vulnerabilities.ToList();

            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CodigoActivo,Nombre,Responsable,Ubicacion,Descripcion,Tipo,Categoria,Clasificacion,EtiquetaPrincipal,ValoracionConfidencialidad,ValoracionIntegridad,ValoracionDisponibilidad,AssetThreats,AssetVulnerabilities")] Asset asset)
        {
            if (id != asset.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asset);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetExists(asset.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Threats = _context.Threats.ToList();
            ViewBag.Vulnerabilities = _context.Vulnerabilities.ToList();

            return View(asset);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets
                .Include(a => a.AssetThreats)
                .ThenInclude(at => at.Threat)
                .Include(a => a.AssetVulnerabilities)
                .ThenInclude(av => av.Vulnerability)
                .FirstOrDefaultAsync(a => a.ID == id);

            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.ID == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool AssetExists(int id)
        {
            return _context.Assets.Any(e => e.ID == id);
        }
    }
}
