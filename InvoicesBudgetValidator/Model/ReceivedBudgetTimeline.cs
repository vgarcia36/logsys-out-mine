using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesBudgetValidator.Model
{
    class ReceivedBudgetTimeline
    {
        public virtual int Company_Id { get; set; }
        public virtual string User_Id { get; set; }
        public virtual DateTime Time { get; set; }
        public virtual DateTime Time_Tx { get; set; }
        public virtual string Identifier { get; set; }
        public virtual int Received_Status_Id { get; set; }
        public virtual Byte[] Received_Timeline_Id { get; set; }
    }
}
