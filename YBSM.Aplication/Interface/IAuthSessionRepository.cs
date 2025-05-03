using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Application.Repositories
{
    public interface IAuthSessionRepository : IAsyncRepository<AuthSession>
    {
        //Task<AuthSession> GetAuthSessionsWithMerchantAsync(long id);
        Task<AuthSession> GetAuthSessionsWithUserAsync(long id);
    }
}