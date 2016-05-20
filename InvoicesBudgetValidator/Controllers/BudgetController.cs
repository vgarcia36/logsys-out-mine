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
    class BudgetController
    {

        Budget source;

        public Budget getCompanyBudget(string rfc)
        {
            using (ISession session = NHibernateHelperBudget.OpenSession())
            {
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        source = session.QueryOver<Budget>()
                                .Where(c => c.RFC == rfc)
                                .SingleOrDefault();
                        //transaction.Commit();
                    }
                    session.Close();
                    return source;
                }
                catch (Exception)
                {
                    session.Close();
                    return null;
                }
            }
            
        }


        public void insertCompanyBudget(Budget newbudget)
        {
            using (ISession session = NHibernateHelperBudget.OpenSession())
            {
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(newbudget);
                        
                        transaction.Commit();
                    }
                    session.Close();
                   // return source;
                }
                catch (Exception)
                {
                    session.Close();
                   // return null;
                }
            }
        }

        public void updateConsolidado(Consolidado newconsolidado)
        {
            using (ISession session = NHibernateHelperBudget.OpenSession())
            {
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(newconsolidado);

                        transaction.Commit();
                    }
                    session.Close();
                    // return source;
                }
                catch (Exception)
                {
                    session.Close();
                    // return null;
                }
            }
        }


    }
}
