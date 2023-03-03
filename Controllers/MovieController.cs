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

namespace Assignment_3.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITweetWrapper _tweetWrapper;

        public MovieController(ApplicationDbContext context, ITweetWrapper tweetWrapper)
        {
            _context = context;
            _tweetWrapper = tweetWrapper;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
              return View(await _context.Movie.ToListAsync());
        }

        public async Task<IActionResult> GetMoviePhoto(int id)
        {
            var movie = await _context.Movie.FirstOrDefaultAsync(a => a.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return File(movie.Poster, "image/jpg");
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }



            var movieActorsVM = new MovieActorsVM();
            movieActorsVM.Movie = movie;
            //Searches for all actors that are associated with the movie
            movieActorsVM.Actors = await _context.ActorMovie.Where(am => am.Movie.Id == movie.Id).Select(am => am.Actor).ToListAsync();

            //Searches for all tweets that are associated with the movie
             await _tweetWrapper.GetTweetsAsync(movie);
            movieActorsVM.TweetWrapper = (TweetWrapper)_tweetWrapper;

            return View(movieActorsVM);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IMBDLink,Genre,ReleaseDate,Poster")] Movie movie, IFormFile poster)
        {
            if (ModelState.IsValid)
            {
                if (poster != null && poster.Length > 0)
                {   
                    var memoryStream = new MemoryStream();
                    await poster.CopyToAsync(memoryStream);
                    movie.Poster = memoryStream.ToArray();
                }

                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IMBDLink,Genre,ReleaseYear")] Movie movie, IFormFile poster)
        {


            if (id != movie.Id)
            {
                return NotFound();
            }

            if(poster == null)
            {
            
               ModelState.Remove("Poster");
               var movieDb = await _context.Movie.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
               movie.Poster = movieDb.Poster;
            }

           if (ModelState.IsValid)
           {

                if (poster != null && poster.Length > 0)
                {
                    var memoryStream = new MemoryStream();
                    await poster.CopyToAsync(memoryStream);
                    movie.Poster = memoryStream.ToArray();
                }

                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return _context.Movie.Any(e => e.Id == id);
        }
    }
}
