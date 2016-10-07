/*
 * Copyright (c) 2002, Brent Rector
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer. 
 * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution. 
 * Neither the name of the Wise Owl, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission. 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;

namespace WiseOwl.Statistics {

  // Linear congruential generator
  // I(j+1) = aI(j) + c (mod m)

  // Simple multiplicative congruential generator
  // I(j+1) = aI(j) (mod m)

  /// <summary>
  /// A uniformly-distributed, extremely long period (> 2.3 * 10^18)
  /// pseudo-random number generator.
  /// </summary>
  public class UniformDeviate : System.Random {
    /// <summary>M1 - 1 = 2 x 3 x 7 x 631 x 81031</summary>
    private const int    M1  = 2147483563;
    private const int    A1  = 40014;

    private const int    Q1  = 53668;
    private const int    R1  = 12211;

    /// <summary>M2 - 1 = 2 x 19 x 31 x 1019 x 1789</summary>
    private const int    M2  = 2147483399;
    private const int    A2  = 40692;

    private const int    Q2  = 52774;
    private const int    R2  = 3791;

    private const double AM   = (1.0/M1);
    private const int    NTAB = 32;
    private const int    NDIV = (M1/NTAB);

    // smallest representable number such that 1.0+DBL_EPSILON != 1.0
    private const double DBL_EPSILON = 2.2204460492503131e-016;

    /// <summary>
    /// RNMX should approximate the largest floating value that is less than 1.
    /// </summary>
    private const double RNMX = (1.0-DBL_EPSILON);

    private int     idum;
    private int     idum2;
    private int     iy;
    private int []  iv = new int[NTAB];

    /// <summary>
    /// Instantiates with a random seed based on date/time.
    /// </summary>
    public UniformDeviate() {
      ReloadShuffleTable (Environment.TickCount + 1);
    }

    /// <summary>
    /// Instantiates with a specific seed for a repeatable sequence.
    /// </summary>
    /// <param name="newSeed">Initial seed for generator</param>

    public UniformDeviate(int newSeed) {
      ReloadShuffleTable (newSeed);
    }

    /// <summary>
    /// Provides a uniform deviate that lies within the range 0.0 to 1.0 
    /// (exclusive of the endpoint values).
    /// </summary>
    /// <remarks>
    /// This generator produces an extended random sequence by combining two
    /// different sequences with different periods so as to obtain a new sequence
    /// whose period is the least common multiple of the two periods using the
    /// L'Ecuyer method (1988, Communications of the ACM, vol. 31, pp 742-774)
    /// with Bays-Durham shuffle and added safeguards.
    /// </remarks>

    // Code derived from Numerical Recipes in C, Second edition, p 282.

    override protected double Sample() {

      // Schrage's algorithm is based on an approximate factorization of m,
      // m = aq + r, i.e., q = [m/a], r = m mod a
      // with square brackets denoting integer part
      //
      // When r is small, specifically r < q, and 0 < z < m - 1,
      // then both a(z mod q) and r[z/q] lie in the range 0, ..., m - 1
      // and that
      // 
      // az mod m = a(z mod q) - r[z/q]       if it is >= 0
      //            a(z mod q) - r[z/q] + m   otherwise

      int k = idum / Q1;
      idum = A1 * (idum - k*Q1) - k*R1;   // Compute idum = (A1 * idum) % M1
      if (idum < 0)                       //  without overflows by Schrage's method
        idum += M1;

      k = idum2 / Q2;
      idum2 = A2 * (idum2 - k*Q2) - k*R2; // Compute idum2 = (A2 * idum2) % M2 likewise
      if (idum2 < 0)
        idum2 += M2;

      int j = iy / NDIV;                  // Will be in the range 0..NTAB-1
      iy = iv[j] - idum2;                 // Here idum is shuffled, idum and idum2 are
      iv[j] = idum;                       //  combined to generate output
      if (iy < 1)
        iy += M1 - 1;

      double temp = AM * iy;
   
      if (temp > RNMX)
        return RNMX;     // Because users don't expect endpoint values
      else
        return temp;
    }

    /// <summary>
    /// Reload the shuffle table. We use the shuffle table
    /// to remove low-order serial correlations in the random deviates. A random deviate
    /// derived from the j-th value in the sequence, I(j), is output not on the j-th call
    /// but rather on a randomized later call, j + 32 on average. The shuffling algorithm
    /// is due to Bays and Durham as described in Knuth (1981, Seminumerical Algorithms,
    /// 2nd ed. vol. 2 of The Art of Computer Programming, 3.2-3.3).
    /// </summary>

    protected void ReloadShuffleTable(int newSeed) {
      if (0 == newSeed)
        throw new ArgumentOutOfRangeException("newSeed", "New seed value cannot be zero.");

      idum = newSeed;
      if (idum <= 0) {                        // Initialize
        idum  = -idum;
        idum2 =  idum;
      }

      for (int j = NTAB + 7; j >= 0; j--) {   // Load the shuffle table (after 8 warm-ups)
        int k = idum / Q1;                  // k = [z/q]
        idum = A1 * (idum - k*Q1) - k*R1;   // idum - k*Q1 = z mod q
        if (idum < 0)
          idum += M1;

        if (j < NTAB)                       // Fill shuffle table entry
          iv[j] = idum;
      }
      iy = iv[0];
    }

    protected static double [] cof;

    protected double gammln (double xx) {
      if (cof == null) {
        cof = new double [6];
            
        cof [0] =  76.18009172947146;
        cof [1] = -86.50532032941677;
        cof [2] =  24.01409824083091;
        cof [3] =  -1.231739572450155;
        cof [4] =   0.1208650973866179e-2;
        cof [5] =  -0.5395239384953e-5;
      }
       
      double x = xx;
      double y = x;
        
      double tmp = x + 5.5;
      tmp -= (x + 0.5) * Math.Log(tmp);

      double ser = 1.000000000190015;

      for (int j = 0; j <= 5; j++)
        ser += cof[j] / ++y;
      return -tmp + Math.Log (2.5066282746310005 * ser / x);
    }
  }
}
