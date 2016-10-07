namespace RndTest
{
using System;
using WiseOwl.Statistics;

/// <summary>
///    Summary description for Class1.
/// </summary>
public class RndTest
{
    public RndTest()
    {
        //
        // TODO: Add Constructor Logic here
        //
    }

    public static int Main(string[] args)
    {
        int limit = 100;
        UniformDeviate d = new BinomialDeviate (0.5, 100000);
        double sum = 0.0;
        for (int i = 0; i < limit; i++) {
            double v = d.NextDouble ();
            sum += v;

            System.Console.WriteLine (v);
        }
        double average = sum / limit;
        System.Console.WriteLine ("Average value is " + average + ".");
        return 0;
    }
}
}
