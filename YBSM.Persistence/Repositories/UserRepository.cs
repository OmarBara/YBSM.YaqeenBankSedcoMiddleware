using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Repositories;
using Core.Domain.Entities;
using Core.Domain.Enum;
using HRM.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : AsyncRepository<User>, IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<List<User>> GetListAsync()
            => _appDbContext.Users.Where(o => o.UserRole != Roles.Admin)
                .ToListAsync();

        public Task<User> GetAsync(Guid id)
            => _appDbContext.Users
                .FirstOrDefaultAsync(o => o.Id == id);

        public Task<User> FindAccount(string email)
            => _appDbContext.Users
                .FirstOrDefaultAsync(o => o.Email == email);
    }
}