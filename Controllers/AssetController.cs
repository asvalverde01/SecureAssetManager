using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,CodigoActivo,Nombre,Responsable,Ubicacion,Descripcion,Tipo,Categoria,Clasificacion,EtiquetaPrincipal,ValoracionConfidencialidad,ValoracionIntegridad,ValoracionDisponibilidad")] Asset asset, int[] selectedThreats, int[] selectedVulnerabilities)
        {
            if (id != asset.ID)
            {
                return NotFound();
            }

            try
            {
                // Obtener el asset existente incluyendo sus amenazas y vulnerabilidades
                var existingAsset = await _context.Assets
                    .Include(a => a.AssetThreats)
                    .Include(a => a.AssetVulnerabilities)
                    .FirstOrDefaultAsync(a => a.ID == asset.ID);

                if (existingAsset == null)
                {
                    return NotFound();
                }

                // Actualizar las amenazas y vulnerabilidades seleccionadas
                UpdateSelectedThreats(existingAsset, selectedThreats);
                UpdateSelectedVulnerabilities(existingAsset, selectedVulnerabilities);

                // Actualizar los demás campos del asset
                existingAsset.CodigoActivo = asset.CodigoActivo;
                existingAsset.Nombre = asset.Nombre;
                existingAsset.Responsable = asset.Responsable;
                existingAsset.Ubicacion = asset.Ubicacion;
                existingAsset.Descripcion = asset.Descripcion;
                existingAsset.Tipo = asset.Tipo;
                existingAsset.Categoria = asset.Categoria;
                existingAsset.Clasificacion = asset.Clasificacion;
                existingAsset.EtiquetaPrincipal = asset.EtiquetaPrincipal;
                existingAsset.ValoracionConfidencialidad = asset.ValoracionConfidencialidad;
                existingAsset.ValoracionIntegridad = asset.ValoracionIntegridad;
                existingAsset.ValoracionDisponibilidad = asset.ValoracionDisponibilidad;

                _context.Update(existingAsset);
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


        private void UpdateSelectedThreats(Asset asset, int[] selectedThreats)
        {
            // Eliminar las amenazas deseleccionadas
            var threatsToRemove = asset.AssetThreats.Where(at => !selectedThreats.Contains(at.ThreatId)).ToList();
            foreach (var threatToRemove in threatsToRemove)
            {
                asset.AssetThreats.Remove(threatToRemove);
            }

            // Agregar las amenazas seleccionadas que no estén en el asset
            var existingThreatIds = asset.AssetThreats.Select(at => at.ThreatId).ToList();
            var newThreats = selectedThreats.Where(threatId => !existingThreatIds.Contains(threatId))
                .Select(threatId => new AssetThreat { AssetId = asset.ID, ThreatId = threatId });
            asset.AssetThreats.AddRange(newThreats);
        }

        private void UpdateSelectedVulnerabilities(Asset asset, int[] selectedVulnerabilities)
        {
            // Eliminar las vulnerabilidades deseleccionadas
            var vulnerabilitiesToRemove = asset.AssetVulnerabilities.Where(av => !selectedVulnerabilities.Contains(av.VulnerabilityId)).ToList();
            foreach (var vulnerabilityToRemove in vulnerabilitiesToRemove)
            {
                asset.AssetVulnerabilities.Remove(vulnerabilityToRemove);
            }

            // Agregar las vulnerabilidades seleccionadas que no estén en el asset
            var existingVulnerabilityIds = asset.AssetVulnerabilities.Select(av => av.VulnerabilityId).ToList();
            var newVulnerabilities = selectedVulnerabilities.Where(vulnerabilityId => !existingVulnerabilityIds.Contains(vulnerabilityId))
                .Select(vulnerabilityId => new AssetVulnerability { AssetId = asset.ID, VulnerabilityId = vulnerabilityId });
            asset.AssetVulnerabilities.AddRange(newVulnerabilities);
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
