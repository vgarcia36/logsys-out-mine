using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using InvoicesBudgetValidator.Model;

namespace InvoicesBudgetValidator.Maps
{
    class BudgetMap : ClassMap<Budget>
    {
        //Constructor
        public BudgetMap()
        {
            Table("presupuesto.Detalle");

            Id(x => x.Id);

            Map(x => x.Vendor_Id);

            Map(x => x.RFC);

            Map(x => x.Abono);

            Map(x => x.Cargo);

            Map(x => x.Acumulado);

            Map(x => x.Evento_Tipo);

            Map(x => x.Usuario);

            Map(x => x.Fecha);

            Map(x => x.Referencia);

            Map(x => x.Referencia);

            Map(x => x.Folio_Fiscal);

        }
    }
}
