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
using CorePractice.ViewModels;


namespace CorePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IMemoryCache _cache;

        private CorePracticeDbContext _db;

        private IMapper _mapper;
        //inject db context and memory cache
        public GroupController(IMemoryCache cache, CorePracticeDbContext db, IMapper mapper)
        {
            _cache = cache;

            _db = db;

            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult GetGroups([FromQuery]int pageSize, [FromQuery]int pageNumber)
        {
            var Group = _db.Groups.OrderBy(m => m.GroupId).ToList();

            var selectedGroup = Group.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return Ok(selectedGroup);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(BackEndCreateGroup backEndCreateGroup)
        {
            //viewmodel failed to validate
            if (!ModelState.IsValid)
            {
                var error = new
                {
                    message = "The request is invalid.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };

                //viewmodel validation failed
                return BadRequest(error);
            }
            else
            {

                var group = _mapper.Map<BackEndCreateGroup, Group>(backEndCreateGroup);

                _db.Groups.Add(group);

                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateGroup), group);
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup(BackEndUpdateGroup backEndUpdateGroup)
        {
            //viewmodel failed to validate
            if (!ModelState.IsValid)
            {
                var error = new
                {
                    message = "The request is invalid.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };

                //viewmodel validation failed
                return BadRequest(error);
            }
            else
            {
                var group = _db.Groups.Where(m => m.GroupId == backEndUpdateGroup.GroupId).FirstOrDefault();

                group.GroupName = backEndUpdateGroup.GroupName;

                group.Description = backEndUpdateGroup.Description;

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