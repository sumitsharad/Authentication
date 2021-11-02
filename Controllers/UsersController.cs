﻿using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    /// <summary>
    /// Used for creating customize error file and errors can be registerd as a log entry in the windows event log
    /// </summary>
    public class UsersController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IUserService _userService;
        /// <summary>
        /// injection in home controller
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// initialization of authentication process.
        /// </summary>
        /// <param name="model"></param>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            log.Info("Authenticating User");
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
            {
                log.Error("incorrect username or password");
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(user);
        }
        /// <summary>
        /// Check and Authorize is admin or not.
        /// </summary>

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        /// <summary>
        /// only allow admins to access other user records
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
                return Forbid();

            var user = _userService.GetById(id);

            if (user == null)
            {
                log.Error("User not found");
                return NotFound();
            }
            return Ok(user);
        }
        /// <summary>
        /// Admin Role to create a User Acoount
        /// </summary>
        /// <param name="user"></param>
        /// <returns>It will reflect creation status</returns>
        [Authorize(Roles = Role.Admin)]
        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] User user)
        {
            var userCreationStatus = _userService.CreateUser(user);

            if (userCreationStatus == null)
            {
                log.Error("Unable to create user");
                return BadRequest(new { message = "User not created" });
            }
            log.Info("User Created");
            return Ok(userCreationStatus);
        }
    }
}