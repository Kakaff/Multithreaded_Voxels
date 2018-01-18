using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Threading.Testing
{
    public class CompletedCalcSqrt : CompletedThreadedWork
    {
        public double num;

        public CompletedCalcSqrt(double result) : base (1,true)
        {
            num = result;
        }
    }
}
