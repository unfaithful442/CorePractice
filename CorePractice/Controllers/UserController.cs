using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        private IMapper _mapper;
        public UserController(IMemoryCache cache, CorePracticeDbContext db, IMapper mapper)
        {
            _cache = cache;

            _db = db;

            _mapper = mapper;
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
            //check if username exist
            var dbuser = _db.Users.Where(u => u.Username == user.Username).FirstOrDefault();

            //username doesnt exist
            if (dbuser != null)
            {
                _db.Users.Add(user);

                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateUser), user);
            }
            else
            {
                //username already exist

                return Conflict(new { message = "Sorry, the Username has been used" });
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(User user)
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
        public async Task<IActionResult> DeleteUser(int id)
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

        [HttpPost]
        [Route("Group")]
        public async Task<IActionResult> SetUserGroups(int id)
        {


            return Ok();
        }


        [HttpDelete]
        [Route("Group/{id:int}")]
        public async Task<IActionResult> DeleteUserGroups(int id)
        {
            var UserGroups = _db.UserGroups.Where(ug => ug.UserId == id).ToList();

            _db.UserGroups.RemoveRange(UserGroups);

            await _db.SaveChangesAsync();

            return Ok(UserGroups);
        }
    }
}