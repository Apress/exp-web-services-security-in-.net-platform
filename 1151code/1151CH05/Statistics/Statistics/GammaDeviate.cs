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
  ///    Summary description for GammaDeviate.
  /// </summary>
  public class GammaDeviate : UniformDeviate {
    private int     order;

    /// <summary>
    /// Create a gamma-distributed generator with the specified order.
    /// Start the psuedo-random sequence using a random seed.
    /// </summary>
    /// <param name="nOrder">Integer order of gama distribution. Must be greater than zero.</param>
    public GammaDeviate(int nOrder) {
      order = nOrder;
    }

    /// <summary>
    /// Create a gamma-distributed generator with the specified order.
    /// Start the psuedo-random sequence using the specified seed.
    /// </summary>
    /// <param name="nOrder">Integer order of gama distribution. Must be greater than zero.</param>
    /// <param name="newSeed">The initial seed for the generator.</param>
    /// </summary>
    public GammaDeviate(int nOrder, int newSeed) : base(newSeed) {
      order = nOrder;
    }

    /// <remarks>
    /// Returns a value from the gamma distribution of integer order <paramref name="order"/>.
    /// 
    /// This is the waiting time to the <paramref name="order"/>-th event
    /// in a Poisson random process of unit mean.
    /// <example>
    /// When <paramRef name="order"/> = 1, it is simply the exponential distribution -
    /// the waiting time to the first event.
    /// </example>
    /// A gamma deviate has a probability p(a)(x)dx of occurring with a value between x and x + dx.
    /// </remarks>

    override protected double Sample() {
      if (order < 1)
        throw new ArgumentOutOfRangeException("order", "The order cannot be less than one.");

      // To generate deviates for small values of 'order',
      // it is best to add up 'order' exponentially distributed waiting times,
      // i.e. logarithms of uniform deviates.
      // As the sum of logarithms is the logarithm of the product,
      // one really has only to generate the product of a uniform deviate, then take the log.

      if (order < 6) {   // Use direct method, adding waiting times
        double newValue = 1.0;
        for (int j = 0; j < order; j++) {
          newValue *= base.Sample ();
        }
        return -Math.Log(newValue);
      }

      // For larger values of 'order', the distribution has a typically bell-shaped
      // form, with a peak at x = 'order' and a half-width of about sqrt('order').

      // Use rejection method
      double e, x;
      do {
        double am, s, y;

        do {
          double v1, v2;

          // These six lines generate the tangent of a random angle
          // They are equivalent to:
          // y = tan (pi * UniformDeviate)
          do {
            v1 = base.Sample ();
            v2 = 2.0 * base.Sample () - 1.0;
          } while (v1 * v1 + v2 * v2 > 1.0);
          y = v2 / v1;

          am = order - 1;
          s = Math.Sqrt (2.0 * am + 1.0);
          x = s * y + am;
        } while (x <= 0.0);     // We decide whether to reject x

        e = (1.0 + y * y) * Math.Exp (am * Math.Log (x / am) - s * y);
      } while (base.Sample () > e);
      return x;
    }
  }
}
