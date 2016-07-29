using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Helpers
{
    class RequestCreator
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program");

        public void createRequestUpdateStatus(int status, string invoice_id)
        {


            try
            {

                string auth = Base64Encode(ConfigurationManager.AppSettings["presupuesto_user"].ToString());

                string services_host = ConfigurationManager.AppSettings["menfisservices"].ToString();


                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();

                    client.Headers.Add("Authorization: Basic " + auth);

                    var response = client.UploadValues(services_host + invoice_id + "/status/" + status, values);

                    var responseString = Encoding.Default.GetString(response);


                    var responsecode = client.ResponseHeaders;

                }


            }
            catch (Exception e)
            {
                log.Error(e);
            }



        }




        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }




    }
}
