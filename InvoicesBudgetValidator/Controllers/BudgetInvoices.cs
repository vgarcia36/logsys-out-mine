using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicesBudgetValidator.Helpers;
using InvoicesBudgetValidator.Model;
using NHibernate;

namespace InvoicesBudgetValidator.Controllers
{
    class BudgetInvoices
    {
        List<Budget_Party> budgetparties;
        List<ReceivedInvoices> receivedinvoices;

        public List<ReceivedInvoices> getInvoices(int company)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                try
                {

                
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var source = session.QueryOver<ReceivedInvoices>()
                            .Where(c => c.Company_Id == company).And(c => c.Status_Id == 6)
                            .List<ReceivedInvoices>()
                            .ToList<ReceivedInvoices>();
                    receivedinvoices = source;
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
                            .Where(f => f.Budget_Is_Required)
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
