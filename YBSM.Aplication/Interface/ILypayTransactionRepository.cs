using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBSM.Core.Domain.Entities;
using YBSM.Core.Domain.ModelDTO.RequestDTO;
using YBSM.Core.Domain.ModelDTO.ResponceDTO;

namespace YBSM.Core.Aplication.Interface
{
    public interface ILypayTransactionRepository
    {
        Task<bool> AddTransaction(string transactionId, string orderId, string status, string amount, string currency, string paymentMethod, string customerEmail);
        Task<bool> UpdateTransaction(string transactionId, string status);
        Task<bool> DeleteTransaction(string transactionId);
        //Task<List<LypayTransaction>> GetLypayTransaction();
        Task<List<LypayTransactionDBResponceDto>> GetLypayTransactionProceduer(Details request);
    }
}
