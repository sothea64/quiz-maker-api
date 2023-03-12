using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiz_maker_api.Logics;
using quiz_maker_models.ListModels;
using quiz_maker_models.Models;
using quiz_maker_models.SearchModels;

namespace quiz_maker_api.Controllers
{
    public class UserController : BaseController<User>
    {
        UserLogic _logic;

        public UserController(UserLogic logic)
        {
            _logic = logic;
        }

        public async override Task<ActionResult<User>> AddAsync([FromBody]User entity)
        {
            return Ok(await _logic.ExecuteAsync(() => _logic.AddAsync(entity)));
        }

        public async override Task<ActionResult<User>> UpdateAsync([FromBody] User entity)
        {
            return Ok(await _logic.ExecuteAsync(() => _logic.UpdateAsync(entity)));
        }

        public async override Task<ActionResult<User>> FindAsync(int Id, bool Include)
        {
            return Ok(await _logic.FindAsync(Id, Include));
        }

        [HttpGet]
        public async Task<ActionResult<List<Quiz>>> SearchAsync([FromQuery] UserSearchModel param)
        {
            return Ok(await _logic.SearchAsync(param));
        }

        [HttpGet("List")]
        public async Task<ActionResult<List<User>>> ListAsync([FromQuery] UserSearchModel param)
        {
            return Ok(await _logic.SearchAsync(param));
        }

        public async override Task<ActionResult<User>> RemoveAsync(int Id)
        {
            return Ok(await _logic.ExecuteAsync(() => _logic.RemoveAsync(Id)));
        }

        public async override Task<ActionResult<bool>> IsExistsAsync([FromBody]User entity)
        {
            return Ok(await _logic.IsExistsAsync(entity));
        }

        public async override Task<ActionResult<bool>> CanRemoveAsync(int Id)
        {
            return await _logic.CanRemoveAsync(Id);
        }
    }
}