using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Bookchin.Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserAccountsController : ControllerBase
    {
        private readonly ILogger<UserAccountsController> _logger;
        private readonly UserAccountsRepository _repository;

        public UserAccountsController(
            ILogger<UserAccountsController> logger, 
            UserAccountsRepository userAccountsRepository
        )
        {
            _logger = logger;
            _repository = userAccountsRepository;
        }

        [HttpGet]
        public ActionResult<List<UserAccount>> GetUsers()
        {
            List<UserAccount> userAccounts = _repository.List();
            
            return Ok(userAccounts);
        }

        [HttpGet]
        [Route("All")]
        public ActionResult<List<UserAccount>> GetAllUsers()
        {
            List<UserAccount> userAccounts = _repository
                .List(userAccount => true);

            return Ok(userAccounts);
        }
    }
}