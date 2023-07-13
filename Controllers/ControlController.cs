using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAssetManager.Data;
using SecureAssetManager.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SecureAssetManager.Controllers
{
    public class ControlController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ControlController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Controls.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Control control)
        {
            if (ModelState.IsValid)
            {
                _context.Add(control);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(control);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var control = await _context.Controls.FirstOrDefaultAsync(m => m.ID == id);
            if (control == null)
            {
                return NotFound();
            }

            return View(control);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var control = await _context.Controls.FindAsync(id);
            if (control == null)
            {
                return NotFound();
            }
            return View(control);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Control control)
        {
            if (id != control.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(control);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControlExists(control.ID))
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
            return View(control);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var control = await _context.Controls.FirstOrDefaultAsync(m => m.ID == id);
            if (control == null)
            {
                return NotFound();
            }

            return View(control);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var control = await _context.Controls.FindAsync(id);
            _context.Controls.Remove(control);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ControlExists(int id)
        {
            return _context.Controls.Any(e => e.ID == id);
        }
    }
}
