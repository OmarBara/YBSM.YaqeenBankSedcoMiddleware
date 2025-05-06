using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBSM.Core.Domain.ModelDTO.ResponceDTO
{
    public class LypayTransactionDBResponceDto
    {

        [Column("MSG_TYPE")]
        [MaxLength(4)]
        public string? MsgType { get; set; }

        [Column("PROC_CODE")]
        [MaxLength(6)]
        public string? ProcCode { get; set; }


        [Column("RESP_CODE")]
        [MaxLength(4)]
        public string? RespCode { get; set; }

        [Column("WORK_PROGRESS")]
        [MaxLength(1)]
        public string? WorkProgress { get; set; }


        [Column("PAN")]
        [MaxLength(19)]
        public string? Pan { get; set; }

    }
}