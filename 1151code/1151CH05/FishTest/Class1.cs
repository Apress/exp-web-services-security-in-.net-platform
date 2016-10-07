using System;

using Twofish;

namespace FishTest
{
   class Class1
   {
      [STAThread]
      static void Main(string[] args)
      {
         try
         {
            Twofish_Algorithm.self_test(16);
            Twofish_Algorithm.self_test(24);
            Twofish_Algorithm.self_test(32);
         }
         catch(Exception ex)
         {
            Console.WriteLine(ex.ToString());
         }
      }
   }
}
