using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAssetManager.Areas.Identity.Data;
using SecureAssetManager.Data;
using SecureAssetManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SecureAssetManager.Controllers
{
	public class AssetController : Controller
	{
		//[Authorize]
		private readonly ApplicationDbContext _context;

		public AssetController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Asset
		public async Task<IActionResult> Index()
		{
			return View(await _context.Assets.ToListAsync());
		}

		// GET: Asset/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Asset/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ID,Nombre,Descripcion,Tipo,Categoria,Clasificacion,EtiquetaPrincipal,TipoValoracion,Valoracion")] Asset asset)
		{
			if (ModelState.IsValid)
			{
				_context.Add(asset);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(asset);
		}

		// GET: Asset/Edit/5
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

		// POST: Asset/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("ID,Nombre,Descripcion,Tipo,Categoria,Clasificacion,EtiquetaPrincipal,TipoValoracion,Valoracion")] Asset asset)
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
				return RedirectToAction(nameof(Index));
			}
			return View(asset);
		}

		// GET: Asset/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var asset = await _context.Assets
				.FirstOrDefaultAsync(m => m.ID == id);
			if (asset == null)
			{
				return NotFound();
			}

			return View(asset);
		}

		// POST: Asset/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var asset = await _context.Assets.FindAsync(id);
			_context.Assets.Remove(asset);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool AssetExists(int id)
		{
			return _context.Assets.Any(e => e.ID == id);
		}
	}
}
