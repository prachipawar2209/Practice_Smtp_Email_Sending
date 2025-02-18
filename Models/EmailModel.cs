using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SendEmail.Models
{
    public class EmailModel
    {
        [Required, EmailAddress]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // This will store the App Password

        public HttpPostedFileBase Attachment { get; set; }
    }
}
