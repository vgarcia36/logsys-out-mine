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
            Table("Presupuesto.Detalle");

            Id(x => x.Id_presupuesto);

            Map(x => x.Company_Id);

            Map(x => x.Vendor_Id);

            Map(x => x.RFC);

            Map(x => x.Abono);

            Map(x => x.Cargo);

            Map(x => x.Acumulado);

            Map(x => x.id_evento);

            Map(x => x.id_usuario);

            Map(x => x.Usuario);

            Map(x => x.Fecha);

            Map(x => x.Referencia);

            Map(x => x.Id_Archivo);

            Map(x => x.Folio_Fiscal);

        }
    }
}
