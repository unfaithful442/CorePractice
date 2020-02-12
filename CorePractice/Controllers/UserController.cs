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
using CorePractice.Helpers;

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
        public IActionResult GetUsers([FromQuery]int pageSize, [FromQuery]int pageNumber)
        {
            var Users = _db.Users.OrderBy(m => m.UserId).ToList();

            var selectedUser = Users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return Ok(selectedUser);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(BackEndCreateUser backEndCreateUser)
        {
            //validate viewmodel
            if (ModelState.IsValid)
            {
                //check if username exist
                var dbuser = _db.Users.Where(u => u.Username == backEndCreateUser.Username).FirstOrDefault();

                //username doesnt exist
                if (dbuser != null)
                {
                    //encrypt pwd with salt

                    string salt = string.Empty;

                    string encryptPassword = SecurityHelper.EncryptPassword(backEndCreateUser.Password, out salt);

                    //map viewmodel to dbmodel
                    var user = _mapper.Map<BackEndCreateUser, User>(backEndCreateUser);

                    user.Salt = salt;

                    user.Password = encryptPassword;

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
            else
            {
                var error = new
                {
                    message = "The request is invalid.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };

                //viewmodel validation failed
                return BadRequest(error);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(BackEndUpdateUser backEndUpdateUser)
        {
            //failed to validate
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
                //update user

                var user = _db.Users.Where(m => m.UserId == backEndUpdateUser.UserId).FirstOrDefault();

                //cant find user by ID
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    string salt = string.Empty;

                    string encryptPassword = SecurityHelper.EncryptPassword(backEndUpdateUser.Password, out salt);

                    //map viewmodel to dbmodel

                    #region
                    user.Salt = salt;

                    user.Password = encryptPassword;

                    user.Firstname = backEndUpdateUser.Firstname;

                    user.Lastname = backEndUpdateUser.Lastname;

                    user.DateOfBirth = backEndUpdateUser.DateOfBirth;

                    user.Email = backEndUpdateUser.Email;

                    user.Phone = backEndUpdateUser.Phone;

                    user.Mobile = backEndUpdateUser.Mobile;

                    #endregion

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

            }

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

        [HttpPut]
        [Route("Group")]
        public async Task<IActionResult> SetUserGroup(int userId, Group group)
        {

            var groupUser = _db.UserGroups.Where(m => m.UserId == userId && m.GroupId == group.GroupId).FirstOrDefault();

            if (groupUser != null)
            {
                //user already have this group

                return Conflict(new { message = "The User is already in the group" });
            }
            else
            {
                UserGroup userGroup = new UserGroup();

                userGroup.UserId = userId;

                userGroup.GroupId = group.GroupId;

                _db.UserGroups.Add(userGroup);

                await _db.SaveChangesAsync();

                return NoContent();
            }


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