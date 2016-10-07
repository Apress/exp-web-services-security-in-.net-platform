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

  /// <remarks>
  /// The binomial probability distribution describes experiments
  /// with repeated, identical trials. Each trial can have only two
  /// possible outcomes: 'Success' or 'Fail'. The probability for 'Success', p,
  /// is the same for all trials. The number of trials, N, is fixed.
  /// The probability of observing n hits is given by 
  /// <code>
  ///                     N!        n       (N-n)
  ///     B(n, N, p) = ---------   p   (1-p)
  ///                  n! (N-n)!
  /// </code> 
  ///
  /// One example of such an experiment is tossing a coin and counting 'heads'.
  /// The mean of the probability distribution is N*p, while the variance is N*p*(1-p). 
  ///
  /// If n hits are observed after N trials, the hit probability can be estimated as p=n/N.
  /// The error on p is given by the error on the number of hits divided by N: 
  ///
  /// <code>
  ///                                        ___       _________
  ///                1      __________     \/ n       / N - n
  ///     sigma_p = ---   \/ N p (1-p)  =  -----  \  / --------
  ///                N                       N     \/     N
  /// </code>
  ///
  /// Poisson statistics would have lead only to the first term.
  /// The second term is always less than 1.
  /// </remarks>

  public class BinomialDeviate : UniformDeviate {
    private double  specifiedProbability;
    private double  normalizedProbability;

    private int     trials;
    private double  gammlnOfTrials;

    // Cached values
    private double  logOfOneMinusProbability;
    private double  logOfProbability;

    public BinomialDeviate(double probability, int cTrials, int newSeed) : base(newSeed) {
      Initialize (probability, cTrials);
    }

    public BinomialDeviate(double probability, int cTrials) {
      Initialize (probability, cTrials);
    }

    private void Initialize(double probability, int cTrials) {
      specifiedProbability = probability;

      // The binomial distribution is invariant under changing specifiedProbability to 1 - specifiedProbability,
      //  if we also change the answer to a minus itself. We will do this below.
      normalizedProbability = (specifiedProbability <= 0.5 ? specifiedProbability : 1.0 - specifiedProbability);

      logOfProbability         = Math.Log (normalizedProbability);
      logOfOneMinusProbability = Math.Log (1.0 - normalizedProbability);

      trials         = cTrials;
      gammlnOfTrials = gammln(trials + 1.0);
    }

    override protected double Sample() {

      int bnl = 0;

      // This is the mean of the deviate to be produced
      double dMeanOfDeviate = trials * normalizedProbability;

      if (trials < 25) {        // Use the direct method when number of trials small
        // This can require up to 25 calls to UniformDeviate
        for (int j = 0; j < trials; j++) {
          if (base.Sample () < normalizedProbability)
            ++bnl;
        }
      } 
      else if (dMeanOfDeviate < 1.0) {            // If fewer than one event is expected out of 25 or more trials,
        double g = Math.Exp(-dMeanOfDeviate);   //  then the distribution is quite accurately Poisson.
        double t = 1.0;                         // Use direct Poisson method
        int j;
        for (j = 0; j <= trials; j++) {
          t *= base.Sample ();
          if (t < g)
            break;
        }
        bnl = (j <= trials ? j : trials);
      }

        // Use the rejection method
      else {

        // Implement rejection method using a Lorentzian comparison function
        double sq = Math.Sqrt(2.0 * dMeanOfDeviate * (1.0 - normalizedProbability));
        double em, t;
        do {
          double y;
          do {
            double angle = Math.PI * base.Sample ();
            y = Math.Tan(angle);
            em = sq * y + dMeanOfDeviate;

            // Reject when in area of zero probability
          } while (em < 0.0 || em >= (trials + 1.0));

          em = Math.Floor(em);     // Trick for integer valued distributions

          // The ratio of the desired distribution to the comparison function.
          // We accept or reject by comparing to another uniform deviate.
          t = 1.2 * sq * (1.0 + y * y) *
            Math.Exp (gammlnOfTrials -
            gammln (em + 1.0) -
            gammln (trials - em + 1.0) +
            em * logOfProbability +
            (trials - em) * logOfOneMinusProbability);

          // Reject: This happens about 1.5 times per deviate, on average
        } while (base.Sample () > t);

        bnl = (int) em;
      }
      if (normalizedProbability != specifiedProbability)      // Undo the symmetry transform when necessary
        bnl = trials - bnl;

      return bnl;
    }
  }
}
