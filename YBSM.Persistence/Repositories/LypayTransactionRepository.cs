using Core.Application.Repositories;
using Core.Domain.Entities;
using Core.Domain.Enum;
using HRM.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YBSM.Core.Aplication.Interface;
using YBSM.Core.Domain.Entities;
using YBSM.Core.Domain.ModelDTO.RequestDTO;
using YBSM.Core.Domain.ModelDTO.ResponceDTO;
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

    /*    public Task<List<LypayTransaction>> GetLypayTransaction()
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
        }*/

       public async Task<List<LypayTransactionDBResponceDto>> GetLypayTransactionProceduer(Details request)            
        {

            try
            {
                /* string query = @"
                 BEGIN
                    GetLypayTransactionTable(
                        p_RRN => :p0,
                        p_STAN => :p1,
                        p_TXNAMT => :p2,
                        p_TERMID => :p3,
                        p_SETLDATE => :p4,
                        p_TransactionData => :p_TransactionData_out -- Placeholder for the OUT cursor
                    );
                 END;";

                 // Define the parameters for the stored procedure call.
                 // Use OracleParameter if you need specific Oracle data types or directions.
                 // For IN parameters mapped from C# strings to Oracle VARCHAR2, simple DbParameter might suffice,
                 // but using OracleParameter is safer and more explicit.
                 var parameters = new object[]
                 {
                     new OracleParameter("p0", OracleDbType.Varchar2) { Value = request.RRN ?? (object)DBNull.Value },
                     new OracleParameter("p1", OracleDbType.Varchar2) { Value = request.STAN ?? (object)DBNull.Value },
                     new OracleParameter("p2", OracleDbType.Varchar2) { Value = request.TXNAMT ?? (object)DBNull.Value },
                     new OracleParameter("p3", OracleDbType.Varchar2) { Value = request.TERMID ?? (object)DBNull.Value },
                     new OracleParameter("p4", OracleDbType.Varchar2) { Value = request.SETLDATE ?? (object)DBNull.Value },
                     // Define the OUT cursor parameter. EF Core expects this for result sets.
                     // The name ':p_TransactionData_out' in the SQL string should match the parameter name here,
                     // and the OracleDbType should be RefCursor.
                     new OracleParameter("p_TransactionData_out", OracleDbType.RefCursor) { Direction = ParameterDirection.Output }
                 };

                 // Execute the stored procedure using FromSqlRaw.
                 // EF Core will map the result set from the OUT cursor to the LypayTransaction entity.
                 var res = await _appDbContext.LypayTransaction
                     .FromSqlRaw(query, parameters)
                     .AsNoTracking() // Use AsNoTracking() if you are not modifying the retrieved entities
                     .ToListAsync();*/

                string query = @"
                    SELECT * FROM TABLE(GetLypayTransaction(
                        p_RRN => :p_RRN,
                        p_STAN => :p_STAN,
                        p_TXNAMT => :p_TXNAMT,
                        p_TERMID => :p_TERMID,
                        p_SETLDATE => :p_SETLDATE
                    ))";

                // Define the parameters for the function call.
                // These correspond to the IN parameters of your Oracle function.
                // Use OracleParameter for explicit type mapping and handling potential nulls.
                var parameters = new object[]
                {
                    new OracleParameter("p_RRN", OracleDbType.Varchar2) { Value = request.RRN ?? (object)DBNull.Value },
                    new OracleParameter("p_STAN", OracleDbType.Varchar2) { Value = request.STAN ?? (object)DBNull.Value },
                    new OracleParameter("p_TXNAMT", OracleDbType.Varchar2) { Value = request.TXNAMT ?? (object)DBNull.Value },
                    new OracleParameter("p_TERMID", OracleDbType.Varchar2) { Value = request.TERMID ?? (object)DBNull.Value },
                    new OracleParameter("p_SETLDATE", OracleDbType.Varchar2) { Value = request.SETLDATE ?? (object)DBNull.Value }
                // Note: There is no OUT parameter for the result set here,
                // as the result comes directly from the SELECT * FROM TABLE(...) structure.
                 };

              
                var res = await _appDbContext.LypayTransaction
                    .FromSqlRaw(query, parameters)
                    .AsNoTracking() // Use AsNoTracking() if you are not modifying the retrieved entities
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
