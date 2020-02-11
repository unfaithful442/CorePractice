using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorePractice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CorePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IMemoryCache _cache;

        private CorePracticeDbContext _db;

        public UserController(IMemoryCache cache, CorePracticeDbContext db)
        {
            _cache = cache;

            _db = db;
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            var Users = _db.Users.OrderBy(m => m.UserId).ToList();

            return Ok(Users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            _db.Users.Add(user);

            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateUser), user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup(User user)
        {
            _db.Entry(user).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var User = _db.Users.Where(m => m.UserId == id).FirstOrDefault();

            if (User == null)
            {
                return NotFound();
            }

            _db.Users.Remove(User);

            await _db.SaveChangesAsync();

            return Ok(User);
        }
    }
}