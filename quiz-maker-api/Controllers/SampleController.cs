using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiz_maker_api.Logics;

namespace quiz_maker_api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize]
    public class SampleController : ControllerBase
    {
        public SampleController()
        {
        }

        [HttpGet]
        public IActionResult Search(string search)
        {
            return Ok(SampleOperation.StaticStrings.Where(x => x.String.Contains(search??string.Empty)).ToList());
        }

        [HttpGet("{Id}")]
        public IActionResult Find(int Id)
        {
            return Ok(SampleOperation.StaticStrings.FirstOrDefault(x => x.Id == Id));
        }

        [HttpPost]
        public IActionResult Add([FromBody] SampleRequestBody body)
        {
            var maxIndex = !SampleOperation.StaticStrings.Any() ? 1 : SampleOperation.StaticStrings.Max(x => x.Id)+1;
            body.Id = maxIndex;
            SampleOperation.StaticStrings.Add(body);
            return Ok(body);
        }

        [HttpPut]
        public IActionResult Update([FromBody] SampleRequestBody body)
        {
            if (!SampleOperation.StaticStrings.Any(x => x.Id == body.Id))
            {
                throw new LogicException("Not found Id");
            }

            SampleOperation.StaticStrings.FirstOrDefault(x => x.Id == body.Id).String = body.String;
            return Ok(body);
        }

        [HttpDelete("{Id}")]
        public IActionResult Remove(int Id)
        {
            if (!SampleOperation.StaticStrings.Any(x => x.Id == Id))
            {
                throw new LogicException("Not found Id");
            }

            SampleOperation.StaticStrings.RemoveAll(x => x.Id == Id);
            return Ok(Id);
        }
    }

    public class SampleRequestBody
    {
        public int Id { get; set; }
        public string String { get; set; }
    }

    public class SampleOperation
    {
        public static List<SampleRequestBody> StaticStrings { get; set; } = new List<SampleRequestBody>();
    }
}