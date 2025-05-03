using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBSM.Core.Domain.ModelDTO.ResponceDTO
{
    public class LypayTransactionResponseDto
    {
        public string Code { get; set; }           // E.g. "R1", "E2", etc.
        public string Message { get; set; }        // Description of the result
        public string? TransactionType { get; set; } // e.g., "DEBIT" or "CREDIT"
        public string? RRN { get; set; }
        public string? STAN { get; set; }
        public string? TXNAMT { get; set; }
        public string? TERMID { get; set; }
        public string? SETLDATE { get; set; }
    }
}
