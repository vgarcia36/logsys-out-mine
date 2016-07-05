using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Model
{
    class Budget_Party
    {
        public virtual int Company_Id { get; set; }
        public virtual bool Send_Mail { get; set; }
        public virtual int Initial_status { get; set; }
    }
}
