using System;
using System.Threading.Tasks;
using YBSM.Core.Aplication.Interface;

namespace Core.Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        
        IAuthSessionRepository AuthSessionRepository { get; }        
        IUserRepository UserRepository{ get; }
        ILypayTransactionRepository LypayTransactionRepository { get; }
        Task SaveChangesAsync();
    }
}