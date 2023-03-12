using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using quiz_maker_api.Helpers;
using quiz_maker_models;
using quiz_maker_models.Helpers;
using quiz_maker_models.Models;
using quiz_maker_models.OutfaceModels;
using quiz_maker_models.SearchModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace quiz_maker_api.Logics
{
    public class UserLogic : BaseLogic<User>, IJwtAuthenticationManager
    {
        public const string REFRESH_TOKEN_CLAIM = "REFRESH_TOKEN";

        public UserLogic(QuizMakerDbContext context, ICurrentScope current)
            : base(context, current)
        {

        }

        public async override Task<bool?> IsExistsAsync(User entity)
        {
            if (await _db.Users.AnyAsync(x => x.Username == entity.Username))
            {
                throw new LogicException(string.Format(ErrorRexs._MsgDuplicateObject_Coloumns, nameof(entity.Username)));
            }
            return false;
        }

        public override IQueryable<User> Search(ISearchModel search)
        {
            var param = search as UserSearchModel;
            var qUsers = _db.Users.Where(x => x.Id != 0);

            if (!string.IsNullOrEmpty(param.Search))
            {
                qUsers = qUsers.Where(x => (x.Name+","+x.Username).ToLower().Trim().Contains(param.Search.ToLower().Trim()));
            }
            if (param.Role != null)
            {
                qUsers = qUsers.Where(x => x.UserRole == param.Role);
            }

            if (param.ShowDeactivateUser)
            {
                qUsers = qUsers.Where(x => x.Active == false);
            }
            else
            {
                qUsers = qUsers.Where(x => x.Active);
            }

            return qUsers;
        }

        public async Task<AuthorizedUserModel> RefreshToken(RefreshTokenOutfaceModel refreshTokenOutface)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (string.IsNullOrWhiteSpace(refreshTokenOutface.RefreshToken))
            {
                throw new LogicException(string.Format(ErrorRexs._MsgValueCannotBeEmpty, nameof(refreshTokenOutface.RefreshToken)));
            }
            if (jwtHandler.CanReadToken(refreshTokenOutface.AuthorizeToken))
            {
                throw new LogicException(ErrorRexs.MsgInvalidToken);
            }

            var oldJwtToken = jwtHandler.ReadJwtToken(refreshTokenOutface.AuthorizeToken);
            var claim = oldJwtToken.Claims.ToList();
            var oldRefreshToken = claim.FirstOrDefault(x => x.Type == REFRESH_TOKEN_CLAIM)?.Value ?? string.Empty;
            var userId = Parse.ToInt(claim.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value ?? string.Empty);

            if (oldRefreshToken != refreshTokenOutface.RefreshToken)
            {
                throw new LogicException(ErrorRexs.MsgInvalidRefreshToken);
            }
            var user = await FindAsync(userId, false);
            if (user == null)
            {
                throw new LogicException(ErrorRexs.MsgTokenNotAssignToAnyUser);
            }

            var returnModel = createJwtToekn(user);

            return returnModel;
        }

        public async Task<AuthorizedUserModel> Authenticate(LoginOutfaceModel model)
        {
            var hashPassword = SecurityHelper.GetMd5HashPassword(model.Username, model.Password);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == model.Username
                                                             && x.HashPasswrd == hashPassword);
            if (user == null)
            {
                throw new UnAuthorizeException(ErrorRexs.MsgInvalidUsernameOrPassoword);
            }

            var returnModel = createJwtToekn(user);

            return returnModel;
        }

        private AuthorizedUserModel createJwtToekn(User user)
        {
            var expireDateTime = DateTime.UtcNow.AddHours(2);
            var refreshToken = Guid.NewGuid().ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Hash, user.HashPasswrd),
                    new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                    new Claim(REFRESH_TOKEN_CLAIM, refreshToken),
                    new Claim(ClaimTypes.Role, ((int)user.UserRole).ToString())
                }),
                Expires = expireDateTime,
                SigningCredentials = new SigningCredentials(
                    AppSetting.SECRET_KEY,
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var returnModel = new AuthorizedUserModel()
            {
                AuthorizeToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken,
                TokenExpireTime = expireDateTime,
                User = user
            };

            return returnModel;
        }

        public async override Task<bool?> CanRemoveAsync(User entity)
        {
            if (entity.Id == 1)
            {
                throw new LogicException(ErrorRexs.CannotRemoveSuperUser);
            }

            return true;
        }
    }
}
