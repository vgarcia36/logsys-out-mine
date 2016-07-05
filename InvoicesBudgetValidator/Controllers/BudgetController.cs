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
                                .List()
                                .OrderBy(x => x.Fecha)
                                .Last();
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


        private void insertCompanyBudget(Budget newbudget)
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

        private void updateConsolidado(Consolidado newconsolidado)
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


        public bool insertBudget(ReceivedInvoices first_invoice, Budget current_budget, int event_type)
        {

            try
            {


            Budget new_budget = new Budget()
            {
                Company_Id = first_invoice.Company_Id,
                Vendor_Id = current_budget.Vendor_Id,
                RFC = current_budget.RFC,
                Abono = 0,
                Cargo = first_invoice.Total,
                Acumulado = makeOperation(event_type,current_budget.Acumulado, first_invoice.Total),
                Evento_Tipo = event_type,
                Usuario = "SISTEMA",
                Fecha = DateTime.Now,
                Referencia = getReference(event_type),
                URL_Archivo = "",
                Folio_Fiscal = first_invoice.Identifier
            };
            insertCompanyBudget(new_budget);

            Consolidado new_consolidado = new Consolidado()
            {
                Company_Id = current_budget.Company_Id,
                Vendor_Id = current_budget.Vendor_Id,
                RFC = current_budget.RFC,
                Razon_Social = first_invoice.Party,
                Presupuesto = new_budget.Acumulado
            };
            updateConsolidado(new_consolidado);

            return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool insertBudget(Received_Archived first_invoice, Budget current_budget, int event_type)
        {

            try
            {


                Budget new_budget = new Budget()
                {
                    Company_Id = first_invoice.Company_Id,
                    Vendor_Id = current_budget.Vendor_Id,
                    RFC = current_budget.RFC,
                    Abono = 0,
                    Cargo = first_invoice.Total,
                    Acumulado = makeOperation(event_type, current_budget.Acumulado, first_invoice.Total),
                    Evento_Tipo = event_type,
                    Usuario = "SISTEMA",
                    Fecha = DateTime.Now,
                    Referencia = getReference(event_type),
                    URL_Archivo = "",
                    Folio_Fiscal = first_invoice.Identifier
                };
                insertCompanyBudget(new_budget);

                Consolidado new_consolidado = new Consolidado()
                {
                    Company_Id = current_budget.Company_Id,
                    Vendor_Id = current_budget.Vendor_Id,
                    RFC = current_budget.RFC,
                    Razon_Social = first_invoice.Party,
                    Presupuesto = new_budget.Acumulado
                };
                updateConsolidado(new_consolidado);

                return true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        private decimal makeOperation(int event_type, decimal acumulado , decimal invoice_total)
        {

            decimal result = 0;

            switch (event_type)
            {
                case (int)Budget_Events_Presupuesto.FACTURA_ACEPTADA:
                    result = acumulado - invoice_total;
                    break;
                case (int)Budget_Events_Presupuesto.NOTA_DE_CREDITO_ACEPTADA:
                    result = acumulado + invoice_total;
                    break;
                case (int)Budget_Events_Presupuesto.FACTURA_CANCELADA:
                    result = acumulado + invoice_total;
                    break;
                case (int)Budget_Events_Presupuesto.NOTA_DE_CREDITO_CANCELADA:
                    result = acumulado - invoice_total;
                    break;
            }

            return result;

        }

        private string getReference(int event_type)
        {

            string result = "";

            switch (event_type)
            {
                case (int)Budget_Events_Presupuesto.FACTURA_ACEPTADA:
                    result = "FACTURA_ACEPTADA";
                    break;
                case (int)Budget_Events_Presupuesto.NOTA_DE_CREDITO_ACEPTADA:
                    result = "NOTA_DE_CREDITO_ACEPTADA";
                    break;
                case (int)Budget_Events_Presupuesto.FACTURA_CANCELADA:
                    result = "FACTURA_CANCELADA";
                    break;
                case (int)Budget_Events_Presupuesto.NOTA_DE_CREDITO_CANCELADA:
                    result = "NOTA_DE_CREDITO_CANCELADA";
                    break;
            }

            return result;

        }




    }
}
