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
    public class GroupController : ControllerBase
    {
        private IMemoryCache _cache;

        private CorePracticeDbContext _db;

        //inject db context and memory cache
        public GroupController(IMemoryCache cache, CorePracticeDbContext db)
        {
            _cache = cache;

            _db = db;
        }


        [HttpGet]
        public IActionResult GetGroups()
        {
            var Group = _db.Groups.OrderBy(m => m.GroupId).ToList<Group>();

            return Ok(Group);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            _db.Groups.Add(group);

            await _db.SaveChangesAsync();

            return CreatedAtAction("CreateGroup", group);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup(Group group)
        {
            _db.Entry(group).State = EntityState.Modified;

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
            var Group = _db.Groups.Where<Group>(m => m.GroupId == id).FirstOrDefault();

            if (Group == null)
            {
                return NotFound();
            }

            _db.Groups.Remove(Group);

            await _db.SaveChangesAsync();

            return Ok(Group);
        }
    }
}