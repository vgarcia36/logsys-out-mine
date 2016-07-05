using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using InvoicesBudgetValidator.Model;

namespace InvoicesBudgetValidator.Maps
{
    class BudgetPartyMap: ClassMap<Budget_Party>
    {
        //Constructor
        public BudgetPartyMap()
        {
            Table("Invoices.Company_Reception");

            Id(x => x.Company_Id);

            Map(x => x.Send_Mail);

            Map(x => x.Initial_status);

        }

    }
}
