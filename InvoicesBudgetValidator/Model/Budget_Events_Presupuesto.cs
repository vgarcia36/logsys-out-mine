using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Model
{
        public enum Budget_Events_Presupuesto
        {
            ARCHIVO_PRESUPUESTO = 1,
            FACTURA_ACEPTADA = 2,
            NOTA_DE_CREDITO_ACEPTADA = 3,
            FACTURA_CANCELADA = 4,
            NOTA_DE_CREDITO_CANCELADA = 5,
            FACTURA_RECIBIDA = 6,
            FACTURA_RECIBIDA_NOMAIL = 7
        }
}
