using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels.ChargeBack
{//Author: Elaine Chan
    public class DisbursementDetailsViewModel
    {
        #region From StationeryRequest
        [Key]
        [StringLength(10)]
        public string RequestId { get; set; }

        [Required]
        [StringLength(128)]
        public string RequestedBy { get; set; }

        [Required]
        [StringLength(4)]
        public string RequestDepartmentId { get; set; }

        [Required]
        [StringLength(25)]
        public string ReqStatus { get; set; }

        public DateTime RequestDate { get; set; }

        public virtual Department Department { get; set; }

        #endregion

        #region From Disbursement
        [StringLength(10)]
        public string DisbursementId { get; set; }

        [Required]
        [StringLength(10)]
        public string DisbursementNo { get; set; }

        [Required]
        [StringLength(128)]
        public string DisbAcknowledgedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime DisbDate { get; set; }

        [Required]
        [StringLength(25)]
        public string DisbStatus { get; set; }
        #endregion

        #region From TransactionDetails

        [Required]
        [StringLength(4)]
        public string ItemDesc { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Amount { get; set; }
        #endregion
    }
}