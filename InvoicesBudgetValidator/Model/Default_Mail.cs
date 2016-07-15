using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace InvoicesBudgetValidator.Model
{

    public class Default_Mail
    {
        public ObjectId _id { get; set; }
        public long company_id { get; set; }
        public string rfc { get; set; }
        public string alias { get; set; }
        public string default_email { get; set; }
        public Email_List[] email_list { get; set; }
        public string updated_by { get; set; }
        public int updated_by_id { get; set; }
        public DateTime update_date { get; set; }
        public bool isupdatable { get; set; }
    }

    public class Email_List
    {
        public string email { get; set; }
    }

}
