using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiz_maker_models.Models;
using quiz_maker_models.SearchModels;

namespace quiz_maker_api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize]
    public class MasterController : ControllerBase
    {

    }

    public class EmptyBaseController : MasterController
    {

    }

    public class BaseController<T> : MasterController where T : BaseModel
    {
        [HttpPost]
        public async virtual Task<ActionResult<T>> AddAsync([FromBody]T entity)
        {
            return Ok(await Task<T>.FromResult(entity));
        }

        [HttpPut]
        public async virtual Task<ActionResult<T>> UpdateAsync([FromBody]T entity)
        {
            return Ok(await Task<T>.FromResult(entity));
        }

        [HttpDelete("{Id}")]
        public async virtual Task<ActionResult<T>> RemoveAsync(int Id)
        {
            return Ok();
        }

        [HttpGet("{Id}/{Include}")]
        public async virtual Task<ActionResult<T>> FindAsync(int Id, bool Include = false)
        {
            return Ok(await Task<T>.FromResult(new BaseModel()));
        }

        [HttpGet("CanRemove/{Id}")]
        public async virtual Task<ActionResult<bool>> CanRemoveAsync(int Id)
        {
            return Ok(await Task.FromResult(false));
        }

        [HttpGet("IsExists/{Id}")]
        public async virtual Task<ActionResult<bool>> IsExistsAsync([FromBody]T Id)
        {
            return Ok(await Task.FromResult(true));
        }
    }
}