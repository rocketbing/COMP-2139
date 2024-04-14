using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.Encodings.Web;
namespace Booking.Helper;

public class EmailSender
{
    public async Task<Response> SendMailBySendGrid(string email, string subject, string callbackUrl)
    {
        var client = new SendGridClient("SG.19wJzTifSGGRdfWe8erXkQ.P8MXADL0O8MQkUzvIt1cklE90IiEStuQGVvQtiVAzzM");
        var from = new EmailAddress("adeelfaryad@xodeactech.club", "GBC Travel");
        var to = new EmailAddress(email);
        var htmlContent = callbackUrl;
        var plainTextContent = "";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        return await client.SendEmailAsync(msg);
    }
}
