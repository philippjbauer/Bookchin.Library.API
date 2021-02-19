using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Bookchin.Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
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

        [HttpGet(Name = nameof(GetUsers))]
        [ProducesResponseType(typeof(UserAccount), StatusCodes.Status200OK)]
        public ActionResult<List<UserAccount>> GetUsers()
        {
            List<UserAccount> userAccounts = _repository.List();
            
            return Ok(userAccounts);
        }

        [HttpGet("All", Name = nameof(GetAllUsers))]
        [ProducesResponseType(typeof(UserAccount), StatusCodes.Status200OK)]
        public ActionResult<List<UserAccount>> GetAllUsers()
        {
            List<UserAccount> userAccounts = _repository
                .List(userAccount => true);

            return Ok(userAccounts);
        }
    }
}