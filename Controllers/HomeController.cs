using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UploadFiles.Models;
using System.Threading;

namespace UploadFiles.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly string _targetFilePath;
        private readonly string _CustomerInvited;
        private string[] permittedExtensions = { ".zip", ".pdf" };
        public HomeController(ILogger<HomeController> logger,
                                 ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            //_fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            // To save physical files to a path provided by configuration:
            _targetFilePath = config.GetValue<string>("StoredFilesPath");
            _CustomerInvited = config.GetValue<string>("Invited");
        }


        [Route("/Home/Upload/{guid}")]
        public IActionResult Upload(string guid)
        {
            // job guid
            if (!string.IsNullOrEmpty(guid))
            {
                CustomerJobsViewModel viewModel = new CustomerJobsViewModel();
                viewModel.Jobs = _context.Jobs.Where(x => x.GUID == guid).FirstOrDefault();
                if (viewModel.Jobs != null)
                {
                    viewModel.Customers = _context.Customers.Where(c => c.Id == viewModel.Jobs.IdCustomer).FirstOrDefault();
                    viewModel.JobsFileUpload = _context.JobsFileUpload.Where(c => c.IdJobs == viewModel.Jobs.Id).ToList();
                    if (viewModel.Customers.Id != int.Parse(_CustomerInvited))
                    {
                        viewModel.JobsOthers = _context.Jobs.Where(x => x.IdCustomer == viewModel.Customers.Id).ToList();
                    }
                    else
                    {

                        viewModel.Invited = true;
                    }

                    return View(viewModel);
                }
            }
            return RedirectToAction(nameof(Index));
        }



        [Route("/Home/Detailfile/{id}")]
        public FileResult Detailfile(int id)
        {
            JobsFileUpload file = _context.JobsFileUpload.Where(c => c.Id == id).Include(j => j.Jobs).FirstOrDefault();
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (provider.TryGetContentType(file.Name, out contentType))
            {
                string fullPath = Path.GetFullPath(_targetFilePath);
                var filePath = Path.Combine(fullPath, file.Jobs.JobName, file.Name);
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, contentType, file.Name);
            }
            return null;
        }


        public IActionResult Index(string guid, int? id = 0)
        {
            // customers guid  , job id
            Customers customer = new Customers();
            if (!string.IsNullOrEmpty(guid))
            {
                customer = _context.Customers.Where(x => x.Guid == guid).FirstOrDefault();
                if (customer != null && customer.Id != int.Parse(_CustomerInvited))
                {
                    Jobs jobs = new Jobs();
                    if (id > 0)
                    {
                        jobs = _context.Jobs.Where(x => x.Id == id).FirstOrDefault();
                    }
                    else
                    {
                        jobs = new Jobs
                        {
                            IdCustomer = customer.Id,
                            JobName = Helpers.RandomString.GetRandomString(3, false) + DateTime.Now.ToString("yyMMddHHmm", CultureInfo.CreateSpecificCulture("en-US")),
                            COD = DateTime.Now.ToString("yyMMddHHmm", CultureInfo.CreateSpecificCulture("en-US")),
                            Customers = customer
                        };
                    }
                    return View(jobs);
                }

            }

            //caso 1 cdo el guid no es null costomer esta cargado  
           int IdInvited = int.Parse(_CustomerInvited);
            if (IdInvited > 0 ) { 
                        Jobs NewJobs = new Jobs();
                        customer = _context.Customers.Where(x => x.Id == IdInvited).FirstOrDefault();
                        if (customer != null)
                        {
                            NewJobs.IdCustomer = customer.Id;
                            NewJobs.JobName = Helpers.RandomString.GetRandomString(3, false) + DateTime.Now.ToString("yyMMddHHmm", CultureInfo.CreateSpecificCulture("en-US"));
                            NewJobs.COD = DateTime.Now.ToString("yyMMddHHmm", CultureInfo.CreateSpecificCulture("en-US"));
                            NewJobs.Customers = customer;
                        }
        return View(NewJobs);
            }
            return RedirectToAction(nameof(NotAccessible));
   
        }

        public IActionResult NotAccessible()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveJob(Jobs model)
        {
            if (ModelState.IsValid)
            {
                
                string fullPath = Path.GetFullPath(_targetFilePath);
                string filenameSave = "";
                string directorySave = "";

                System.Guid guid = System.Guid.NewGuid();               
                model.GUID= guid.ToString();
                Jobs jobs = _context.Jobs.Find(model.Id);                
                if (jobs == null)
                {
                    jobs = _context.Jobs.Add(model).Entity;
                    _context.SaveChanges();
                }
                else {

                    jobs.Notes = model.Notes;
                    jobs.DateExpiration = model.DateExpiration;
                    jobs.IsUnique = model.IsUnique;
                    jobs = _context.Update(jobs).Entity;
                    _context.SaveChanges();

                }

                if (jobs != null)
                {
                    var oJson = Newtonsoft.Json.JsonConvert.SerializeObject(new { data = jobs.Id, result = 200 });
                    return Json(oJson);                  
                }
                else
                {
                    return Json(new { result = false });
                }
            }
       
            ModelState.Clear();
            return Json(new { result = false });
        }

        [HttpPost]
        [DisableRequestSizeLimit]
      
        public async Task<IActionResult> AjaxFile(  int Id,
                                                      string jobname,
                                                      IFormFile dataFile)
        {


            string fullPath = Path.GetFullPath(_targetFilePath);
            string filenameSave = "";
            string directorySave = "";
            if ((Id == 0) || (dataFile == null)) { 
                return Json("{'result':201}"); 
            }
            if (dataFile.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(dataFile.ContentDisposition).FileName.Trim('"');
                var ext = Path.GetExtension(fileName).ToLowerInvariant();
                if (!string.IsNullOrEmpty(ext) || permittedExtensions.Contains(ext))
                {
                    directorySave = existeDirectory(jobname, fullPath);
                    filenameSave = existeArchivo(fileName, directorySave);
                    string filePath = Path.Combine(directorySave, filenameSave);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await dataFile.CopyToAsync(stream);
                    }
                }
              
                JobsFileUpload fileUpload = new JobsFileUpload
                {
                    IdJobs = Id,
                    Name = filenameSave,
                    CreationDate = DateTime.Now,
                    Quantity = QuantityOfCopias(filenameSave, directorySave)
                };

               
                var file = _context.JobsFileUpload.Add(fileUpload);
                _context.SaveChanges();

                if (file != null)
                {
                    var oJson = Newtonsoft.Json.JsonConvert.SerializeObject(new { data = file.Entity.Id, result = 200 });
                    return Json(oJson);
                }
                else
                {
                    return Json("{'result':403}");
                }
            }
            else
            {              
                return Json("{'result':500}");
            }
        }


        [Route("/Home/Sent/{name}")]
        public IActionResult Sent(string name)
        {
            // 
            Jobs jobs = null;
            if (!string.IsNullOrEmpty(name))
            {
                jobs = _context.Jobs.Where(x => x.JobName == name).Include(c => c.Customers).FirstOrDefault();
                if (jobs != null)
                {
                    if (jobs.Customers.Id == int.Parse(_CustomerInvited))
                    {

                        return View(null);
                    }
                }
            }
            return View(jobs);
        }

        private int QuantityOfCopias(string FileName, string fullPath)
        {
            int quantity = 0;
            FileInfo fInfo = new FileInfo(Path.Combine(fullPath, FileName));
            if (fInfo.Exists)
            {
                string ext = Path.GetExtension(FileName);
                string na = Path.GetFileNameWithoutExtension(FileName);
                string dato = na.Substring((na.Length - 3), 3);
                int found = dato.IndexOf("(");
                if (found == 0)
                {
                    quantity = int.Parse(dato.Substring(found + 1, 1)) + 1;
                }
                else
                {
                    quantity = 1;
                }
            }
            return quantity;
        }

        private string existeArchivo(string FileName, string fullPath)
        {
            FileInfo fInfo = new FileInfo(Path.Combine(fullPath, FileName));
            if (fInfo.Exists)
            {
                string ext = Path.GetExtension(FileName);
                string na = Path.GetFileNameWithoutExtension(FileName);
                string dato = na.Substring((na.Length - 3), 3);
                int found = dato.IndexOf("(");
                if (found == 0)
                {
                    int i = int.Parse(dato.Substring(found + 1, 1)) + 1;
                    FileName = na.Substring(0, na.Length - 3) + "(" + i + ")" + ext;
                }
                else
                {
                    FileName = na + "(" + 1 + ")" + ext;
                }

                return existeArchivo(FileName, fullPath);
            }
            else
            {
                return FileName;
            }
            return FileName;
        }

        private string existeDirectory(string codJobs, string fullPath)
        {
            //verifica la existencia de las carpetas
            string directorySave = Path.Combine(fullPath, codJobs);

            if (!Directory.Exists(directorySave))
            {
                Directory.CreateDirectory(directorySave);
            }
            return directorySave;

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
