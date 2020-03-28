using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFiles.Models
{
    public class Jobs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("IdCustomer")]
        public Customers Customers { get; set; }
        public int IdCustomer { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("COD")]
        public string COD { get; set; }

        [Column(TypeName = "varchar(150)")]
        [DisplayName("Job Name")]
        public string JobName { get; set; }


        [Column(TypeName = "Date")]
        [DisplayName("Date Expiration")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateExpiration { get; set; }
        
        [Column(TypeName = "text")]
        public string Notes { get; set; }

        public bool IsUnique { get; set; }

        [Column(TypeName = "varchar(40)")]
        [DisplayName("GUID")]
        public string GUID { get; set; }

        public List<JobsFileUpload> jobsFileUploads { get; set; }
    }
}
