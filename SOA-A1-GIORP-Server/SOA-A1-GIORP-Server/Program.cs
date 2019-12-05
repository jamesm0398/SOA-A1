using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SOA_A1_GIORP_Server
{
    class Program
    {
        char fs = (char)28; //file seperator char
        // State object for reading client data asynchronously  
       

        public static int Main(String[] args)
         {
            String regIpL = "10.192.39.249";
            IPAddress remoteIPAddressL = System.Net.IPAddress.Parse(regIpL);
            IPEndPoint localEndPoint = new IPEndPoint(remoteIPAddressL, 47888);

            try
            {

                // Create a Socket that will use Tcp protocol      
                Socket listener = new Socket(remoteIPAddressL.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method  
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.  
                // We will listen 10 requests at a time  
                listener.Listen(10);

                Console.WriteLine("Waiting for a connection...");
                Socket handler = listener.Accept();

                // Incoming data from the client.    
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("<EOF>") > -1)
                    {
                        break;
                    }
                }

                Console.WriteLine("Text received : {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
            return 0;
            }
        
    }
}
