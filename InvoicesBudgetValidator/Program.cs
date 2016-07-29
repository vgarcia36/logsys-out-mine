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

            log4net.Config.XmlConfigurator.Configure();

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
                                    mailsender.Send_Mail("El comprobante con UUID " + first_invoice.Identifier + " ha sido aprovada.", defaultmail, "Factura aprovada");

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
                                    mailsender.Send_Mail("El comprobante con UUID " + first_invoice.Identifier + " ha sido validada exitosamente, sin embargo, aún no cuenta con aprobación para accesar a nuestro sistema. Por favor comuníquese con el contacto que le solicitó el bien o servicio.", defaultmail, "Validación de comprobante fiscal.");
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
                                    mailsender.Send_Mail("El comprobante con UUID " + first_invoice.Identifier + " ha sido aprovada.", defaultmail, "Factura aprovada");

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
                                    mailsender.Send_Mail("El comprobante con UUID " + first_invoice.Identifier + " ha sido validada exitosamente, sin embargo, aún no cuenta con aprobación para accesar a nuestro sistema. Por favor comuníquese con el contacto que le solicitó el bien o servicio.", defaultmail, "Validación de comprobante fiscal.");
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
