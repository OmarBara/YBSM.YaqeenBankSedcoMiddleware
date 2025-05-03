using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBSM.Core.Domain.ModelDTO.ResponceDTO
{
    public class InquiryResultResponceDto
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string TransactionType { get; set; }
    }

    public class InquiryResponse
    {
        public List<InquiryResultResponceDto> Result { get; set; }
    }
}
