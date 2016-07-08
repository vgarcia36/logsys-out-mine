using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using InvoicesBudgetValidator.Model;

namespace InvoicesBudgetValidator.Maps
{
    class ReceivedInvoicesMap : ClassMap<ReceivedInvoices>
    {
        //Constructor
        public ReceivedInvoicesMap()
        {
            Table("Invoices.vw_Received_Invisibles");

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

            Map(x => x.Exchange_Rate);

        }
    }
}
