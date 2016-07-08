using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicesBudgetValidator.Helpers;
using InvoicesBudgetValidator.Model;
using NHibernate;
using NHibernate.Linq;

namespace InvoicesBudgetValidator.Controllers
{
    class BudgetInvoices
    {
        List<Budget_Party> budgetparties;
        List<ReceivedInvoices> receivedinvoices = new List<ReceivedInvoices>();

        public List<ReceivedInvoices> getInvoices(int company)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {

                    /*var source2 = session.QueryOver<ReceivedBudgetTimeline>()
                            .Select(x => x.Identifier)
                            .List<string>();*/

                
                using (ITransaction transaction = session.BeginTransaction())
                {
                    /*var source = session.QueryOver<ReceivedInvoices>()
                            .Where(c => c.Company_Id == company).And(c => c.Status_Id == 6)
                            .List<ReceivedInvoices>()
                            .ToList<ReceivedInvoices>();*/

                        var source3 = session.QueryOver<ReceivedInvoices>()
                            .Where(c => c.Company_Id == company).And(c => c.Status_Id == (int)Menfis_Invoices_Status.FACTURA_RECIBIDA_PRESUPUESTO || c.Status_Id == (int)
                                Menfis_Invoices_Status.FACTURA_RECIBIDA_PRESUPUESTO_NOMAIL)
                            .OrderBy(c => c.TIme).Asc
                            .List();
                        if (source3 == null)
                        {
                            return null;
                        }
                        else
                            receivedinvoices = source3.ToList<ReceivedInvoices>();
                        
                    

                   // receivedinvoices = source;
                    //transaction.Commit();
                }
                session.Close();
                return receivedinvoices;
                }
                catch (Exception)
                {
                    session.Close();
                    return null;
                }
            }
        }

        public List<Budget_Party> getBudgetCompanies()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {

                        var source = session.QueryOver<Budget_Party>()
                            .Where(f => f.Initial_status == (int)Menfis_Invoices_Status.FACTURA_RECIBIDA_PRESUPUESTO)
                            .List<Budget_Party>()
                            .ToList<Budget_Party>();
                        budgetparties = source;
                       
                    }
                    session.Close();
                    return budgetparties;
                    //transaction.Commit();
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
