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
            ViewBag.Assets = _context.Assets.ToList(); //
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,ExistingControl")] Risk risk)
        {
            var asset = _context.Assets.FirstOrDefault(a => a.CodigoActivo == risk.Code);
            if (asset != null)
            {
                var assetVulnerabilities = _context.AssetVulnerabilitys.Where(av => av.AssetId == asset.ID);
                var assetThreats = _context.AssetThreats.Where(at => at.AssetId == asset.ID);

                if (assetVulnerabilities.Any() && assetThreats.Any())
                {
                    double sumConfidencialidad = assetVulnerabilities.Select(av => av.Vulnerability.Probability).Sum();
                    double sumIntegridad = assetVulnerabilities.Select(av => av.Vulnerability.Probability).Sum();
                    double sumDisponibilidad = assetVulnerabilities.Select(av => av.Vulnerability.Probability).Sum();
                    double averageConfidencialidad = sumConfidencialidad / assetVulnerabilities.Count();
                    double averageIntegridad = sumIntegridad / assetVulnerabilities.Count();
                    double averageDisponibilidad = sumDisponibilidad / assetVulnerabilities.Count();

                    double sumThreatProbability = assetThreats.Select(at => at.Threat.Probability).Sum();
                    double averageThreatProbability = sumThreatProbability / assetThreats.Count();

                    double sumVulnerabilityProbability = assetVulnerabilities.Select(av => av.Vulnerability.Probability).Sum();
                    double averageVulnerabilityProbability = sumVulnerabilityProbability / assetVulnerabilities.Count();

                    risk.CID = (averageConfidencialidad + averageIntegridad + averageDisponibilidad) / 3.0;
                    risk.ThreatLevel = (int)averageThreatProbability;
                    risk.VulnerabilityLevel = (int)averageVulnerabilityProbability;
                    risk.RiskLevel = risk.CID * risk.ThreatLevel * risk.VulnerabilityLevel;
                    risk.Result = risk.RiskLevel > 20 ? "Alto" : risk.RiskLevel > 5 ? "Medio" : "Bajo";
                }
            }

            _context.Add(risk);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        public IActionResult Tratamiento(string code)
        {
            // Aquí puedes realizar cualquier lógica adicional antes de mostrar la vista "Tratamiento.cshtml"
            return View();
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
