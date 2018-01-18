using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Threading.Testing
{
    public class CalcSqrt : ThreadedWork
    {
        int num;
        public CalcSqrt(int num)
        {
            this.num = num;
        }

        public override CompletedThreadedWork Work()
        {
            double res = Math.Sqrt(num);
            return new CompletedCalcSqrt(res);
        }
    }
}
