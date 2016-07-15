using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvoicesBudgetValidator.Controllers;
using InvoicesBudgetValidator.Model;
using InvoicesBudgetValidator.Helpers;

namespace InvoicesBudgetValidator
{
    class Program
    {


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
   ("Program");

        static void Main(string[] args)
        {

            //esta parte del codigo busca y modifica las facturas con presupuesto que tengan status cancelas

          /*  var cancelled = new CancelledBudgetController();
            var canceled_invoices = cancelled.getCancelledInvoices();

            if (canceled_invoices != null)
            {
                foreach (var invoice in canceled_invoices)
                {
                    var budget = new BudgetController();
                    var current_budget = budget.getCompanyBudget(invoice.Party_rfc);
                    if (current_budget != null)
                    {
                        if (invoice.Invoice.Substring(0, 2) == "NC")
                        {
                            var insertrtcompleted = budget.insertBudget(invoice, current_budget, (int)Budget_Events_Presupuesto.NOTA_DE_CREDITO_CANCELADA);

                            if (insertrtcompleted)
                            {
                                var mailsender = new Mail_Sender();
                                mailsender.Send_Mail("", "", "Factura aceptada");

                                var request = new RequestCreator();
                                request.createRequestUpdateStatus(2, invoice.Invoice_Id.Remove(0, 1));
                            }
                        }
                        else{
                        var insertrtcompleted = budget.insertBudget(invoice, current_budget, (int)Budget_Events_Presupuesto.FACTURA_CANCELADA);

                        if (insertrtcompleted)
                        {
                            var request = new RequestCreator();
                            request.createRequestUpdateStatus(2, invoice.Invoice_Id.Remove(0, 1));
                        }    
                        }
                    }
                    
                }
            }

            */




            try
            {



                //esta parte del codigo busca y modifica las facturas recibidas

                BudgetInvoices invoicescontroller = new BudgetInvoices();
                var parties = invoicescontroller.getBudgetCompanies();
                foreach (var party in parties)
                {
                    var invoices = invoicescontroller.getInvoices(party.Company_Id);
                    //var first_invoice = invoices.OrderByDescending(i => i.TIme).First();

                    if (invoices.Count > 0)
                    {


                        var rfcs = invoices.Select(x => x.Party_rfc).ToList<String>();

                        foreach (var rfc in rfcs)
                        {
                            var first_invoice = invoices.Where(i => i.Party_rfc == rfc).OrderBy(i => i.TIme).First();
                            first_invoice.Total *= first_invoice.Exchange_Rate;
                            var budget = new BudgetController();
                            var current_budget = budget.getCompanyBudget(rfc,first_invoice.Company_Id);



                            //si es nota de credito inserta directo al presupuesto 
                            if (first_invoice.Type_Id == (int)Invoices_types.EGRESO && current_budget != null)
                            {
                                var insertrtcompleted = budget.insertBudget(first_invoice, current_budget, (int)Budget_Events_Presupuesto.NOTA_DE_CREDITO_ACEPTADA);

                                if (insertrtcompleted)
                                {
                                    var mailsender = new Mail_Sender();
                                    var monngomails = new MongoMails();
                                    string defaultmail = monngomails.getResponseMail(new Default_Mail() { company_id=first_invoice.Company_Id,rfc=first_invoice.Party_rfc});
                                    mailsender.Send_Mail("", defaultmail, "Factura aceptada");

                                    var request = new RequestCreator();
                                    request.createRequestUpdateStatus((int)Menfis_Invoices_Status.FACTURA_RECIBIDA, first_invoice.Invoice_Id.Remove(0, 1));
                                }
                                continue;
                            }
                            else if (first_invoice.Type_Id == (int)Invoices_types.EGRESO && current_budget == null)
                            {
                                var request = new RequestCreator();
                                request.createRequestUpdateStatus((int)Menfis_Invoices_Status.FACTURA_RECIBIDA_PRESUPUESTO_NOMAIL, first_invoice.Invoice_Id.Remove(0, 1));
                            }

                            //valida si la empresa cuenta con un registro de presupuesto, si no lo tiene lo manda al status de sin presupuesto
                            if (current_budget == null)
                            {
                                if (first_invoice.Status_Id == (int)Menfis_Invoices_Status.FACTURA_RECIBIDA_PRESUPUESTO)
                                {
                                    var mailsender = new Mail_Sender();
                                    var monngomails = new MongoMails();
                                    string defaultmail = monngomails.getResponseMail(new Default_Mail() { company_id = first_invoice.Company_Id, rfc = first_invoice.Party_rfc });
                                    mailsender.Send_Mail("", defaultmail, "Su factura no cuenta con presupuesto.");
                                    var request = new RequestCreator();
                                    request.createRequestUpdateStatus((int)Menfis_Invoices_Status.FACTURA_RECIBIDA_PRESUPUESTO_NOMAIL, first_invoice.Invoice_Id.Remove(0, 1));
                                }

                            }
                            //valida si la empresa cuenta con presupuesto y si el presupuesto es suficiente, si no lo manda al status de sin presupuesto
                            else if (first_invoice.Total <= current_budget.Presupuesto && current_budget != null && (first_invoice.Type_Id == (int)Invoices_types.INGRESO || first_invoice.Type_Id == (int)Invoices_types.TRASLADO) )
                            {

                                var insertrtcompleted = budget.insertBudget(first_invoice, current_budget, (int)Budget_Events_Presupuesto.FACTURA_ACEPTADA);

                                if (insertrtcompleted)
                                {
                                    var mailsender = new Mail_Sender();
                                    var monngomails = new MongoMails();
                                    string defaultmail = monngomails.getResponseMail(new Default_Mail() { company_id = first_invoice.Company_Id, rfc = first_invoice.Party_rfc });
                                    mailsender.Send_Mail("", defaultmail, "Factura aceptada");

                                    var request = new RequestCreator();
                                    request.createRequestUpdateStatus((int)Menfis_Invoices_Status.FACTURA_RECIBIDA, first_invoice.Invoice_Id.Remove(0, 1));
                                }
                            }
                            else if (first_invoice.Total >= current_budget.Presupuesto && current_budget != null)
                            {
                                if (first_invoice.Status_Id == (int)Menfis_Invoices_Status.FACTURA_RECIBIDA_PRESUPUESTO)
                                {
                                    var mailsender = new Mail_Sender();
                                    var monngomails = new MongoMails();
                                    string defaultmail = monngomails.getResponseMail(new Default_Mail() { company_id = first_invoice.Company_Id, rfc = first_invoice.Party_rfc });
                                    mailsender.Send_Mail("", defaultmail, "Su factura no cuenta con presupuesto.");
                                    var request = new RequestCreator();
                                    request.createRequestUpdateStatus((int)Menfis_Invoices_Status.FACTURA_RECIBIDA_PRESUPUESTO_NOMAIL, first_invoice.Invoice_Id.Remove(0, 1));
                                }
                            }

                            //Console.Write(invoices.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                //throw;
            }
        }
    }
}
