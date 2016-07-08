using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Model
{
        public enum Menfis_Invoices_Status
        {
            FACTURA_RECIBIDA = 1,
            FACTURA_CANCELADA = 2,
            FACTURA_RECIBIDA_PRESUPUESTO = 6,
            FACTURA_RECIBIDA_PRESUPUESTO_NOMAIL = 7,
            FACTURA_CANCELDA_PRESUPUESTO = 8
        }
 }
