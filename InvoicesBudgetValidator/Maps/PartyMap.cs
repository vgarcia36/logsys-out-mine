using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using InvoicesBudgetValidator.Model;

namespace InvoicesBudgetValidator.Maps
{
    class PartyMap: ClassMap<Party>
    {
        //Constructor
        public PartyMap()
        {
            Table("Core.Party");

            Id(x => x.Party_Id);

            Map(x => x.RFC);

            Map(x => x.Name);

            Map(x => x.Name_Short);


        }
    }
}
