using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBSM.Core.Domain.ModelDTO.RequestDTO
{
    public class LypayTransRequestDto
    {
        public HeaderSwitchModel? HeaderSwitchModel { get; set; }
        public LookUpData LookUpData { get; set; }
    }

    public class HeaderSwitchModel
    {
        public string? TargetSystemUserID { get; set; }
    }

    public class LookUpData
    {
        public Details Details { get; set; }
    }

    public class Details
    {
        public string RRN { get; set; }
        public string STAN { get; set; }
        public string TXNAMT { get; set; } // You can make it decimal if needed
        public string TERMID { get; set; }
        public string SETLDATE { get; set; } // Format: "DD-MM-YYYY"
    }
}
