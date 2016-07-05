using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Helpers
{
    class RequestCreator
    {
        
        public string createRequestUpdateStatus(int status,string invoice_id)
        {
            
               
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();

                    client.Headers.Add("Authorization: Basic YQBkAG0AaQBuADoAbAAwAGcANQB5ADUA");

                    var response = client.UploadValues("http://bi.mexamerik.com:8089/services/received/invoice/"+invoice_id+"/status/"+status, values);

                    var responseString = Encoding.Default.GetString(response);

                    return responseString;
                }
        }

    }
}
