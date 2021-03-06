using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Repositories;
using Bookchin.Library.API.Controllers.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace Bookchin.Library.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("UserAccounts/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        [HttpPost(Name = nameof(CreateOrganization))]
        [ProducesResponseType(typeof(Organization), StatusCodes.Status201Created)]
        public ActionResult<Organization> CreateOrganization([FromBody] OrganizationViewModel data)
        {
            Organization organization = _repository.Create(data);

            return CreatedAtRoute(nameof(Organization), new { Id = organization.Id }, organization);
        }
        
        [HttpGet(Name = nameof(ListOrganizations))]
        [ProducesResponseType(typeof(Organization), StatusCodes.Status200OK)]
        public ActionResult<List<Organization>> ListOrganizations()
        {
            List<Organization> organizations = _repository
                .List(organization => true);
            
            return Ok(organizations);
        }
        
        [HttpGet("{id}", Name = nameof(ReadOrganization))]
        [ProducesResponseType(typeof(Organization), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Organization> ReadOrganization(Guid id)
        {
            Organization organization = _repository.Read(id);
            
            return Ok(organization);
        }

        [HttpPut("{id}", Name = nameof(UpdateOrganization))]
        [ProducesResponseType(typeof(Organization), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Organization> UpdateOrganization(Guid id, [FromBody] OrganizationViewModel vm)
        {
            Organization organization = _repository.Update(id, vm);

            return Ok(organization);
        }

        [HttpDelete("{id}", Name = nameof(DeleteOrganization))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteOrganization(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}