using Microsoft.AspNetCore.Mvc;
using Trisatech.KampDigi.Application.Models.Resident;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trisatech.KampDigi.WebApp.Helpers;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Domain;

namespace Trisatech.KampDigi.WebApp.Controllers
{
    public class ResidentController : BaseController
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IResidentService _residentService;
        private readonly KampDigiContext _digiContext;
        public ResidentController(IWebHostEnvironment webHost, 
            IResidentService residentService,
            KampDigiContext digiContext)
        {
            _webHost = webHost;
            _residentService = residentService;
            _digiContext = digiContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ResidentAdd()
        {
            ViewBag.EmergencyRelation = new SelectList(_digiContext.Residents, "Id", "Nama");
            ViewBag.House = new SelectList(_digiContext.Houses, "Id", "Number");
            return View(new ResidentAddModel());
        }

        [HttpPost]
        public async Task<IActionResult> ResidentAdd(ResidentAddModel dataResident)
        {
            if (!ModelState.IsValid)
            {
                return View(dataResident);
            }
            try
            {
                string fileName = string.Empty;

                //insert identityphoto/KTP
                if (dataResident.KTP != null)
                {
                    fileName = $"{Guid.NewGuid()}-{dataResident.KTP?.FileName}";
                    string filePathName = _webHost.ContentRootPath + $"/images/{fileName}";

                    using (var StreamWriter = System.IO.File.Create(filePathName))
                    {
                        //await StreamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
                        await StreamWriter.WriteAsync(dataResident.KTP.OpenReadStream().ToBytes());
                    }

                    dataResident.IdentityPhoto = $"images/{fileName}";
                }
                else
                {
                    dataResident.IdentityPhoto = String.Empty;
                }

                //insert identityfamilyphoto/KK
                fileName = String.Empty;
                if (dataResident.KK != null)
                {
                    fileName = $"{Guid.NewGuid()}-{dataResident.KK?.FileName}";
                    string filePathName = _webHost.ContentRootPath + $"/images/{fileName}";

                    using (var StreamWriter = System.IO.File.Create(filePathName))
                    {
                        //await StreamWriter.WriteAsync(Common.StreamToBytes(request.GambarFile.OpenReadStream()));
                        await StreamWriter.WriteAsync(dataResident.KK.OpenReadStream().ToBytes());
                    }

                    dataResident.IdentityFamilyPhoto = $"images/{fileName}";
                }
                else
                {
                    dataResident.IdentityFamilyPhoto = String.Empty;
                }



                await _residentService.ResidentAdd(dataResident);

                return Redirect(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            catch (Exception)
            {
                throw;
            }

            return View(dataResident);
        }
    }
}
