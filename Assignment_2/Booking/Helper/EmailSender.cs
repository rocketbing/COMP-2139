using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.Encodings.Web;
namespace Booking.Helper;

public class EmailSender
{
    public async Task<Response> SendMailBySendGrid(string email, string subject, string callbackUrl)
    {
        var client = new SendGridClient("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");
        var from = new EmailAddress("adeelfaryad@xodeactech.club", "GBC Travel");
        var to = new EmailAddress(email);
        var htmlContent = callbackUrl;
        var plainTextContent = "";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        return await client.SendEmailAsync(msg);
    }
}
