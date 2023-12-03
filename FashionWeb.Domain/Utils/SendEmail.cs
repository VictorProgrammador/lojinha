using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Utils
{

    public static class SendEmail
    {
        public static string Send(string htmltemplate, string message, string subject, string recipient)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtpout.secureserver.net", 80);
                client.EnableSsl = false;
                client.Credentials = new NetworkCredential("comercial@oshopdigital.com", "Vitorsal123");

                var html = "";
                html = htmltemplate;
                html = html.Replace("[Title]", subject);
                html = html.Replace("[Conteudo]", message);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("comercial@oshopdigital.com", "Vestir Bem Br"),
                    Subject = subject,
                    Body = html,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipient);

                client.Send(mailMessage);

                return "sucesso enviou email";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            return "Falha ao enviar email";
        }
    }
}
