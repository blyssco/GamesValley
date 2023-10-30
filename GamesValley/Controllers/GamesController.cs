using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GamesValley.Data;
using GamesValley.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GamesValley.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; 

        public GamesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Games.Include(g => g.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var games = await _context.Games
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (games == null)
            {
                return NotFound();
            }

            return View(games);
        }

        // GET: Games/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,GameName,GameDescription,UserId")] Games games)
        {
            var user = await _userManager.GetUserAsync(User);
            games.UserId = user.Id;
/*            if (ModelState.IsValid)
            {*/

                if (user != null)
                {
                    _context.Add(games);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
/*            }*/
            return View(games);
        }


        // GET: Games/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var games = await _context.Games.FindAsync(id);
            if (games == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            // Check if the current user is the creator of the game
            if (games.UserId != user.Id)
            {
                // You can return an unauthorized view or a custom error view here
                // For now, returning a generic "Access Denied" view
                return View("AccessDenied");
            }


            return View(games);
        }


        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,GameName,GameDescription,UserId")] Games games)
        {
            if (id != games.GameId)
            {
                return NotFound();
            }

/*            if (ModelState.IsValid)*/
              if (games != null)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    games.UserId = user.Id;
                    _context.Update(games);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GamesExists(games.GameId))
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
            return View(games);
        }

        // GET: Games/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var games = await _context.Games
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (games == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            // Check if the current user is the creator of the game
            if (games.UserId != user.Id)
            {
                // You can return an unauthorized view or a custom error view here
                // For now, returning a generic "Access Denied" view
                return View("AccessDenied");
            }

            return View(games);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Games == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Games'  is null.");
            }
            var games = await _context.Games.FindAsync(id);
            if (games != null)
            {
                _context.Games.Remove(games);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GamesExists(int id)
        {
          return (_context.Games?.Any(e => e.GameId == id)).GetValueOrDefault();
        }
    }
}
