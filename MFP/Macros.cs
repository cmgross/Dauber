using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFP
{
    public class Macros
    {
        public string Calories { get; set; }
        public string Protein { get; set; }
        public string Fat { get; set; }
        public string Carbs { get; set; }
        public string Fiber { get; set; }

        public Macros()
        {
            Calories = "0";
            Protein = "0";
            Fat = "0";
            Carbs = "0";
            Fiber = "0";
        }

        public override string ToString()
        {
            return Calories + "," + Protein + "," + Fat + "," + Carbs + "," + Fiber;
        }
    }
}
