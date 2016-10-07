using System;
using System.Net;
using System.IO;
using System.Text;

namespace WebOptions
{
	class Fingerprint
	{
		[STAThread]
		static int Main(string[] args)
		{
         if(args.Length<=0)
         {
            DisplayHelp();
         }
         else
         {
            try
            {
               string suri = args[0].ToString();
               Uri uri = new Uri(args[0].ToString());

               WebClient myWebClient = new WebClient();

               myWebClient.OpenRead(suri);
               WebHeaderCollection myWebHeaderCollection = myWebClient.ResponseHeaders;
               
               Console.WriteLine("\nDisplaying the Server Header\n");

               for (int i=0; i < myWebHeaderCollection.Count; i++)                
                  Console.WriteLine ("\t" + myWebHeaderCollection.GetKey(i) + " = " + myWebHeaderCollection.Get(i));
         
               Console.WriteLine("\nDisplaying the Server OPTIONS Fingerprint\n");

               byte[] byteArray = Encoding.ASCII.GetBytes("Hello");
               byte[] responseArray = myWebClient.UploadData(suri,"OPTIONS", byteArray);
               myWebHeaderCollection = myWebClient.ResponseHeaders;
               
               for (int i=0; i < myWebHeaderCollection.Count; i++)                
                  Console.WriteLine ("\t" + myWebHeaderCollection.GetKey(i) + " = " + myWebHeaderCollection.Get(i));

               return 0;
            }
            catch(Exception ex)
            {
               Console.WriteLine(ex.ToString());
            }
         }

         return 1;
      }

      private static void DisplayHelp()
      {
         Console.WriteLine("Must input valid URI");
      }
	}
}
