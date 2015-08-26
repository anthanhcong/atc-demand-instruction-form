using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Job_Assignment
{
    public class ShiftSummaryContract
    {
        public String ShiftName;
        public TimeSpan FromTime;
        public TimeSpan ToTime;
        public double ValueOnShift;
        public Boolean isOT
        {
            get
            {
                return ValueOnShift > 1;
            }
        }

        public ShiftSummaryContract(string _shiftName, double _val)
        {
            ShiftName = _shiftName;
            ValueOnShift = _val;
        }

    }
}
