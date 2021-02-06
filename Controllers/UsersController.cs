using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Repositories;

namespace Bookchin.Library.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UsersRepository _repository;

        public UsersController(
            ILogger<UsersController> logger, 
            UsersRepository usersRepository
        )
        {
            _logger = logger;
            _repository = usersRepository;
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = _repository.List();
            
            return Ok(users);
        }

        [HttpGet]
        [Route("All")]
        public ActionResult<List<User>> GetAllUsers()
        {
            List<User> users = _repository
                .List(user => user.IsActive == false || user.IsActive == true);

            return Ok(users);
        }
    }
}