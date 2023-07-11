using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureAssetManager.Data;
using SecureAssetManager.Models;

namespace SecureAssetManager.Controllers
{
	public class ThreatController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ThreatController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.Threats.ToListAsync());
		}

        public IActionResult Create()
        {
            var assets = _context.Assets.ToList();
            ViewBag.AssetCodes = new SelectList(assets, "CodigoActivo", "CodigoActivo");
            return View();
        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Code,ThreatOrigin,ThreatDescription,Degradation,Probability")] Threat threat)
		{
			if (ModelState.IsValid)
			{
				_context.Add(threat);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(threat);
		}

		public async Task<IActionResult> Details(string code)
		{
			if (code == null)
			{
				return NotFound();
			}

			var threat = await _context.Threats
				.FirstOrDefaultAsync(m => m.Code == code);
			if (threat == null)
			{
				return NotFound();
			}

			return View(threat);
		}

		public async Task<IActionResult> Edit(string code)
		{
			if (code == null)
			{
				return NotFound();
			}

			var threat = await _context.Threats.FindAsync(code);
			if (threat == null)
			{
				return NotFound();
			}
			return View(threat);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string code, [Bind("Code,ThreatOrigin,ThreatDescription,Degradation,Probability")] Threat threat)
		{
			if (code != threat.Code)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(threat);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ThreatExists(threat.Code))
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
			return View(threat);
		}

		public async Task<IActionResult> Delete(string code)
		{
			if (code == null)
			{
				return NotFound();
			}

			var threat = await _context.Threats
				.FirstOrDefaultAsync(m => m.Code == code);
			if (threat == null)
			{
				return NotFound();
			}

			return View(threat);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string code)
		{
			var threat = await _context.Threats.FindAsync(code);
			_context.Threats.Remove(threat);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ThreatExists(string code)
		{
			return _context.Threats.Any(e => e.Code == code);
		}
	}
}
