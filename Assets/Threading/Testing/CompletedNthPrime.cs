using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Assets.Threading.Testing
{
    public class CompletedNthPrime : CompletedThreadedWork
    {
        long prime; 
        int primeNum;

        public string Result
        {
            get
            {
                return "";//$"{prime} is the {primeNum}th prime number";
            }
        }
        public CompletedNthPrime(long prime, int primeNum) : base (0,true)
        {
            this.prime = prime;
            this.primeNum = primeNum;
        }
    }
}
