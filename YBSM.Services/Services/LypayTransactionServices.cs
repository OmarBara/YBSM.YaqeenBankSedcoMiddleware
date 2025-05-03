using Core.Application.Repositories;
using Core.Domain.Wrappers;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using YBSM.Core.Domain.Entities;
using YBSM.Core.Domain.ModelDTO.RequestDTO;
using YBSM.Core.Domain.ModelDTO.ResponceDTO;

namespace YBSM.Infrastructure.Services.Services
{
    public class LypayTransactionServices
    {
        private readonly IUnitOfWork _unitOfWork;


        public LypayTransactionServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }


        public async Task<OperationResult> GetLypayTransaction(LypayTransRequestDto lypayTransRequest)
        {
            //_logger.LogInformation("Get All Branch List by user {UserId}", id);
            try
            {
               
                // --- 1. Format Inputs ---
                string formattedTxnAmt;
                var details = lypayTransRequest.LookUpData.Details;
                lypayTransRequest.LookUpData.Details.TXNAMT = details.TXNAMT.PadLeft(12, '0');
                if (DateTime.TryParseExact(details.SETLDATE, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    lypayTransRequest.LookUpData.Details.SETLDATE = parsedDate.ToString("yyMMdd");
                }         
                            

                var trns = await _unitOfWork.LypayTransactionRepository.GetLypayTransaction();
                Console.WriteLine("Transaction Table: " + trns);

                
                var transactions = trns
                     .Where(t => t.Rrn == lypayTransRequest.LookUpData.Details.RRN  &&
                        t.TermId == lypayTransRequest.LookUpData.Details.TERMID &&
                        t.Stan == lypayTransRequest.LookUpData.Details.STAN &&
                        t.TxnAmt == lypayTransRequest.LookUpData.Details.TXNAMT &&
                        t.SetlDate == lypayTransRequest.LookUpData.Details.SETLDATE)
                    .ToList();               


                if (!transactions.Any())
                {
                    var messages = new List<string>();
                    var res = new InquiryResultResponceDto
                    {
                        Code = "E2",
                        Message = "Transaction not found"
                    };
                    return OperationResult<InquiryResultResponceDto>.UnValid(new List<string> { res.Message });

                }

                // Find the original transaction (assuming MSG_TYPE maps to a property like MessageType)
                var originalTransaction = transactions.FirstOrDefault(t => t.MsgType == "1200"); // Adjust property name 'MessageType' if needed

                if (originalTransaction == null)
                {
                    // Data inconsistency? Only reversal found?
                    return OperationResult<InquiryResultResponceDto>.Valid(
                         new InquiryResultResponceDto { Code = "R4", Message = "Original Transaction (1200) not Found" }
                    );
                }

                // --- 4. Determine Transaction Type ---
                string? transactionType = null;
                // Adjust property names 'ProcessingCode', 'ResponseCode', 'MessageType' according to your LypayTransaction entity
                if (originalTransaction.ProcCode?.StartsWith("0") ?? false)
                {
                    transactionType = "DEBIT";
                }
                else if (originalTransaction.ProcCode?.StartsWith("2") ?? false)
                {
                    transactionType = "CREDIT";
                }

                if (transactionType == null)
                {
                    // Not a type we handle for status check (e.g., inquiry)
                    return OperationResult<InquiryResultResponceDto>.Valid(
                        new InquiryResultResponceDto { Code = "R4", Message = "Transaction Found (Type Not DEBIT/CREDIT)" }
                   );
                }

                // --- 5. Determine Status ---
                bool isFailed = originalTransaction.RespCode != "000";

                if (isFailed)
                {
                    return OperationResult<InquiryResultResponceDto>.Valid(
                        new InquiryResultResponceDto { Code = "R1", Message = "Transaction is Failed", TransactionType = transactionType } // Assuming DTO has TransactionType
                   );
                }

                bool isReversed = transactions.Any(t => t.MsgType == "1400");

                if (isReversed)
                {
                    return OperationResult<InquiryResultResponceDto>.Valid(
                        new InquiryResultResponceDto { Code = "R2", Message = "Transaction is already Reversed", TransactionType = transactionType }
                   );
                }

                // If not Failed and not Reversed, it's Processed
                return OperationResult<InquiryResultResponceDto>.Valid(
                     new InquiryResultResponceDto { Code = "R3", Message = "Transaction is already Processed", TransactionType = transactionType }
                );
            


            }
            catch (OracleException ex) when (ex.Message.Contains("ORA-12545"))
            {
                return OperationResult<InquiryResultResponceDto>.UnValid("E8", "ORA-12545: TNS:destination host unreachable");
            }
            catch (Exception)
            {
                return OperationResult<InquiryResultResponceDto>.UnValid("E9", "Core bank system error");
            }
        }
    }
}
