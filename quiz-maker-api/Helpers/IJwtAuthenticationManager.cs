using quiz_maker_models.OutfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.Helpers
{
    public interface IJwtAuthenticationManager
    {
        Task<AuthorizedUserModel> Authenticate(LoginOutfaceModel model);
        Task<AuthorizedUserModel> RefreshToken(RefreshTokenOutfaceModel outfaceModel);
    }
}
