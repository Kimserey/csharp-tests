using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EFTest.Controllers
{
    public class CommaSeparatedArrayAttribute : ModelBinderAttribute
    {
        public CommaSeparatedArrayAttribute()
        {
            BinderType = typeof(CommaSeparatedArrayModelBinder);
        }

        public class CommaSeparatedArrayModelBinder : IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                bindingContext.Result = ModelBindingResult.Success(
                    bindingContext.ValueProvider
                        .GetValue(bindingContext.ModelName)
                        .FirstValue
                        .Split(',')
                );
                return Task.CompletedTask;
            }
        }
    }

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet("EntityTaggingRules(Entity:{entity}|Tags:{tags})")]
        public IActionResult Get(string entity, [CommaSeparatedArray] string[] tags)
        {
            return Ok(new { entity, tags });
        }

        [HttpGet("EntityTaggingRules(Entity:{entity})/Schema")]
        public IActionResult Get(string entity)
        {
            return Ok(new { entity });
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
