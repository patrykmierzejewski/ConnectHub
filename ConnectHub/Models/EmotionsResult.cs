using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectHub.Models
{
    internal class EmotionsResult
    {
        public string Category { get; set; }
        public decimal Score { get; set; }
        public string AudioTimeString { get; set; }
    }
}
