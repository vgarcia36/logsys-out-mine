using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Model
{
    class Party
    {
        public virtual long Party_Id { get; set; }
        public virtual string RFC { get; set; }
        public virtual string Name_Short { get; set; }
        public virtual string Name { get; set; }
    }
}
