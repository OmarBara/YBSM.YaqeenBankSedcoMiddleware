using System.Threading.Tasks;
using Core.Application.Repositories;
using HRM.Persistence;
using YBSM.Core.Aplication.Interface;
using YBSM.Infrastructure.Persistence.Repositories;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private IUserRepository _userRepository;

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository != null)
                    return _userRepository;
                return _userRepository = new UserRepository(_appDbContext);
            }
        }

        private IAuthSessionRepository _authSessionRepository;

        public IAuthSessionRepository AuthSessionRepository
        {
            get
            {
                if (_authSessionRepository != null)
                    return _authSessionRepository;
                return _authSessionRepository = new AuthSessionRepository(_appDbContext);
            }
        }

         private ILypayTransactionRepository _lypayTransactionRepository;

         public ILypayTransactionRepository LypayTransactionRepository
        {
             get
             {
                 if (_lypayTransactionRepository != null)
                     return _lypayTransactionRepository;
                 return _lypayTransactionRepository = new LypayTransactionRepository(_appDbContext);
             }
         }


        /*private IStoreInfoRepository _storeInfoRepository;

        public IStoreInfoRepository StoreInfoRepository
        {
            get
            {
                if (_storeInfoRepository != null)
                    return _storeInfoRepository;
                return _storeInfoRepository = new StoreInfoRepository(_appDbContext);
            }
        }




        private IMerchantRepository _merchantRepository;

        public IMerchantRepository MerchantRepository
        {
            get
            {
                if (_merchantRepository != null)
                    return _merchantRepository;
                return _merchantRepository = new MerchantRepository(_appDbContext);
            }
        }


        private IImageStoreRepository _imageStoreRepository;

        public IImageStoreRepository ImageStoreRepository
        {
            get
            {
                if (_imageStoreRepository != null)
                    return _imageStoreRepository;
                return _imageStoreRepository = new ImageStoreRepository(_appDbContext);
            }
        }



        private IActivityRepository _activityRepository;

        public IActivityRepository ActivityRepository
        {
            get
            {
                if (_activityRepository != null)
                    return _activityRepository;
                return _activityRepository = new ActivityRepository(_appDbContext);
            }
        }
        */

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }


        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}