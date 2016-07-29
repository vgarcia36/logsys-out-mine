using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InvoicesBudgetValidator.Model
{
    class Budget
    {
        public virtual long Id_presupuesto { get; set; }
        public virtual long Company_Id { get; set; }
        public virtual long Vendor_Id { get; set; }
        public virtual string RFC { get; set; }
        public virtual decimal Abono { get; set; }
        public virtual decimal Cargo { get; set; }
        public virtual decimal Acumulado { get; set; }
        public virtual int id_evento { get; set; }
        public virtual long id_usuario { get; set; }
        public virtual string Usuario { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual string Referencia { get; set; }
        public virtual long? Id_Archivo { get; set; }
        public virtual string Folio_Fiscal { get; set; }
    }
}
