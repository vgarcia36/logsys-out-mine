using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Helpers
{
    class Mail_Sender
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        ("Program");

        public void Send_Mail(string mensaje, string mail_to,string subject)
        {

            

            try
            {
                string mailinfo = ConfigurationManager.AppSettings["mailconnection"].ToString();
                var maildinnfofields = mailinfo.Split(';');
                int count = 0;
                foreach (var item in maildinnfofields)
                {
                    maildinnfofields[count] = item.Remove(0, item.Split('=')[0].Length + 1);
                    count++;
                }
                int port = Convert.ToInt32(maildinnfofields[0]);
                string host = maildinnfofields[1];
                string user = maildinnfofields[2];
                string password = maildinnfofields[3];
                //string mailto = maildinnfofields[4];
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(mail_to);
                message.Subject = subject;
                message.From = new System.Net.Mail.MailAddress(user);
                message.Body = mensaje;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(host);
                smtp.Port = port;
                smtp.Credentials = new System.Net.NetworkCredential(user, password);
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
            catch (Exception e)
            {

                log.Error(e);
            }

        }
    }
}
