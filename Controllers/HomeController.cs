using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using VCardQRGenerator.Models;
using QRCoder;
using System.Text;

namespace VCardQRGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IndexViewModel model)
        {
            string vcfContent = "BEGIN:VCARD\r\n" +
                    $"VERSION:3.0\r\n" +
                    $"FN:{model.Details.FirstName} {model.Details.LastName}\r\n" +
                    $"N:{model.Details.LastName};{model.Details.FirstName};;;\r\n" +
                    $"EMAIL;TYPE=INTERNET:{model.Details.Email}\r\n" +
                    $"TEL;TYPE=WORK:{model.Details.PhoneNumber}\r\n" +
                    $"ADR;TYPE=HOME;LABEL=\"{model.Details.City}, {model.Details.Country}\"\r\n" +
                    $"ORG:{model.Details.Company}\r\n" +
                    $"TITLE:{model.Details.Job}\r\n" +  
                    $"ADR;TYPE=WORK;LABEL=\"{model.Details.ZIP}\"\r\n" + 
                    $"TEL;TYPE=FAX:{model.Details.Fax}\r\n" +
                    "END:VCARD\r\n";


            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrgen = new QRCodeGenerator();
                var qrcodedata = qrgen.CreateQrCode(vcfContent, QRCodeGenerator.ECCLevel.H);
                var qrcode = new Base64QRCode(qrcodedata);
                model.QrCode = qrcode.GetGraphic(20);
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
