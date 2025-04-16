using System;
using System.Threading.Tasks;

namespace Core.Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        //ICategoryRepository CategoryRepository { get; }
        //IStoreInfoRepository StoreInfoRepository { get; }
        IAuthSessionRepository AuthSessionRepository { get; }
        //IMerchantRepository MerchantRepository { get; }
        //IImageStoreRepository ImageStoreRepository { get; }
        //IActivityRepository ActivityRepository { get; }
        IUserRepository UserRepository{ get; }
        Task SaveChangesAsync();
    }
}