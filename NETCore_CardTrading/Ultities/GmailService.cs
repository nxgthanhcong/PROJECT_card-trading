using Microsoft.Extensions.Configuration;
using NETCore_CardTrading.Areas.Admin.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NETCore_CardTrading.Ultities
{
    public interface IGmailService
    {
        Task Send(GmailData data);
    }
    public class GmailService : IGmailService
    {
        private readonly IConfiguration _config;
        public GmailService(IConfiguration config)
        {
            _config = config;
        }
        public async Task Send(GmailData gmail)
        {
            try
            {
                string body = string.Empty;
                if(!string.IsNullOrEmpty(gmail.mailTemplate))
                {
                    using (StreamReader reader = new StreamReader($"{gmail.mailTemplate}"))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{Title}", gmail.title);
                    body = body.Replace("{MainContent}", gmail.mainContent);
                }

                body = gmail.mainContent;
                MailMessage mailMessage = new MailMessage(
                    from: gmail.from,
                    to: gmail.to,
                    subject: gmail.subject,
                    body: body
                );

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.ReplyToList.Add(new MailAddress(gmail.from));
                mailMessage.Sender = new MailAddress(gmail.from);

                if (gmail.path != null)
                {
                    mailMessage.Attachments.Add(new Attachment(gmail.path));
                }

                using (SmtpClient client = new SmtpClient(_config["Gmail:smtpServer"]))
                {
                    client.Port = int.Parse(_config["Gmail:port"]);
                    client.UseDefaultCredentials = true;
                    client.Credentials = new NetworkCredential(_config["Gmail:credential:username"], _config["Gmail:credential:password"]);
                    client.EnableSsl = bool.Parse(_config["Gmail:enabledSSL"]);
                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
