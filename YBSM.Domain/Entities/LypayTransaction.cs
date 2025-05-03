using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBSM.Core.Domain.Entities
{
    //[Table("V_LYPAY", Schema = "V_LYPAY")]
    [Table("V_LYPAY2")]
    public class LypayTransaction
    {
        
        
        [Column("XREF")]
        [Required]
        [MaxLength(16)]
        public string? Xref { get; set; }

        [Column("P_KEY")]
        [MaxLength(255)]
        public string PKey { get; set; }

        [Column("MSG_TYPE")]
        [MaxLength(4)]
        public string? MsgType { get; set; }

        [Column("PAN")]
        [MaxLength(19)]
        public string? Pan { get; set; }

        [Column("PROC_CODE")]
        [MaxLength(6)]
        public string? ProcCode { get; set; }

        [Column("TXN_AMT")]
        [MaxLength(16)]
        public string? TxnAmt { get; set; }

        [Column("SETL_AMT")]
        [MaxLength(16)]
        public string? SetlAmt { get; set; }

        [Column("BILL_AMT")]
        [MaxLength(16)]
        public string? BillAmt { get; set; }

        [Column("TRANS_DT_TIME")]
        [MaxLength(10)]
        public string? TransDtTime { get; set; }

        [Column("STAN")]
        [MaxLength(12)]
        public string? Stan { get; set; }

        [Column("ACQ_INS_ID")]
        [MaxLength(11)]
        public string? AcqInsId { get; set; }

        [Column("FWD_INS_ID")]
        [MaxLength(11)]
        public string? FwdInsId { get; set; }

        [Column("RRN")]
        [MaxLength(12)]
        public string? Rrn { get; set; }

        [Column("RESP_CODE")]
        [MaxLength(4)]
        public string? RespCode { get; set; }

        [Column("TERM_ID")]
        [MaxLength(16)]
        public string? TermId { get; set; }

        [Column("TXN_CCY_CODE")]
        [MaxLength(3)]
        public string? TxnCcyCode { get; set; }

        [Column("SETL_CCY_CODE")]
        [MaxLength(3)]
        public string? SetlCcyCode { get; set; }

        [Column("BILL_CCY_CODE")]
        [MaxLength(3)]
        public string? BillCcyCode { get; set; }

        [Column("FROM_ACC")]
        [MaxLength(28)]
        public string? FromAcc { get; set; }

        [Column("TO_ACC")]
        [MaxLength(28)]
        public string? ToAcc { get; set; }

        [Column("TXN_DESC")]
        [MaxLength(255)]
        public string? TxnDesc { get; set; }

        [Column("EXP_DATE")]
        [MaxLength(4)]
        public string? ExpDate { get; set; }

        [Column("SETL_DATE")]
        [MaxLength(8)]
        public string? SetlDate { get; set; }

        [Column("CONV_DATE")]
        [MaxLength(4)]
        public string? ConvDate { get; set; }

        [Column("CAPT_DATE")]
        [MaxLength(4)]
        public string? CaptDate { get; set; }

        [Column("MSG_FLOW")]
        [MaxLength(2000)]
        public string? MsgFlow { get; set; }

        [Column("WORK_PROGRESS")]
        [MaxLength(1)]
        public string? WorkProgress { get; set; }

        [Column("ERR_PARAM")]
        [MaxLength(255)]
        public string? ErrParam { get; set; }

        [Column("TRN_REF_NO")]
        [MaxLength(16)]
        public string? TrnRefNo { get; set; }

        [Column("AMOUNT_BLOCK_NO")]
        [MaxLength(35)]
        public string? AmountBlockNo { get; set; }

        [Column("PURGE_DATE")]
        public DateTime? PurgeDate { get; set; }

        [Column("ERROR_CODE")]
        [MaxLength(11)]
        public string? ErrorCode { get; set; }

        [Column("PRE_AUTH_SEQ_NO")]
        [MaxLength(16)]
        public string? PreAuthSeqNo { get; set; }

        [Column("RECONSILED")]
        [MaxLength(1)]
        public string? Reconsiled { get; set; }

        [Column("DCN")]
        [MaxLength(16)]
        public string? Dcn { get; set; }

        [Column("ADD_AMT")]
        [MaxLength(4000)]
        public string? AddAmt { get; set; }

        [Column("MINI_STMT_DATA")]
        [MaxLength(999)]
        public string? MiniStmtData { get; set; }

        [Column("OFFUS_ONUS")]
        [MaxLength(1)]
        public string? OffusOnus { get; set; }

        [Column("FILE_PROCESS")]
        [MaxLength(1)]
        public string? FileProcess { get; set; }

        [Column("FIN_IMP")]
        [MaxLength(1)]
        public string? FinImp { get; set; }

        [Column("DB_IN_TIME")]
        [MaxLength(20)]
        public string? DbInTime { get; set; }

        [Column("DB_OUT_TIME")]
        [MaxLength(20)]
        public string? DbOutTime { get; set; }

        [Column("TERM_ADDR")]
        [MaxLength(999)]
        public string? TermAddr { get; set; }
        
        // public int id { get; set; }
        /* public string Termid { get; set; }
         public string Status { get; set; }
         public string F1 { get; set; }        
         public decimal Lcy_amount { get; set; }
         public string Ref_num_debit { get; set; }
         public string Addl_text { get; set; }
         public string Value_dt { get; set; }*/
    }
}
