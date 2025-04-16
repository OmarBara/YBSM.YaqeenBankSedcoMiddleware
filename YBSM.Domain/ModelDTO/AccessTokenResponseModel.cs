using System;

namespace Core.Domain.ModelDTO
{
    public class AccessTokenResponseModel
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string RefreshToken { get; set; }
        public long SessionId { get; set; }
    }
}