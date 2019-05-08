using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twatter.Data;
using Twatter.Models;
using Twatter.ViewModels;

namespace Twatter.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Posts
        [Authorize]
        public async Task<IActionResult> Index(string toggle)
        {

            var user = await _userManager.GetUserAsync(User);

            var posts = _context.Post.OrderByDescending(m => m.Id).Include("User").AsQueryable();


            if (toggle == "following")
            {
                // get posts that from the users that are being followed
                var followingUserIds = _context
                    .Followings
                    .Where(m => m.UserId == user.Id)
                    .Select(m => m.UserFollowingId)
                    .ToList();

                posts = _context.Post.Where(m => followingUserIds.Contains(m.UserId) || m.UserId == user.Id).Include("User").OrderByDescending(m => m.Id);
            }
            else if (toggle == "all")
            {
                posts = _context.Post.OrderByDescending(m => m.Id).Include("User").AsQueryable();
            }

          

            var viewModel = new PostsListViewModel
            {
                //Posts = await _context.Post.OrderByDescending(m => m.Id).Include("User").ToListAsync(),
                Posts = posts,
                NewPost = new Post()

            };
            return View(viewModel);


        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(include: "NewPost")] PostsListViewModel viewModel)
        {

            var post = new Post(viewModel.NewPost.Content, viewModel.NewPost.User.Id);

            //var post = viewModel.NewPost;
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Likes,Retweets")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
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
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }

        public async Task<IActionResult> LikePost(int id)
        {

            var user = await _userManager.GetUserAsync(User);
            var post = _context.Post.Single(m => m.Id == id);


            if (_context.TwatVotes.Any(m => m.UserId == user.Id && m.PostId == id))
            {
                //already liked so remove

                var userLike = _context.TwatVotes.Single(m => m.UserId == user.Id && m.PostId == id);
                post.TwatVotes--;
                _context.TwatVotes.Remove(userLike);
            }
            else
            {
                post.TwatVotes++;

                var postLike = new TwatVotes
                {
                    PostId = post.Id,
                    UserId = user.Id
                };

                _context.TwatVotes.Add(postLike);


            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //public IActionResult NewPost(string content)
        //{
        //    var post = new Post(content);

        //    _context.Add(post);
        //    _context.SaveChanges();

        //    return RedirectToAction("Index");
        //}
        public async Task<IActionResult> Follow(string userId)
        {
            var user = await _userManager.GetUserAsync(User);

            var following = new Following
            {
                UserId = user.Id,
                UserFollowingId = userId
            };
            _context.Add(following);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}