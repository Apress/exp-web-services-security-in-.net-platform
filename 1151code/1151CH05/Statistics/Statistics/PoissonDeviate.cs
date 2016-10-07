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

  /// <summary>
  /// A Poisson-distributed pseudo-random number generator.
  /// </summary>
  /// <remarks>
  /// Some events are rather rare, they don't happen that often.
  /// For instance, car accidents are the exception rather than the rule.
  /// Still, over a period of time, we can say something about the nature of rare events.
  /// An example is the improvement of traffic safety, where the government wants to know
  /// whether seat belts reduce the number of death in car accidents. Here, the Poisson
  /// distribution can be a useful tool to answer question about benefits of seat belt use.
  /// Other phenomena that often follow a Poisson distribution are death of infants,
  /// the number of misprints in a book, the number of customers arriving,
  /// and the number of activations of a geiger counter. The poisson distribution was derived
  /// by the French mathematician Poisson in 1837, and the first application was the description
  /// of the number of deaths by horse kicking in the Prussian army (Bortkiewicz, 1898).<br>
  /// <p>
  /// The Poisson distribution is a mathematical rule that assigns probabilities to the number
  /// of occurences. The only thing we have to know to specify the Poisson distribution is the
  /// mean number of occurences.
  /// </p>
  /// <example>
  /// <p>
  /// In a driving simulation, we start with a mean of 1/2 (we are driving slowly),
  /// and the probability of no accident (x = 0) is large. Over time the simulation increases
  /// the mean, (we are driving faster), and the probability of no accident decreases dramatically.
  /// That is, if you drive very fast you will probably end up in a car crash.
  /// </p>
  /// </example>
  /// <p> 
  /// The Poisson distribution resembles the Binomial distribution if the probability of an
  /// accident is very small. However, if we want to use the Binomial distribution
  /// we have to know both the number of people who make it safely from A to B,
  /// and the number of people who have an accident while driving from A to B,
  /// whereas the number of accidents is sufficient for applying the Poisson distribution.
  /// Thus, the Poisson distribution is cheaper to use because the number of accidents is
  /// usually recorded by the police department, whereas the total number of drivers is not.
  /// </p>
  /// </remarks>
  public class PoissonDeviate : UniformDeviate {
    private double dMean;

    // Cached values
    private double  m_dExpOfMean;
    private double  m_dLogOfMean;
    private double  m_dSqrt2Mean;

    /// <summary>
    /// Create a Poisson-distributed generator with the specified mean.
    /// Start the psuedo-random sequence using a random seed.
    /// <param name='mean'>Average number of successes in a certain time period.</param>
    /// </summary>
    public PoissonDeviate(double mean) {
      Initialize (mean);
    }

    /// <summary>
    /// Create a Poisson-distributed generator with the specified mean.
    /// Start the psuedo-random sequence using the specified seed.
    /// <param name='mean'>Average number of successes in a certain time period.</param>
    /// <param name="newSeed">The initial seed for the generator.</param>
    /// </summary>
    public PoissonDeviate(double mean, int newSeed) : base(newSeed) {
      Initialize (mean);
    }

    private void Initialize(double mean) {
      dMean = mean;
      m_dExpOfMean = Math.Exp (-dMean);
      m_dSqrt2Mean = Math.Sqrt (2.0 * dMean);
      m_dLogOfMean = Math.Log (dMean);
      m_dExpOfMean = dMean * m_dLogOfMean - gammln (dMean + 1.0);
    }

    /// <remarks>
    /// The Poisson distribution is conceptually related to the gamma distribution.
    /// It gives the probability of a certain integer number m of unit rate Poisson
    /// random events occurring in a given interval of time x, while the gamma
    /// distribution is the probability of waiting time between event x and x + dx
    /// to the m-th event.
    /// 
    /// The Poisson distribution is most commonly used to model the number of random
    /// occurrences of some phenomenon in a specified unit of space or time. For example,
    /// The number of phone calls received by a telephone operator in a 10-minute period.
    /// The number of flaws in a bolt of fabric.
    /// 
    /// 
    /// </remarks>
    /// <param name="dMean"> </param>

    override protected double Sample () {
      if (dMean < 12.0) {             // Use direct method
        int result = -1;
        double t = 1.0;
        do {                        
          result++;               
          // Instead of adding exponential deviates, it is equivalent to multiply uniform deviates.
          // We never have to take the log, merely compare to the precomputed exponential.
          t *= base.Sample ();
        } while (t > m_dExpOfMean);
        return result;
      }

      // Use the rejection method

      double em, t2;
      do {
        double y;      // y is a deviate from a Lorentzian comparison function
        do {
          y = Math.Tan (Math.PI * base.Sample ());
          em = m_dSqrt2Mean * y + dMean;  // em is y, shifted and scaled

          // Reject when in area of zero probability
        } while (em < 0.0);

        em = Math.Floor (em);     // Trick for integer valued distributions

        // The ratio of the desired distribution to the comparison function.
        // We accept or reject by comparing to another uniform deviate.
        // The factor 0.9 is chosen so that t never exceeds 1.
        t2 = 0.9 * (1.0 + y * y) * Math.Exp(em * m_dLogOfMean - gammln(em + 1.0) - m_dExpOfMean);
      } while (base.Sample () > t2);
      return (int) em;
    }
  }
}
