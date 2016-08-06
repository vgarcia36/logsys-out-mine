using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InvoicesBudgetValidator.Helpers;
using InvoicesBudgetValidator.Model;
using NHibernate;

namespace InvoicesBudgetValidator.Controllers
{
    class BudgetController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
        ("Program");
        Consolidado source;

        public Consolidado getCompanyBudget(string rfc, long company)
        {
            using (ISession session = NHibernateHelperBudget.OpenSession())
            {
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        source = session.QueryOver<Consolidado>()
                                .Where(c => c.RFC == rfc && c.Company_Id == company)
                                .List()
                                .Last();
                        //transaction.Commit();
                    }
                    session.Close();
                    return source;
                }
                catch (Exception e)
                {
                    session.Close();
                    log.Error(e);
                    return null;
                }
            }
            
        }


        private bool insertCompanyBudget(Budget newbudget, Consolidado newconsolidado, ReceivedInvoices first_invoice)
        {

            using (ISession session = NHibernateHelperBudget.OpenSession())
            {
                try
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        try
                        {
                            

                            session.Update(newconsolidado);

                            session.Save(newbudget);

                            transaction.Commit();

                            var request = new RequestCreator();
                            request.createRequestUpdateStatus((int)Menfis_Invoices_Status.FACTURA_RECIBIDA, first_invoice.Invoice_Id.Remove(0, 1));

                            session.Close();

                            return true;
                        }
                        catch (WebException e)
                        {
                            transaction.Rollback();
                            log.Error(e);
                            return false;
                            //throw;
                        }


                        catch (Exception e)
                        {
                            transaction.Rollback();
                            log.Error(e);
                            return false;
                            //throw;
                        }
                    }
                    
                   // return source;
                }
                catch (Exception e)
                {
                    session.Close();
                    log.Error(e);
                    return false;
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
                        session.SaveOrUpdate(newconsolidado);

                        transaction.Commit();
                    }
                    session.Close();
                    // return source;
                }
                catch (Exception e)
                {
                    session.Close();
                    log.Error(e);
                    // return null;
                }
            }
        }


        public bool insertBudget(ReceivedInvoices first_invoice, Consolidado current_budget, int event_type, ReceivedInvoices invoice)
        {

            try
            {


                var operations = getOperations(event_type, first_invoice);

                var cargo = operations[1];
                var abono = operations[0];
                var total = operations[2];

                Budget new_budget = new Budget()
                {
                    Company_Id = first_invoice.Company_Id,
                    Vendor_Id = current_budget.Vendor_Id,
                    RFC = current_budget.RFC,
                    Abono = abono,
                    Cargo = cargo,
                    Acumulado = makeOperation(event_type, current_budget.Presupuesto, total),
                    id_evento = event_type,
                    id_usuario = 10,
                    Usuario = "SISTEMA",
                    Fecha = DateTime.Now,
                    Referencia = getReference(event_type),
                    Id_Archivo = null,
                    Folio_Fiscal = first_invoice.Identifier
                };
            //insertCompanyBudget(new_budget);

            Consolidado new_consolidado = new Consolidado()
            {
                Id_presupuesto = current_budget.Id_presupuesto,
                Company_Id = current_budget.Company_Id,
                Vendor_Id = current_budget.Vendor_Id,
                RFC = current_budget.RFC,
                Razon_Social = first_invoice.Party,
                Presupuesto = new_budget.Acumulado
            };
            //updateConsolidado(new_consolidado);


            return insertCompanyBudget(new_budget,new_consolidado,invoice);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                log.Error(e);
                return false;
            }

        }

        public bool insertBudget(Received_Archived first_invoice, Budget current_budget, int event_type, ReceivedInvoices invoice)
        {

            try
            {

                var operations = getOperations(event_type, first_invoice);

                var cargo = operations[1];
                var abono = operations[0];
                var total = operations[2];

                Budget new_budget = new Budget()
                {
                    Company_Id = first_invoice.Company_Id,
                    Vendor_Id = current_budget.Vendor_Id,
                    RFC = current_budget.RFC,
                    Abono = abono,
                    Cargo = cargo,
                    Acumulado = makeOperation(event_type, current_budget.Acumulado,total),
                    id_evento = event_type,
                    Usuario = "SISTEMA",
                    Fecha = DateTime.Now,
                    Referencia = getReference(event_type),
                    Id_Archivo = null,
                    Folio_Fiscal = first_invoice.Identifier
                };
                //insertCompanyBudget(new_budget);

                Consolidado new_consolidado = new Consolidado()
                {
                    Company_Id = current_budget.Company_Id,
                    Vendor_Id = current_budget.Vendor_Id,
                    RFC = current_budget.RFC,
                    Razon_Social = first_invoice.Party,
                    Presupuesto = new_budget.Acumulado
                };
                //updateConsolidado(new_consolidado);

                return insertCompanyBudget(new_budget,new_consolidado,invoice);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                log.Error(e);
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

        private decimal[] getOperations(int event_type, dynamic invoice)
        {
            decimal[] result = new decimal[3];

            switch (event_type)
            {
                case (int)Budget_Events_Presupuesto.FACTURA_ACEPTADA:
                    result[0] = 0;
                    result[1] = invoice.Total;
                    break;
                case (int)Budget_Events_Presupuesto.NOTA_DE_CREDITO_ACEPTADA:
                    result[1] = 0;
                    result[0] = invoice.Total;
                    break;
                case (int)Budget_Events_Presupuesto.FACTURA_CANCELADA:
                    result[1] = 0;
                    result[0] = invoice.Total;
                    break;
                case (int)Budget_Events_Presupuesto.NOTA_DE_CREDITO_CANCELADA:
                    result[0] = 0;
                    result[1] = invoice.Total;
                    break;
            }

            result[2] = invoice.Total;

            return result;

        }




    }
}
