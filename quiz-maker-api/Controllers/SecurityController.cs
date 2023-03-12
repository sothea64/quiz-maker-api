using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiz_maker_api.Logics;
using quiz_maker_models.OutfaceModels;

namespace quiz_maker_api.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        UserLogic _userLogic;
        public SecurityController(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthorizedUserModel>> AuthenticateAsync([FromBody] LoginOutfaceModel model)
        {
            return Ok(await _userLogic.Authenticate(model));
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<AuthorizedUserModel>> RefreshToken([FromBody] RefreshTokenOutfaceModel outfaceModel)
        {
            return Ok(await _userLogic.RefreshToken(outfaceModel));
        }
    }
}