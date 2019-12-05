using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;

//SOA_A1_Consumer
//Summary:This program is effectively the client application that 
// will connect to the soa registry and consume services, it also
// handles the registering of our team name

namespace SOA_A1_Consumer
{
    public partial class Form1 : Form
    {
        Socket soa_socket;    //socket to communicate with registry
        Socket service_socket; //socket to communicate with service machine
        bool regOrUnreg = true;  //flag to change the register button to unregister after the user has clicked it, true represents register, false represents unregister
        int teamID = 0;  //TEAM ID assigned after registration
        string serviceTeamName = "";    //the team who's service that the user is calling's team name
        string servicePort = ""; //port of the service machine
        string serviceIP = "";  //ip of the service machine
        string serviceName = ""; //name of the service to be called
        int numArgs = 0;          //number of arguments in the service, retreived from query msg
        string[] serviceArgs = new string[5];   //the actual arguments needed for the service
        string[] serviceArgDataTypes = new string[5];   //the data types of the arguments needed
        char fs = (char)28;

        public Form1()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
        }


        public void Logging(string log)
        {
            string filename = @"SOAConsumerLogging.txt";
            DateTime date = DateTime.Now;
            string logtext = $"{log}\t{date.ToString("MM/dd/yyyy HH:mm:ss")}" + Environment.NewLine;

            if (!File.Exists(filename))
            {
                File.Create(filename);
                File.AppendAllText(filename, "--USER APP LOG-- \n Team: jTeam (James M, John H");
            }

            File.AppendAllText(filename, logtext);
        }
     

        //Register_Click
        //Summary: Register our team by sending the team name
        //Params: sender, e
        //Returns: none
        private void Register_Click(object sender, EventArgs e)
        {
            
            //attempt to connect to the registry IP
            try
            {
                soa_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                String ip_addr = Reg_IP.Text;
                String s_port = portText.Text;

                int port = System.Convert.ToInt32(s_port, 10);
                System.Net.IPAddress remoteIP = System.Net.IPAddress.Parse(ip_addr);
                System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIP, port);
                soa_socket.ReceiveTimeout = 10000;
                soa_socket.Connect(remoteEndPoint);

                if (soa_socket.Connected)
                {
                    responseMsg.Text = "Connected";
                }


               

            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

            //attempt to send team name to registry
            try
                
            {
                Object regData = "";
                
                if(regOrUnreg == true)
                {
                     regData = "\vDRC|REG-TEAM|||\rINF|" + teamName.Text + "|||\r" + fs + "\r";
                }
               
                else
                {
                    regData = "\vDRC|UNREG-TEAM|" + teamName.Text + "|" + teamID + "|\r" + fs + "\r";
                }


              

                byte[] byteData = System.Text.Encoding.ASCII.GetBytes(regData.ToString());
                soa_socket.Send(byteData);
                Logging("Calling SOA-Registry with message:");
                Logging(regData.ToString());


                
                // soa_socket.Close();
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

            ////received message from registry
            try
            {
               
                byte[] buffer = new byte[1024];
                int iRx = soa_socket.Receive(buffer);
                char[] chars = new char[iRx];

                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
                String szData = new System.String(chars);
                responseMsg.Text = szData;

                if (!(szData.Contains("NOT")))
                {
                    string[] respParts = szData.Split('|');
                    teamID = Convert.ToInt32(respParts[2]);
                }

                Logging("Response from SOA-Registry:");
                Logging(szData);
                soa_socket.Shutdown(SocketShutdown.Both);
                soa_socket.Disconnect(false);

            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

           
           

        }

        //Query_Click
        //Summary: Query the registry to look for the service
        //Params: sender, e
        //Returns: none
        private void Query_Click(object sender, EventArgs e)
        {
            string tag_name = "";

            soa_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            String ip_addr = Reg_IP.Text;
            String s_port = portText.Text;

            if(Reg_IP.Text == "" || portText.Text == "")
            {
                MessageBox.Show("Please enter the IP and/or port of the registry");
                return;
            }

            int port = System.Convert.ToInt32(s_port, 10);
            System.Net.IPAddress remoteIP = System.Net.IPAddress.Parse(ip_addr);
            System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIP, port);
            soa_socket.ReceiveTimeout = 10000;
            soa_socket.Connect(remoteEndPoint);

            if (serviceList.SelectedItem.ToString() == "GIORP-5000 Purchase Totalizer")
            {
                tag_name = "GIORP-TOTAL";
            }

           else if (serviceList.SelectedItem.ToString() == "Pay Stub Generator")
            {
                tag_name = "PAYROLL";
            }

           else if (serviceList.SelectedItem.ToString() == "Car Loan Calculator")
            {
                tag_name = "CAR-LOAN";
            }

            else if (serviceList.SelectedItem.ToString() == "Canadian Postal Code Validator")
            {
                tag_name = "POSTAL";
            }

            else
            {
                MessageBox.Show("Service not selected");
                return;
            }

            if(teamName.Text == "" || teamID == 0)
            {
                MessageBox.Show(" Your team is not registered");
                return;
            }


            try
            {
                Object queryData;
                queryData = "\vDRC|QUERY-SERVICE|" + teamName.Text + "|" + teamID + "|\r" +
                    "SRV|" + tag_name + "||||||" + "\r" + fs + "\r";
                byte[] queryBytes = Encoding.ASCII.GetBytes(queryData.ToString());
                soa_socket.Send(queryBytes);
                Logging("Query message sent to registry: ");
                Logging(queryData.ToString());
                

            }

            catch(SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

            try
            {
                byte[] queryResp = new byte[1024];
                int qR = soa_socket.Receive(queryResp);
                char[] queryChars = new char[qR];
                Decoder queryD = Encoding.UTF8.GetDecoder();
                int qLen = queryD.GetChars(queryResp, 0, qR, queryChars, 0);
                string qszData = new string(queryChars);

                if(!(qszData.Contains("NOT")))
                {
                    string[] queryParts = qszData.Split('|');
                    serviceTeamName = queryParts[6];
                    serviceName = queryParts[7];
                    numArgs = Convert.ToInt32(queryParts[9]);
                    int argStart = 14;
                    int argDataTypeStart = 15;
                    //get argument name and their data type for as many arguments are returned
                    for(int i = 0; i<numArgs;i++)
                    {
                        serviceArgs[i] = queryParts[argStart];
                        serviceArgDataTypes[i] = queryParts[argDataTypeStart];
                        argStart = argStart + 6;
                        argDataTypeStart = argDataTypeStart + 6;
                    }
                    
                    
                    string[] getMCH = qszData.Split(new[] { "MCH" }, StringSplitOptions.None);
                    string[] mchParts = getMCH[1].Split('|');
                    serviceIP = mchParts[1];
                    servicePort = mchParts[2];
                    responseMsg.Text = qszData;
                    Logging("Message received from registry: ");
                    Logging(qszData);
                    soa_socket.Disconnect(false);

                }

                else
                {
                    responseMsg.Text = qszData;
                }

            }

            catch(SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Execute_click
        //Summary: Called when the user is actually ready to execute their chosen service
        //Params: sender, e
        //Returns: none
        private void execute_Click(object sender, EventArgs e)
        {
           
            if (serviceIP == "" || servicePort == "" || numArgs == 0)
            {
                //MessageBox.Show("IP/Port/Number of arguments of service missing");
            }

            else
            {
                try
                {
                    service_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    int servPort = System.Convert.ToInt32(servicePort, 10);
                    System.Net.IPAddress servIP = System.Net.IPAddress.Parse(serviceIP);
                    System.Net.IPEndPoint servEndPoint = new System.Net.IPEndPoint(servIP, servPort);
                    service_socket.ReceiveTimeout = 4000;
                    service_socket.Connect(servEndPoint);
                }

                catch(SocketException se)
                {
                    MessageBox.Show(se.Message);
                    Logging(se.Message);
                }

                try
                {
                    Object execData = "";
                    
                    //build exec message based on how many arguments are needed
                    if(numArgs == 2)
                    {
                        execData = "\vDRC|EXEC-SERVICE|" + teamName.Text + "|" + teamID + "|\r" +
                            "SRV||" + serviceName + "||2|||\r" +
                            "ARG|1|" + serviceArgs[0] + "|" + serviceArgDataTypes[0] + "||" + param1.Text + "|\r" +
                            "ARG|2|" + serviceArgs[1] + "|" + serviceArgDataTypes[1] + "||" + param2.Text + "|\r" + fs + "\r";

                    }

                   if(numArgs ==3)
                    {
                        execData = "\vDRC|EXEC-SERVICE|" + teamName.Text + "|" + teamID + "|\r" +
                           "SRV||" + serviceName + "||3|||\r" +
                           "ARG|1|" + serviceArgs[0] + "|" + serviceArgDataTypes[0] + "||" + param1.Text + "|\r" +
                           "ARG|2|" + serviceArgs[1] + "|" + serviceArgDataTypes[1] + "||" + param2.Text + "|\r" +
                           "ARG|3|" + serviceArgs[2] + "|" + serviceArgDataTypes[2] + "||" + param3.Text + "|\r" + fs + "\r";
                    }

                    if (numArgs ==4)
                    {
                        execData = "\vDRC|EXEC-SERVICE|" + teamName.Text + "|" + teamID + "|\r" +
                          "SRV||" + serviceName + "||4|||\r" +
                          "ARG|1|" + serviceArgs[0] + "|" + serviceArgDataTypes[0] + "||" + param1.Text + "|\r" +
                          "ARG|2|" + serviceArgs[1] + "|" + serviceArgDataTypes[1] + "||" + param2.Text + "|\r" +
                          "ARG|3|" + serviceArgs[2] + "|" + serviceArgDataTypes[2] + "||" + param3.Text + "|\r" +
                          "ARG|4|" + serviceArgs[3] + "|" + serviceArgDataTypes[3] + "||" + param4.Text + "|\r" + fs + "\r";
                    }

                    if (numArgs == 5)
                    {
                        execData = "\vDRC|EXEC-SERVICE|" + teamName.Text + "|" + teamID + "|\r" +
                         "SRV||" + serviceName + "||5|||\r" +
                         "ARG|1|" + serviceArgs[0] + "|" + serviceArgDataTypes[0] + "||" + param1.Text + "|\r" +
                         "ARG|2|" + serviceArgs[1] + "|" + serviceArgDataTypes[1] + "||" + param2.Text + "|\r" +
                         "ARG|3|" + serviceArgs[2] + "|" + serviceArgDataTypes[2] + "||" + param3.Text + "|\r" +
                         "ARG|4|" + serviceArgs[3] + "|" + serviceArgDataTypes[3] + "||" + param4.Text + "|\r" +
                         "ARG|5|" + serviceArgs[4] + "|" + serviceArgDataTypes[4] + "||" + param5.Text + "|\r" + fs + "\r";
                    }
                    byte[] byteData = System.Text.Encoding.ASCII.GetBytes(execData.ToString());
                    service_socket.Send(byteData);
                    Logging("Message sent to " + serviceIP);
                    Logging(execData.ToString());
                   
                }

                catch (SocketException se)
                {
                    MessageBox.Show(se.Message);
                    Logging(se.Message);
                }

                try
                {
                    byte[] execResp = new byte[1024];
                    int eqx = service_socket.Receive(execResp);
                    char[] echars = new char[eqx];

                    Decoder QueryE = Encoding.UTF8.GetDecoder();
                    int EcharLen = QueryE.GetChars(execResp, 0, eqx, echars, 0);
                    String exeRespData = new string(echars);

                    if(!(exeRespData.Contains("NOT")))
                    {
                        responseMsg.Text = exeRespData;
                   

                    }
                    responseMsg.Text = exeRespData;
                    Logging("Message received from published service: ");
                    Logging(exeRespData);
                    service_socket.Disconnect(false);

                }

                catch(SocketException se)
                {
                    MessageBox.Show(se.Message);
                    Logging(se.Message);
                }
            }
        }

        //Close/shutdown all sockets before exiting
        public void OnProcessExit(object sender, EventArgs e)
        {
           
           
         
        }

        private void unreg_Click(object sender, EventArgs e)
        {
            //attempt to connect to the registry IP
            try
            {
                soa_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                String ip_addr = Reg_IP.Text;
                String s_port = portText.Text;

                int port = System.Convert.ToInt32(s_port, 10);
                System.Net.IPAddress remoteIP = System.Net.IPAddress.Parse(ip_addr);
                System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIP, port);
                soa_socket.ReceiveTimeout = 10000;
                soa_socket.Connect(remoteEndPoint);

                if (soa_socket.Connected)
                {
                    responseMsg.Text = "Connected";
                }




            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

            //attempt to send team name to registry
            try

            {
                Object regData = "\vDRC|UNREG-TEAM|" + teamName.Text + "|" + teamID + "|\r" + fs + "\r";

              
                byte[] byteData = System.Text.Encoding.ASCII.GetBytes(regData.ToString());
                soa_socket.Send(byteData);
                Logging("Calling SOA-Registry with message:");
                Logging(regData.ToString());



                // soa_socket.Close();
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

            ////received message from registry
            try
            {
               
                byte[] buffer = new byte[1024];
                int iRx = soa_socket.Receive(buffer);
                char[] chars = new char[iRx];

                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
                String szData = new System.String(chars);
                responseMsg.Text = szData;

             

                Logging("Response from SOA-Registry:");
                Logging(szData);
                soa_socket.Shutdown(SocketShutdown.Both);
                soa_socket.Disconnect(false);

            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }
        }
    }
}
