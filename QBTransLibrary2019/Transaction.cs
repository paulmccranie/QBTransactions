using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBTransactions.ViewModels
{
    public class Transaction
    {
        public string Type { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }
        public List<string> Items { get; set; }
        public string TransactionNumber { get; set; }
    }
}
