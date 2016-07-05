using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Model
{
    class Consolidado
    {
        public virtual int Id { get; set; }
        public virtual int Company_Id { get; set; }
        public virtual int Vendor_Id { get; set; }
        public virtual string RFC { get; set; }
        public virtual string Razon_Social { get; set; }
        public virtual decimal Presupuesto { get; set; }
    }
}
