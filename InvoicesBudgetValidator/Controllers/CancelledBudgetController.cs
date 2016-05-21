using InvoicesBudgetValidator.Helpers;
using InvoicesBudgetValidator.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Controllers
{
    class CancelledBudgetController
    {

        public List<ReceivedBudgetTimeline> getCancelledInvoices()
        {
            
            
            using (ISession session = NHibernateHelperBudget.OpenSession())
            {

                List<ReceivedBudgetTimeline> invoices = new List<ReceivedBudgetTimeline>();
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                       var source = session.QueryOver<ReceivedBudgetTimeline>()
                            .Select(x => x.Identifier)
                            .List();
                        //transaction.Commit();

                       var liststatus2 = session.QueryOver<ReceivedBudgetTimeline>()
                           .Where(z => z.Received_Status_Id == 2)
                           .OrderBy(z => z.Time).Desc
                           .List()
                           .ToList<ReceivedBudgetTimeline>();

                       var liststatus6 = session.QueryOver<ReceivedBudgetTimeline>()
                        .Where(z => z.Received_Status_Id == 6)
                        .OrderBy(z => z.Time).Desc
                        .List()
                        .ToList<ReceivedBudgetTimeline>();


                       foreach (var item in source)
                       {
                           var invoicecancelled = liststatus2.FirstOrDefault(x => x.Identifier == item.ToString());
                           var invoicereceived = liststatus6.FirstOrDefault(x => x.Identifier == item.ToString());

                           var invoicebudgetdetail = session.QueryOver<Budget>()
                           .Where(x => x.Folio_Fiscal == item.ToString())
                           .OrderBy(x => x.Fecha).Desc
                           .SingleOrDefault();

                           if (invoicebudgetdetail != null)
                           {
                               if (invoicecancelled.Time > invoicereceived.Time && invoicecancelled.Time > invoicebudgetdetail.Fecha)
                               {
                                   invoices.Add(invoicecancelled);
                               }
                           }
                           
                       }
                    }
                    session.Close();
                    return invoices;
                }
                catch (Exception)
                {
                    session.Close();
                    return null;
                }
            }


        }





    }
}
