using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Application.Repositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<List<User>> GetListAsync();
        Task<User> GetAsync(Guid id);

        Task<User> FindAccount(string email);
    }
}