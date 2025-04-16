using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Core.Domain.Exceptions;

namespace Core.Domain.ModelDTO
{
    public class CurrentUserModel
    {
        public Guid UserId { get; set; }
        public long SessionId { get; set; }

        public static CurrentUserModel GetUser(ClaimsPrincipal User)
        {
            try
            {
                var u = new CurrentUserModel();
                u.UserId = Guid.Parse(User.Claims.First(e => e.Type == "user_id").Value);
                u.SessionId = long.Parse(User.Claims.First(e => e.Type == "session_id").Value);

                return u;
            }
            catch
            {
                throw new APIException("", HttpStatusCode.Unauthorized);
            }
        }
    }
}