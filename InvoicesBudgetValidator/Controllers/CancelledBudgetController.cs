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

        private List<ReceivedBudgetTimeline> getTimelineCancelledInvoices()
        {
            
            
            using (ISession session = NHibernateHelper.OpenSession())
            {

                List<ReceivedBudgetTimeline> invoices = new List<ReceivedBudgetTimeline>();
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                       
                       var liststatus2 = session.QueryOver<ReceivedBudgetTimeline>()
                           .OrderBy(z => z.Time_Tx).Asc
                           .List()
                           .ToList<ReceivedBudgetTimeline>();
                       invoices = liststatus2;   
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


        public List<Received_Archived> getCancelledInvoices()
        {

            var cancelled_timeline = getTimelineCancelledInvoices();

            var identifiers = cancelled_timeline.Select(x => x.Identifier).ToList<string>();

            List<Received_Archived> invoices = new List<Received_Archived>();

            foreach (var item in identifiers)
            {

                using (ISession session = NHibernateHelper.OpenSession())
                {             
                    try
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {

                            var invoice = session.QueryOver<Received_Archived>()
                                .Where(z => z.Identifier == item)
                                .List()
                                .ToList<Received_Archived>()
                                .FirstOrDefault();
                            if (invoice!=null)
                            {
                                invoices.Add(invoice);
                            }
                            
                        }
                        session.Close();
                        
                    }
                    catch (Exception)
                    {
                        session.Close();
                        return null;
                    }
                }


            }
            if (invoices.Count > 0)
            {
                return invoices;
            }
            else
                return null;
            
        }





    }
}
