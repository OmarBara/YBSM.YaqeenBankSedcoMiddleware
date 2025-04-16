using System;

namespace Core.Domain.Entities
{
    public  class AuthSession
    {
        public long Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ClientIpAddress { get; set; }
        public string UserAgent { get; set; }
        public string MetaData { get; set; }

        public Guid? MerchantId { get; set; }
        //public virtual  Merchant? Merchant { get; set; }

        public Guid? UserId { get; set; }
        public virtual  User? User { get; set; }
    }
}