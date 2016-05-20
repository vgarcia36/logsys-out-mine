using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using InvoicesBudgetValidator.Model;

namespace InvoicesBudgetValidator.Maps
{
    class ConsolidadoMap : ClassMap<Consolidado>
    {
        //Constructor
        public ConsolidadoMap()
        {
            Table("presupuesto.Detalle");

            Id(x => x.Id);

            Map(x => x.Vendor_Id);

            Map(x => x.RFC);

            Map(x => x.Razon_Social);

            Map(x => x.Presupueso);
        }
    }
}
