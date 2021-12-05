using DynamicContract.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DynamicContract.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        // GET: api/<PersonsController>
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return Mock.Persons;
        }

        // GET api/<PersonsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Person person = Mock.Persons.FirstOrDefault(x => x.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // POST api/<PersonsController>
        [HttpPost]
        public void Post([FromBody] Person model)
        {
        }

        // PUT api/<PersonsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Person model)
        {
        }

        // DELETE api/<PersonsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
