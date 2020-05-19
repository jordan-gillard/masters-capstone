using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRCoder;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Capstone.Models;

namespace Capstone.Controllers
{
    public class QRController : Controller
    {
        public ActionResult GenerateRandomQRCode()
        {
            return View();
        }
        public ActionResult GenerateRandomQRCodeImage()
        {
            using (MemoryStream ms = new MemoryStream())
            {

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(GetRandomHospitalCode(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    return File(QRExtension.ConvertToByteArray(bitMap), "image/jpeg");
                }
            }
        }
        private string GetRandomHospitalCode()
        {
            using (MedicalEntities db = new MedicalEntities())
            {
                List<int> ListOfHospitalCodes = db.hospitals.Select(x => x.id).ToList();
                var random = new Random();
                int index = random.Next(ListOfHospitalCodes.Count);
                return ListOfHospitalCodes[index].ToString();
            }

        }

    }
    public static class QRExtension
    {
        public static byte[] ConvertToByteArray(this Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }

}