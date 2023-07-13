using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using SecureAssetManager.Data;
using SecureAssetManager.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SecureAssetManager.Controllers
{
    public class RiskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RiskController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Risks.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.AssetCodes = _context.Assets.Select(a => a.CodigoActivo);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,ExistingControl")] Risk risk)
        {
            Console.WriteLine("Primer paso");

            var asset = _context.Assets.FirstOrDefault(a => a.CodigoActivo == risk.Code);
            var threat = _context.Threats.FirstOrDefault(t => t.Id == int.Parse(risk.Code));

            var vulnerability = _context.Vulnerabilities.FirstOrDefault(v => v.Id == int.Parse(risk.Code));

            if (asset != null && threat != null && vulnerability != null)
            {
                risk.CID = (asset.ValoracionConfidencialidad + asset.ValoracionIntegridad + asset.ValoracionDisponibilidad) / 3.0;
                risk.ThreatLevel = threat.Probability;
                risk.VulnerabilityLevel = vulnerability.Probability;
                risk.RiskLevel = risk.CID * risk.ThreatLevel * risk.VulnerabilityLevel;
                risk.Result = risk.RiskLevel > 20 ? "Alto" : risk.RiskLevel > 5 ? "Medio" : "Bajo";

                // Logging the values
                Console.WriteLine($"CodigoActivo: {asset.CodigoActivo}");
                Console.WriteLine($"ValoracionConfidencialidad: {asset.ValoracionConfidencialidad}");
                Console.WriteLine($"ValoracionIntegridad: {asset.ValoracionIntegridad}");
                Console.WriteLine($"ValoracionDisponibilidad: {asset.ValoracionDisponibilidad}");
                Console.WriteLine($"Threat Probability: {threat.Probability}");
                Console.WriteLine($"Vulnerability Probability: {vulnerability.Probability}");
                Console.WriteLine($"CID: {risk.CID}");
                Console.WriteLine($"Risk Level: {risk.RiskLevel}");
                Console.WriteLine($"Result: {risk.Result}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("Segundo paso");
                    _context.Add(risk);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("Error al guardar el objeto de riesgo en la base de datos:");
                    Console.WriteLine(ex.ToString());

                }
            }
            else
            {
                Console.WriteLine("Model state is not valid. Errors:");
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                _context.Add(risk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AssetCodes = _context.Assets.Select(a => a.CodigoActivo);
            return View(risk);
        }



        public async Task<IActionResult> Details(string code)
        {
            if (code == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks.SingleOrDefaultAsync(m => m.Code == code);
            if (risk == null)
            {
                return NotFound();
            }

            return View(risk);
        }


        public async Task<IActionResult> Edit(string code)
        {
            if (code == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks.FindAsync(code);
            if (risk == null)
            {
                return NotFound();
            }
            return View(risk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string code, Risk risk)
        {
            if (code != risk.Code)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(risk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiskExists(risk.Code))
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
            return View(risk);
        }

        public async Task<IActionResult> Delete(string code)
        {
            if (code == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks.FirstOrDefaultAsync(m => m.Code == code);
            if (risk == null)
            {
                return NotFound();
            }

            return View(risk);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string code)
        {
            var risk = await _context.Risks.FindAsync(code);
            _context.Risks.Remove(risk);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiskExists(string code)
        {
            return _context.Risks.Any(e => e.Code == code);
        }
    }
}
