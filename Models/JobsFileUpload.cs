using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFiles.Models
{
    public class JobsFileUpload
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("IdJobs")]
        public Jobs Jobs { get; set; }
        public int IdJobs { get; set; }

        [Column(TypeName = "varchar(150)")]
        [DisplayName("Name")]
        public string Name { get; set; }


        [Column(TypeName = "DateTime")]
        [DisplayName("Creation Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? CreationDate { get; set; }

        public int Quantity { get; set; }

     }
}
