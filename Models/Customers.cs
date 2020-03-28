using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFiles.Models
{
    public class Customers
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]        
        [DisplayName("Name")]
        public string BusinessName { get; set; }

        [Column(TypeName = "varchar(150)")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(40)")]
        [DisplayName("Guid")]
        public string Guid { get; set; }

        public List<Jobs> Jobs { get; set; }
    }
}
