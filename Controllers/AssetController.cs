using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAssetManager.Data;
using SecureAssetManager.Models;
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

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CodigoActivo,Nombre,Descripcion,Tipo,Categoria,Clasificacion,EtiquetaPrincipal,TipoValoracion,Valoracion")] Asset asset)
		{
			if (ModelState.IsValid)
			{
				_context.Add(asset);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index", "Home");
			}
			return View(asset);
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.Assets.ToListAsync());
		}
	}
}
