cusing System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Model
{
    class Budget_Party
    {
        public virtual int Company_Id { get; set; }
        public virtual bool Budget_Is_Required { get; set; }
    }
}
