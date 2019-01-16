using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.ViewModels
    //Author: Elaine Chan
{
    public class DepartmentChargeBackViewModel
    {

        // Use Case 25: View Department Charge Back

        #region From Disbursement Table

        [StringLength(10)]
        public string DisbursementId { get; set; }

        [Required]
        [StringLength(10)]
        public string DisbursementNo { get; set; }

        [Required]
        [StringLength(4)]
        public string DepartmentId { get; set; }

        [Required]
        [StringLength(128)]
        public string AcknowledgedBy { get; set; }

        [Required]
        [StringLength(128)]
        public string DisbursedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(10)]
        public string RequestId { get; set; }

        [Required]
        [StringLength(25)]
        public string Status { get; set; }

        [StringLength(4)]
        public string OTP { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual AspNetUsers AspNetUsers1 { get; set; }

        public virtual Department Department { get; set; }

        public virtual StationeryRequest StationeryRequest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
        #endregion

        public DateTime toDTP { get; set; }
        public DateTime fromDTP { get; set; }
    }
}