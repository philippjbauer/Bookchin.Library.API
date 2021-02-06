using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Repositories;
using Bookchin.Library.API.Controllers.ViewModels;

namespace Bookchin.Library.API.Controllers
{
    [ApiController]
    [Route("Users/[controller]")]
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

        [HttpPost]
        public ActionResult<Individual> CreateIndividual([FromBody] IndividualViewModel data)
        {
            Individual individual = _repository.Create(data);

            return Ok(individual);
        }
        
        [HttpGet]
        public ActionResult<List<Individual>> ListIndividuals()
        {
            List<Individual> individuals = _repository
                .List(individual => individual.IsActive == true || individual.IsActive == false);
            
            return Ok(individuals);
        }
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<Individual> ReadIndividual(Guid id)
        {
            Individual individual = _repository.Read(id);
            
            return Ok(individual);
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<Individual> UpdateIndividual(Guid id, [FromBody] IndividualViewModel vm)
        {
            Individual individual = _repository.Update(id, vm);

            return Ok(individual);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteIndividual(Guid id)
        {
            _repository.Delete(id);

            return Ok();
        }
    }
}