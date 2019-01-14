namespace Team7ADProject.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CollectionPoint")]
    public partial class CollectionPoint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CollectionPoint()
        {
            Department = new HashSet<Department>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CollectionPointId { get; set; }

        [Required]
        [StringLength(50)]
        public string CollectionDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh:mm tt}")]
        public DateTime Time { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Department { get; set; }
    }
}
