using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvoicesBudgetValidator.Controllers;
using InvoicesBudgetValidator.Model;

namespace InvoicesBudgetValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            BudgetInvoices invoicescontroller = new BudgetInvoices();
            var parties = invoicescontroller.getBudgetCompanies();
            foreach (var party in parties)
            {
                var invoices = invoicescontroller.getInvoices(party.Company_Id);
                var first_invoice = invoices.OrderByDescending(i => i.TIme).First();
                var budget = new BudgetController();
                var current_budget = budget.getCompanyBudget(first_invoice.party_rfc);
                if (first_invoice.Total<=current_budget.Acumulado)
                {
                    Budget new_budget = new Budget() 
                    {
                        Vendor_Id=current_budget.Vendor_Id,
                        RFC = current_budget.RFC,
                        Abono = 0,
                        Cargo = first_invoice.Total,
                        Acumulado = current_budget.Acumulado - first_invoice.Total,
                        Evento_Tipo = (int)Budget_Events.COMPROBANTE_ACEPTADO,
                        Usuario = "SISTEMA",
                        Fecha = DateTime.Now,
                        Referencia = "Actualizacion por cfdi aceptado",
                        URL_Archivo = "",
                        Folio_Fiscal = first_invoice.identifier
                    };
                    budget.insertCompanyBudget(new_budget);
                    Consolidado new_consolidado = new Consolidado()
                    {
                        Vendor_Id = current_budget.Vendor_Id,
                        RFC = current_budget.RFC,
                        Razon_Social = "",
                        Presupuesto = new_budget.Acumulado
                    };
                    budget.updateConsolidado(new_consolidado);
                    
                }
                Console.Write(invoices.ToString());
            }
        }
    }
}
