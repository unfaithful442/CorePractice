using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorePractice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetUsers()
        {
            return Ok();
        }

    }
}