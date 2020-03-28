using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadFiles.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

       public DbSet<Customers> Customers { get; set; }

       public DbSet<Jobs> Jobs { get; set; }

       public DbSet<JobsFileUpload> JobsFileUpload { get; set; }
    }
}
