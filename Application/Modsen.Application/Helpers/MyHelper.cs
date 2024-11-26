using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Modsen.Infrastructure;

namespace Modsen.Application
{
    public static class MyHelpers
    {
        public static string SaveImage(IFormFile EventImage, IWebHostEnvironment hostEnvironment)
        {
            var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName;
            string filePath;

            do
            {
                int randomNumber = new Random().Next(1, 1_000_000);

                uniqueFileName = $"Imo{randomNumber}.jpg";
                filePath = Path.Combine(uploadsFolder, uniqueFileName);

            } while (File.Exists(filePath));

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                EventImage.CopyTo(fileStream);
            }

            return Path.Combine("Images", uniqueFileName);
        }


        public static bool EventExists(int id, ModsenContext dbContext)
        {
            return dbContext.Events.Any(e => e.Id == id);
        }

        public static void DeleteUnusedImages(IWebHostEnvironment hostEnvironment, ModsenContext dbContext)
        {
            var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Images");

            var allFiles = Directory.GetFiles(uploadsFolder);

            var usedImages = dbContext.EventImages.Select(img => img.ImagePath).ToList();

            foreach (var filePath in allFiles)
            {
                var fileName = Path.GetFileName(filePath);
                if (!usedImages.Contains("Images\\" + fileName))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        public static void SendEmail(string toEmail, string subject, string body)
        {
            const string FromEmail = "newt3rr@gmail.com";      
            const string Password = "yync qxpf yuzl ltrd";   
            const string SmtpServer = "smtp.gmail.com";  
            const int SmtpPort = 587;

            using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(FromEmail);
                        mail.To.Add(toEmail);
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = false;

                        using (SmtpClient smtp = new SmtpClient(SmtpServer, SmtpPort))
                        {
                            smtp.Credentials = new NetworkCredential(FromEmail, Password);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                }
        }

    }
}