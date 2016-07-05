using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using InvoicesBudgetValidator.Model;

namespace InvoicesBudgetValidator.Maps
{
    class Received_Archived_Map : ClassMap<Received_Archived>
    {
        //Constructor
        public Received_Archived_Map()
        {
            Table("Invoices.vw_Received_Archived");

            Id(x => x.Identifier);

            Map(x => x.Party_rfc);

            Map(x => x.Party);

            Map(x => x.Company_Id);

            Map(x => x.Party_Id);

            Map(x => x.Invoice);

            Map(x => x.Invoice_Id);

            Map(x => x.Type_Id);

            Map(x => x.TIme);

            Map(x => x.Status_Id);

            Map(x => x.Total);


        }
    }
}
