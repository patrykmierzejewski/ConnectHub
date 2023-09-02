using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectHub.Models
{
    public class OperatorRankingToday
    {
        public int Position { get; set; }
        public string Day { get; set; }
        public string OperatorName { get; set; }
        public int OpeartorScore { get; set; }
        public int OperatorSale { get; set; }
        public int OperatorSpechRate { get; set; }
    }
}
