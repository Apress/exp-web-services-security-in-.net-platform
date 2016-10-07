using System;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace PortScanner
{
	class Class1
	{
      private const int MAXPORT=1500;

		[STAThread]
		static void Main(string[] args)
		{
         string sIP;

         foreach(string arg in args)
         {
            Match m = Regex.Match(arg, "help");
            if(m.Success)
            {
               Console.WriteLine("portscanner [IP Address]\n");
            }
         }

         if(args.Length>0)
         {
            sIP = args[0].ToString();
         }
         else
         {
            sIP = "127.0.0.1";
         }

         for(int i=0; i<=MAXPORT;i++)
         {
            TcpClient tc = null;	
            try
            {
               tc = new TcpClient(sIP, i);

               if(tc != null)
               {
                  Console.WriteLine(sIP + ":" + i.ToString());
                  Console.WriteLine("Connected");
               }

               tc.Close();
            }
            catch (System.Exception ex)
            {
               Console.WriteLine(sIP + ":" + i.ToString() + ":\t");
               Console.WriteLine(ex.Message.Replace("\n", ""));
            }
         }
		}
	}
}
