//using SendEmail.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Web;
//using System.Web.Mvc;

//namespace SendEmail.Controllers
//{
//    public class HomeController : Controller
//    {
//        // GET: Home
//        public ActionResult Index()
//        {
//            return View();
//        }
//        public ActionResult SendMail()
//        {
//            return View();
//        }
//        [HttpPost]
//        public ActionResult SendMail(EmailModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.Message = "Please fill in all required fields.";
//                return View();
//            }

//            try
//            {
//                using (MailMessage mm = new MailMessage(model.Email, model.To))
//                {
//                    mm.Subject = model.Subject;
//                    mm.Body = model.Body;
//                    mm.IsBodyHtml = false;

//                    // Attach file if uploaded
//                    if (model.Attachment != null && model.Attachment.ContentLength > 0)
//                    {
//                        string fileName = Path.GetFileName(model.Attachment.FileName);
//                        mm.Attachments.Add(new Attachment(model.Attachment.InputStream, fileName));
//                    }

//                    // SMTP Configuration
//                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
//                    {
//                        smtp.EnableSsl = true;
//                        smtp.UseDefaultCredentials = false; // Must be false
//                        smtp.Credentials = new NetworkCredential(model.Email, model.Password); // Use App Password
//                        smtp.Send(mm);
//                        ViewBag.Message = "Email Sent Successfully!";
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Message = "Error: " + ex.Message;
//            }

//            return View();
//        }
//    }
//}


using SendEmail.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace SendEmail.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMail(EmailModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please fill in all required fields.";
                return View();
            }

            try
            {
                using (MailMessage mm = new MailMessage(model.Email, model.To))
                {
                    mm.Subject = model.Subject;

                    // Generate a dynamic link (Example: Redirect to a confirmation page)
                    string confirmationLink = Url.Action("Confirm", "Home", new { email = model.To }, Request.Url.Scheme);

                    // HTML email body with a clickable link
                    mm.Body = $"<p>Dear Customer,</p>" +
                              $"<p>Please click the link below to confirm:</p>" +
                              $"<p><a href='{confirmationLink}' target='_blank'>Click Here to Confirm</a></p>" +
                              $"<p>Thank you!</p>";

                    mm.IsBodyHtml = true; // Enable HTML format

                    // Attach file if uploaded
                    if (model.Attachment != null && model.Attachment.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(model.Attachment.FileName);
                        mm.Attachments.Add(new Attachment(model.Attachment.InputStream, fileName));
                    }

                    // SMTP Configuration
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(model.Email, model.Password); // Use App Password
                        smtp.Send(mm);
                        ViewBag.Message = "Email Sent Successfully!";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }

            return View();
        }

        // New Action for Confirmation Page
        public ActionResult Confirm(string email)
        {
            ViewBag.Email = email;
            return View();
        }
    }
}
