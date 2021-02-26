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
    public class IndividualsController : ControllerBase
    {
        private readonly ILogger<IndividualsController> _logger;
        private readonly IndividualsRepository _repository;

        public IndividualsController(
            ILogger<IndividualsController> logger, 
            IndividualsRepository individualsRepository
        )
        {
            _logger = logger;
            _repository = individualsRepository;
        }

        [HttpPost(Name = nameof(CreateIndividual))]
        [ProducesResponseType(typeof(Individual), StatusCodes.Status201Created)]
        public ActionResult<Individual> CreateIndividual([FromBody] IndividualViewModel data)
        {
            Individual individual = _repository.Create(data);

            return CreatedAtAction(nameof(Individual), new { Id = individual.Id }, individual);
        }

        [HttpGet(Name = nameof(ListIndividuals))]
        [ProducesResponseType(typeof(List<Individual>), StatusCodes.Status200OK)]
        public ActionResult<List<Individual>> ListIndividuals()
        {
            List<Individual> individuals = _repository
                .List(individual => true);
            
            return Ok(individuals);
        }
        
        [HttpGet("{id}", Name = nameof(ReadIndividual))]
        [ProducesResponseType(typeof(Individual), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Individual> ReadIndividual(Guid id)
        {
            Individual individual = _repository.Read(id);
            
            return Ok(individual);
        }

        [HttpPut("{id}", Name = nameof(UpdateIndividual))]
        [ProducesResponseType(typeof(Individual), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Individual> UpdateIndividual(Guid id, [FromBody] IndividualViewModel vm)
        {
            Individual individual = _repository.Update(id, vm);

            return Ok(individual);
        }

        [HttpDelete("{id}", Name = nameof(DeleteIndividual))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteIndividual(Guid id)
        {
            _repository.Delete(id);

            return NoContent();
        }
    }
}