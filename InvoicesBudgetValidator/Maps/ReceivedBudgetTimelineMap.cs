using FluentNHibernate.Mapping;
using InvoicesBudgetValidator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Maps
{
    class ReceivedBudgetTimelineMap : ClassMap<ReceivedBudgetTimeline>
    {
        //Constructor
        public ReceivedBudgetTimelineMap()
        {
            Table("Invoices.vw_Received_Cancelled_Budget");

            Id(x => x.Identifier);

            Map(x => x.Company_Id);

            Map(x => x.Time);

            Map(x => x.Time_Tx);

            Map(x => x.Invoice);

            Map(x => x.Received_Status_Id);

            Map(x => x.User_Id);

            Map(x => x.Total);
        }
    
    }
}
