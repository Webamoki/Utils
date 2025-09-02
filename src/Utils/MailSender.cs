using System.Net;
using System.Net.Mail;
using System.Text;

namespace Webamoki.Utils;

public class MailSender
{
    private const string PublicAccessKey = "username";
    private const string SecretAccessKey = "password";

    // Properties IsBodyHTML + Body + To + From + To.Add + To.From + To.Clear
    // Set IsBodyHTML true if using MailBody to construct body
    public MailMessage Message { get; }

    private readonly SmtpClient _client;

    // TODO: Inject AWS_SES Options into constructor - DO NOT turn this into a primary constructor
    // ReSharper disable once ConvertToPrimaryConstructor
    public MailSender(
        string subject,
        string fromEmail = "noreply@keycrox.co.uk",
        string fromName = "No Reply"
    )
    {
        Message = new MailMessage
        {
            Subject = subject,
            From = new MailAddress(fromEmail, fromName)
        };

        _client = new SmtpClient("email-smtp.eu-west-2.amazonaws.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(PublicAccessKey, SecretAccessKey),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        Message.BodyEncoding = Encoding.UTF8;
    }

    public void Send()
    {
        try
        {
            _client.Send(Message);
        }
        catch (Exception ex)
        {
            Logging.WriteLog($"Error sending email to {string.Join(", ", Message.To.Select(t => t.Address.ToString()))}\n{ex.Message}");
        }
    }

    public void AddHTMLBody(string content)
    {
        Message.Body += content;
        Message.IsBodyHtml = true;
    }

    public void AttachFile(string filePath, string? name = null) => Message.Attachments.Add(string.IsNullOrEmpty(name) ? new Attachment(filePath) : new Attachment(filePath, name));

    public void AttachImage(string filePath, string contentId)
    {
        var attachment = new Attachment(filePath);
        attachment.ContentDisposition!.Inline = true;
        attachment.ContentId = contentId;
        Message.Attachments.Add(attachment);
    }
}
