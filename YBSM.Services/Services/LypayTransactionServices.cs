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

                //---- convert dinar to dirham
                decimal transactionAmountInDinar;
                // Attempt to parse the transaction amount string as a decimal
                if (!Decimal.TryParse(details.TXNAMT.Replace(',', '.'), NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out transactionAmountInDinar))
                {
                    // CHANGE: Using the new OperationResult<T>.UnValid if parsing fails
                    return OperationResult<List<TransactionResultItem>>.UnValid($"Invalid transaction amount format: {details.TXNAMT}. Expected a numeric value.");
                }

                // Convert the amount from Dinars to Dirhams (multiply by 1000 for 3 decimal places)
                // Then convert to a long integer to represent the total number of minor units.
                long transactionAmountInDirhams = (long)(transactionAmountInDinar * 1000);

                // Format this integer amount (in Dirhams) as a zero-padded string of length 12
                // This assumes the ISO 8583 amount field is a 12-digit integer representing the minor currency unit.
                details.TXNAMT = transactionAmountInDirhams.ToString().PadLeft(12, '0');

                var trns = await _unitOfWork.LypayTransactionRepository.GetLypayTransaction();
                //Console.WriteLine("Transaction Table: " + trns);

                
                var transactions = trns
                     .Where(t => t.Rrn == lypayTransRequest.LookUpData.Details.RRN  &&
                        t.TermId == lypayTransRequest.LookUpData.Details.TERMID &&
                        t.Stan == lypayTransRequest.LookUpData.Details.STAN &&
                        t.TxnAmt == lypayTransRequest.LookUpData.Details.TXNAMT &&
                        t.SetlDate == lypayTransRequest.LookUpData.Details.SETLDATE)
                    .ToList();               


                if (!transactions.Any())
                {
                    // If no transactions match the criteria, return a result indicating "not found"
                    var notFoundItem = new TransactionResultItem
                    {
                        Code = "R4",
                        Message = "Transaction not found",
                       // TransactionType = null // No transaction means no type
                    };
                    var resultList = new List<TransactionResultItem> { notFoundItem };
                    // CHANGE: Returning OperationResult<List<TransactionResultItem>>.Valid because the lookup operation was successful, even if no match was found.
                    // The status "Transaction not found" is conveyed within the list item.
                    return OperationResult<TransactionResultItem>.Valid(resultList);

                    /*var messages = new List<string>();
                    var res = new InquiryResultResponceDto
                    {
                        Code = "E2",
                        Message = "Transaction not found"
                    };
                    return OperationResult<InquiryResultResponceDto>.UnValid(new List<string> { res.Message });*/

                }

                // Find the original transaction (assuming MSG_TYPE maps to a property like MessageType)
                var originalTransaction = transactions.FirstOrDefault(t => t.MsgType == "1200"); // Adjust property name 'MessageType' if needed

                if (originalTransaction == null)
                {
                    // If matching transactions were found but none with MTI "1200" (the original purchase/financial request)
                    var originalNotFoundItem = new TransactionResultItem
                    {
                        Code = "R4",
                        Message = "Original Transaction (1200) not Found",
                        TransactionType = null
                    };
                    var resultList = new List<TransactionResultItem> { originalNotFoundItem };
                    // CHANGE: Returning OperationResult<List<TransactionResultItem>>.Valid
                    return OperationResult<TransactionResultItem>.Valid(resultList);

                    // Data inconsistency? Only reversal found?
                    /* return OperationResult<InquiryResultResponceDto>.Valid(
                          new InquiryResultResponceDto { Code = "R4", Message = "Original Transaction (1200) not Found" }
                     );*/
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
                    var failedItem = new TransactionResultItem
                    {
                        Code = "R1",
                        Message = "Transaction is Failed",
                        TransactionType = transactionType // Include determined type
                    };
                    var resultList = new List<TransactionResultItem> { failedItem };
                    // CHANGE: Returning OperationResult<List<TransactionResultItem>>.Valid
                    return OperationResult<TransactionResultItem>.Valid(resultList);

                    // Not a type we handle for status check (e.g., inquiry)
                    /*return OperationResult<InquiryResultResponceDto>.Valid(
                        new InquiryResultResponceDto { Code = "R4", Message = "Transaction Found (Type Not DEBIT/CREDIT)" }
                   );*/
                }

                // --- 5. Determine Status ---
                bool isFailed = originalTransaction.RespCode != "000";

                if (isFailed)
                {
                    var failedItem = new TransactionResultItem
                    {
                        Code = "R1",
                        Message = "Transaction is Failed",
                        TransactionType = transactionType // Include determined type
                    };
                    var resultList = new List<TransactionResultItem> { failedItem };
                    // CHANGE: Returning OperationResult<List<TransactionResultItem>>.Valid
                    return OperationResult<TransactionResultItem>.Valid(resultList);

                    /*return OperationResult<InquiryResultResponceDto>.Valid(
                        new InquiryResultResponceDto { Code = "R1", Message = "Transaction is Failed", TransactionType = transactionType } // Assuming DTO has TransactionType
                   );*/
                }

                bool isReversed = transactions.Any(t => t.MsgType == "1400" ||  t.MsgType == "1420");

                if (isReversed)
                {
                    var reversedItem = new TransactionResultItem
                    {
                        Code = "R2",
                        Message = "Transaction is already Reversed",
                        TransactionType = transactionType // Include determined type
                    };
                    var resultList = new List<TransactionResultItem> { reversedItem };
                    // CHANGE: Returning OperationResult<List<TransactionResultItem>>.Valid
                    return OperationResult<TransactionResultItem>.Valid(resultList);

                    /*return OperationResult<InquiryResultResponceDto>.Valid(
                        new InquiryResultResponceDto { Code = "R2", Message = "Transaction is already Reversed", TransactionType = transactionType }
                   );*/
                }

                // If the original transaction was found, not failed, and not reversed, it's considered Processed.
                var processedItem = new TransactionResultItem
                {
                    Code = "R3",
                    Message = "Transaction is already Processed",
                    TransactionType = transactionType // Include determined type
                };
                var processedResultList = new List<TransactionResultItem> { processedItem };
                // CHANGE: Returning OperationResult<List<TransactionResultItem>>.Valid
                return OperationResult<TransactionResultItem>.Valid(processedResultList);

                // If not Failed and not Reversed, it's Processed
                /*return OperationResult<InquiryResultResponceDto>.Valid(
                     new InquiryResultResponceDto { Code = "R3", Message = "Transaction is already Processed", TransactionType = transactionType }
                );*/



            }
            catch (OracleException ex) when (ex.Message.Contains("ORA-12545"))
            {
                var processedItem = new TransactionResultItem
                {
                    Code = "E8",
                    Message = "destination host unreachable - ORA-12545",
                    TransactionType = null 
                };
                var processedResultList = new List<TransactionResultItem> { processedItem };
                
                return OperationResult<TransactionResultItem>.Valid(processedResultList);

               /* return OperationResult<List<TransactionResultItem>>.TechError(messages: new[] { "E8", "ORA-12545: TNS:destination host unreachable" });
                return OperationResult<InquiryResultResponceDto>.UnValid("E8", "ORA-12545: TNS:destination host unreachable");*/
            }
            catch (Exception)
            {
                var processedItem = new TransactionResultItem
                {
                    Code = "E9",
                    Message = "Core bank system error",
                    TransactionType = null 
                };
                var processedResultList = new List<TransactionResultItem> { processedItem };
                
                return OperationResult<TransactionResultItem>.Valid(processedResultList);

                /*return OperationResult<InquiryResultResponceDto>.UnValid("E9", "Core bank system error");*/
            }
        }
    }
}
