using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Model
{
    class Received_Archived
    {
        public virtual int Company_Id { get; set; }
        public virtual int Party_Id { get; set; }
        public virtual string Party { get; set; }
        public virtual string Invoice_Id { get; set; }
        public virtual string Invoice { get; set; }
        public virtual int Type_Id { get; set; }
        public virtual DateTime TIme { get; set; }
        public virtual string Party_rfc { get; set; }
        public virtual string Identifier { get; set; }
        public virtual int Status_Id { get; set; }
        public virtual decimal Total { get; set; }
    }
}
