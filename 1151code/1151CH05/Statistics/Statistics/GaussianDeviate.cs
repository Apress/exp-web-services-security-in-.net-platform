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
  ///    Summary description for GaussianDeviate.
  /// </summary>
  public class GaussianDeviate : UniformDeviate {
    private bool    m_bGaussianSet;
    private double  m_dCachedGaussian;

    /// <summary>
    /// Create a Gaussian-distributed generator with zero mean and unit variance.
    /// Start the psuedo-random sequence using a random seed.
    /// </summary>
    public GaussianDeviate() {
      m_bGaussianSet = false;
    }

    /// <summary>
    /// Create a Gaussian-distributed generator with zero mean and unit variance.
    /// Start the psuedo-random sequence using the specified seed.
    /// </summary>
    /// <param name="newSeed">The initial seed for the generator.</param>
    public GaussianDeviate(int newSeed) : base(newSeed) {
      m_bGaussianSet = false;
    }

    /// <summary>
    /// Provides a Gaussian deviate with zero mean and unit variance.
    /// </summary>
    override protected double Sample() {
      double fac, rsq, v1, v2;
       
      if (!m_bGaussianSet) {      // There is no extra deviate handy so
        do {
          // Select two uniform deviates in the unit square
          v1 = 2.0 * base.Sample () - 1.0;
          v2 = 2.0 * base.Sample () - 1.0;
          rsq = v1 * v1 + v2 * v2;          // See if they are in the unit circle
        } while (rsq >= 1.0 || rsq == 0.0); //  and if not, try again

        fac = Math.Sqrt(-2.0 * Math.Log(rsq) / rsq);

        // Now make the Box-Muller transformation to get two gaussian (normal) deviates
        // Return one and cache the other for the next call
        m_dCachedGaussian = v2 * fac;
        m_bGaussianSet    = true;
        return v1 * fac;
      }
      m_bGaussianSet    = false;
      return m_dCachedGaussian;
    }
  }
}
