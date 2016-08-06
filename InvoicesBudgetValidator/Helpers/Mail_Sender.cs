using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicesBudgetValidator.Model;
using NHibernate;
using System.Net.Mail;

namespace InvoicesBudgetValidator.Helpers
{
    class Mail_Sender
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        ("Program");

        public void Send_Mail(ReceivedInvoices invoice,int type, string mail_to,string subject)
        {

            

            try
            {
                string mailinfo = ConfigurationManager.AppSettings["mailconnection"].ToString();
                string savedir = ConfigurationManager.AppSettings["mailsavedir"].ToString();
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
                message.Body = messageBuilder(invoice,type);
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(host);
                smtp.Port = port;
                smtp.Credentials = new System.Net.NetworkCredential(user, password);
                smtp.EnableSsl = true;
                smtp.Send(message);

                SmtpClient client = new SmtpClient("mysmtphost");
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                client.PickupDirectoryLocation =savedir;
                client.Send(message);




            }
            catch (Exception e)
            {

                log.Error(e);
            }

        }


        private string messageBuilder(ReceivedInvoices invoice,int type)
        {
            string message = "";

            string template="";

            switch (type)
            {
                case (int)Mail_Type.BUDGETMAIL:
                    template = File.ReadAllText("Mail_Templates/Budget_template.txt");
                    break;
                case (int)Mail_Type.NOBUDGETMAIL:
                    template = File.ReadAllText("Mail_Templates/NoBudget_template.txt");
                    break;
                default:
                    break;
            }

            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {

                    /*var source2 = session.QueryOver<ReceivedBudgetTimeline>()
                            .Select(x => x.Identifier)
                            .List<string>();*/

                
                using (ITransaction transaction = session.BeginTransaction())
                {
                    /*var source = session.QueryOver<ReceivedInvoices>()
                            .Where(c => c.Company_Id == company).And(c => c.Status_Id == 6)
                            .List<ReceivedInvoices>()
                            .ToList<ReceivedInvoices>();*/

                        var source3 = session.QueryOver<Party>()
                            .Where(c => c.Party_Id==invoice.Company_Id)
                            .List()
                            .FirstOrDefault();
                        if (source3 == null)
                        {
                            return null;
                        }
                        else
                            invoice.Company=source3.Name;

                   // receivedinvoices = source;
                    //transaction.Commit();
                }
                session.Close();
                }
                catch (Exception e)
                {
                    session.Close();
                    log.Error(e);
                    return null;
                }
            }



            if (invoice.Invoice!="")
            {
                message = template.Replace("@INVOICENO", invoice.Invoice);
            }else
            {
                message = template.Replace("@INVOICENO", invoice.Identifier);
            }

            message = message.Replace("@EMISOR", invoice.Party);
            message = message.Replace("@RECEPTOR", invoice.Company);

            

            

            return message;

            
        }

    }
}
