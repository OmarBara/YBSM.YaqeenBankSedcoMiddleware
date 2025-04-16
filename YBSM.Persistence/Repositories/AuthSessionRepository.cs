using System.Threading.Tasks;
using Core.Application.Repositories;
using Core.Domain.Entities;
using HRM.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class AuthSessionRepository : AsyncRepository<AuthSession>, IAuthSessionRepository
    {
        private readonly AppDbContext _appDbContext;

        public AuthSessionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }


        /* public Task<AuthSession> GetAuthSessionsWithMerchantAsync(long id)
             => _appDbContext.AuthSessions.Include(o => o.Merchant)
                .FirstOrDefaultAsync(o => o.Id == id);    
         */
        public Task<AuthSession> GetAuthSessionsWithUserAsync(long id)
            => _appDbContext.AuthSessions.Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
    }
}