using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_API.Models;

namespace WEB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DbController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DbController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authors
        [HttpGet]
        [Route("/authors/all")]
        public IEnumerable<Author> GetAllAuthors()
        {
            return _context.Authors
                .Include(author => author.Posts)
                .ToList();
        }
        // GET: Author
        [HttpGet("/authors/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = _context.Authors
                .Include(a => a.Posts)
                .FirstOrDefault(a=> a.Id == id);


            if (author == null)
            {
                return NotFound();
            }
            
            return Ok(author);
        }

        // POST: Authors
        [HttpPost]
        [Route("/authors")]
        public async Task<ActionResult<Author>> PostAuthor( Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllAuthors), author);
        }
        // PUT: Authors
        [HttpPut("/authors/{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if(id != author.Id)
            {
                return BadRequest();
            }
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return NoContent();


        }
        // DELETE: Authors
        [HttpDelete("/authors/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {

            var author = await _context.Authors.FindAsync(id);
            var posts = _context.Posts.Where(post => post.AuthorId == author.Id);
            foreach (var post in posts) {
                _context.Posts.Remove(post);
            }
            
            if(author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*Section regarding posts*/

        // GET: Posts
        [HttpGet]
        [Route("/posts")]
        public IActionResult GetAllPosts()
        {
            var posts = _context.Posts
                .Include(post => post.Author)
                .Select(post => new
                {
                    id = post.id,
                    title = post.title,
                    content = post.content,
                    description = post.description,
                    AuthorName = post.Author.Name,
                    authorId = post.Author.Id
                }).ToList();
            return Ok(posts);
        }
        // GET: Post
        [HttpGet("/posts/{id}")]
        public IActionResult GetPostById(int id)
        {
            var post = _context.Posts
                .Include(p => p.Author)
                .FirstOrDefault(p => p.id == id);

            if(post == null)
            {
                return NotFound();
            }

            

            return Ok(post);
        }

        // POST: Post
        [HttpPost("/posts")]
        public async Task<ActionResult<Post>> CreatePost(int authorId, Post post)
        {
            if(post == null)
            {
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(post.AuthorId);
            if(author == null)
            {
                return NotFound();
            }

            var postTemp = new Post
            {
                title = post.title,
                content = post.content,
                description = post.description,
                Author = author
            };

            _context.Posts.Add(postTemp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllPosts), postTemp); 
        }

        // PUT: Post
        [HttpPut("/posts/{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if(id != post.id)
            {
                return BadRequest();
            }
            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
            }
            return NoContent();
        }


       
        private bool AuthorExists(int id)
        {
          return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
