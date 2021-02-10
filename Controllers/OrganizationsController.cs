using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Repositories;
using Bookchin.Library.API.Controllers.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Bookchin.Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("UserAccounts/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly ILogger<OrganizationsController> _logger;
        private readonly OrganizationsRepository _repository;

        public OrganizationsController(
            ILogger<OrganizationsController> logger, 
            OrganizationsRepository organizationsRepository
        )
        {
            _logger = logger;
            _repository = organizationsRepository;
        }

        [HttpPost]
        public ActionResult<Organization> CreateOrganization([FromBody] OrganizationViewModel data)
        {
            Organization organization = _repository.Create(data);

            return Ok(organization);
        }
        
        [HttpGet]
        public ActionResult<List<Organization>> ListOrganizations()
        {
            List<Organization> organizations = _repository
                .List(organization => true);
            
            return Ok(organizations);
        }
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Organization> ReadOrganization(Guid id)
        {
            Organization organization = _repository.Read(id);
            
            return Ok(organization);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<Organization> UpdateOrganization(Guid id, [FromBody] OrganizationViewModel vm)
        {
            Organization organization = _repository.Update(id, vm);

            return Ok(organization);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteOrganization(Guid id)
        {
            _repository.Delete(id);

            return Ok();
        }
    }
}