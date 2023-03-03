using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment_3.Data;
using Assignment_3.Models;
using Assignment_3.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Assignment_3.Controllers
{
    public class ActorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITweetWrapper _tweetWrapper;

        public ActorController(ApplicationDbContext context, ITweetWrapper tweetWrapper)
        {
            _context = context;
            _tweetWrapper = tweetWrapper;
        }

        // GET: Actor
        public async Task<IActionResult> Index()
        {
              return View(await _context.Actor.ToListAsync());
        }

        // GET: Actor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            var actorMoviesVM = new ActorMoviesVM();


            actorMoviesVM.Actor = actor;

            //Searches through db for movies that are associated
            actorMoviesVM.Movies = await _context.ActorMovie.Where(am => am.Actor.Id == actor.Id).Select(am => am.Movie).ToListAsync();
            
            //Searches twitter for tweets
            actorMoviesVM.Tweets = await _tweetWrapper.GetTweetsAsync(actor);

            return View(actorMoviesVM);
        }

        public async Task<IActionResult> GetActorPhoto(int id)
        {
            var actor = await _context.Actor.FirstOrDefaultAsync(a => a.Id == id);

            if(actor == null)
            {
                return NotFound();
            }

            return File(actor.Image, "image/jpg");
        }

        // GET: Actor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,IMBDLink,Image")] Actor actor, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if(photo != null && photo.Length > 0)
                {
                    var memoryStream = new MemoryStream();
                    await photo.CopyToAsync(memoryStream);
                    actor.Image = memoryStream.ToArray();
                }

                _context.Add(actor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(actor);
        }

        // GET: Actor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }
            return View(actor);
        }

        // POST: Actor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Gender,IMBDLink")] Actor actor, IFormFile image)
        {
            if (id != actor.Id)
            {
                return NotFound();
            }

            //If the image is null, prevent error checking and saving to the database
            if (image == null)
            {
                ModelState.Remove("image");
                _context.Entry(actor).Property("Image").IsModified = false;
               
            }
            else if(image.Length > 0) 
            {
                var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                actor.Image = memoryStream.ToArray();
            }
            if(actor.Image == null)
            {
                var dbActor = await _context.Actor.FirstOrDefaultAsync(a => a.Id == id);
                actor.Image = dbActor.Image;
            }

            ModelState.ClearValidationState(nameof(actor));
            if (TryValidateModel(actor, nameof(actor)))
            {
                try
                {
                    _context.Update(actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorExists(actor.Id))
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
            return View(actor);
        }

        // GET: Actor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Actor == null)
            {
                return NotFound();
            }

            var actor = await _context.Actor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return View(actor);
        }

        // POST: Actor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Actor == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Actor'  is null.");
            }
            var actor = await _context.Actor.FindAsync(id);
            if (actor != null)
            {
                _context.Actor.Remove(actor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorExists(int id)
        {
          return _context.Actor.Any(e => e.Id == id);
        }
    }
}
