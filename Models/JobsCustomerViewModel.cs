using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFiles.Models
{
    public class CustomerJobsViewModel
    {

        public Customers Customers { get; set; }
        public Jobs Jobs { get; set; }
        public IEnumerable<JobsFileUpload> JobsFileUpload { get; set; }
        public IEnumerable<Jobs> JobsOthers { get; set; }
        public bool Invited { get; set; }
    }
}
