using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InvoicesBudgetValidator.Controllers;

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
                Console.Write(invoices.ToString());
            }
            


        }
    }
}
