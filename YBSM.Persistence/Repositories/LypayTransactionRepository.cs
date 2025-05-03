using Core.Application.Repositories;
using Core.Domain.Entities;
using Core.Domain.Enum;
using HRM.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YBSM.Core.Aplication.Interface;
using YBSM.Core.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace YBSM.Infrastructure.Persistence.Repositories
{
    public class LypayTransactionRepository : AsyncRepository<User>, ILypayTransactionRepository
    {
        private readonly AppDbContext _appDbContext;

        public LypayTransactionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task<List<LypayTransaction>> GetLypayTransaction()
        {

            //var query = "SELECT * FROM ALL_TABLES WHERE TABLE_NAME = 'V_LYPAY' AND OWNER = 'V_LYPAY'";
            var query = "SELECT * FROM V_LYPAY2";
            try
            {
                var res = _appDbContext.LypayTransaction
               .FromSqlRaw(query)
               .AsNoTracking()
               .ToListAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving Lypay transactions", ex);
            }
        }
       

        public Task<bool> AddTransaction(string transactionId, string orderId, string status, string amount, string currency, string paymentMethod, string customerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTransaction(string transactionId)
        {
            throw new NotImplementedException();
        }

        

        public Task<bool> UpdateTransaction(string transactionId, string status)
        {
            throw new NotImplementedException();
        }
    }
}
