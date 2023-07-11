using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAssetManager.Data;
using SecureAssetManager.Models;

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodigoActivo,Nombre,Responsable,Ubicacion,Descripcion,Tipo,Categoria,Clasificacion,EtiquetaPrincipal,ValoracionConfidencialidad,ValoracionIntegridad,ValoracionDisponibilidad")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(asset);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }
            return View(asset);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CodigoActivo,Nombre,Responsable,Ubicacion,Descripcion,Tipo,Categoria,Clasificacion,EtiquetaPrincipal,ValoracionConfidencialidad,ValoracionIntegridad,ValoracionDisponibilidad")] Asset asset)
        {
            if (id != asset.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAsset = _context.Assets.Find(asset.ID);
                    if (existingAsset == null)
                    {
                        return NotFound();
                    }

                    // update existing asset with values from request
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

                    // save changes to database
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
            return View(asset);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Assets.FirstOrDefaultAsync(m => m.ID == id);
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

            var asset = await _context.Assets.FirstOrDefaultAsync(m => m.ID == id);
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
