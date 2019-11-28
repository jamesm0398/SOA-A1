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

        public Form1()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
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
                String s_port = "3245";

                int port = System.Convert.ToInt16(s_port, 10);
                System.Net.IPAddress remoteIP = System.Net.IPAddress.Parse(ip_addr);
                System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIP, port);
                soa_socket.Connect(remoteEndPoint);
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }

            //attempt to send team name to registry
            try
            {
                Object regData;

                if(regOrUnreg == true)
                {
                   regData = "\vDRC|REG-TEAM|||\rINF|" + teamName.Text + "|||\r" + Path.DirectorySeparatorChar + "\r";
                }
                else
                {
                   regData = "\vDRC|UNREG-TEAM|"+teamName.Text+"|"+teamID+"|\r" + Path.DirectorySeparatorChar + "\r";
                }

               
                byte[] byteData = System.Text.Encoding.ASCII.GetBytes(regData.ToString());
                soa_socket.Send(byteData);
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }

            //received message from registry
            try
            {
                byte[] buffer = new byte[1024];
                int iRx = soa_socket.Receive(buffer);
                char[] chars = new char[iRx];

                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
                String szData = new System.String(chars);
                responseMsg.Text = szData;
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }

            regOrUnreg = false;
            if(regOrUnreg == false)
            {
                register.Text = "Unregister";
            }

            else
            {
                register.Text = "Register";
            }
           

        }

        //Query_Click
        //Summary: Query the registry to look for the service
        //Params: sender, e
        //Returns: none
        private void Query_Click(object sender, EventArgs e)
        {
            string tag_name = "";

            if(serviceList.SelectedIndex == 0)
            {
                tag_name = "GIORP-TOTAL";
            }

            if (serviceList.SelectedIndex == 1)
            {
                tag_name = "PAYROLL";
            }

            if (serviceList.SelectedIndex == 2)
            {
                tag_name = "CAR-LOAN";
            }

            if (serviceList.SelectedIndex == 3)
            {
                tag_name = "POSTAL";
            }

            else
            {
                MessageBox.Show("Service not selected");
                return;
            }

            try
            {
                Object queryData;
                queryData = "\vDRC|QUERY-SERVICE|" + teamName + "|" + teamID + "|\r" +
                    "SRV|" + tag_name + "||||||" + "\r" + Path.DirectorySeparatorChar + "\r";
                byte[] queryBytes = Encoding.ASCII.GetBytes(queryData.ToString());
                soa_socket.Send(queryBytes);

            }

            catch(SocketException se)
            {
                MessageBox.Show(se.Message);
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
                    numArgs = Convert.ToInt32(queryParts[8]);
                    int argStart = 13;
                    int argDataTypeStart = 14;
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
                    serviceIP = mchParts[0];
                    servicePort = mchParts[1];
                    responseMsg.Text = qszData;

                }

                else
                {
                    responseMsg.Text = qszData;
                }

            }

            catch(SocketException se)
            {
                MessageBox.Show(se.Message);
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
                MessageBox.Show("IP/Port/Number of arguments of service missing");
            }

            else
            {
                try
                {
                    service_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    int servPort = System.Convert.ToInt16(servicePort, 10);
                    System.Net.IPAddress servIP = System.Net.IPAddress.Parse(serviceIP);
                    System.Net.IPEndPoint servEndPoint = new System.Net.IPEndPoint(servIP, servPort);
                    service_socket.Connect(servEndPoint);
                }

                catch(SocketException se)
                {
                    MessageBox.Show(se.Message);
                }

                try
                {
                    Object execData;
                    
                    //build exec message based on how many arguments are needed
                    if(numArgs == 2)
                    {
                        execData = "\vDRC|EXEC-SERVICE|" + teamName + "|" + teamID + "|\r" +
                            "SRV||" + serviceName + "||2|||\r" +
                            "ARG|1" + serviceArgs[0] + "|" + serviceArgDataTypes[0] + "||" + param1.Text + "|\r" +
                            "ARG|2" + serviceArgs[1] + "|" + serviceArgDataTypes[1] + "||" + param2.Text + "|\r" + Path.DirectorySeparatorChar + "\r";

                    }

                   if(numArgs ==3)
                    {
                        execData = "\vDRC|EXEC-SERVICE|" + teamName + "|" + teamID + "|\r" +
                           "SRV||" + serviceName + "||3|||\r" +
                           "ARG|1" + serviceArgs[0] + "|" + serviceArgDataTypes[0] + "||" + param1.Text + "|\r" +
                           "ARG|2" + serviceArgs[1] + "|" + serviceArgDataTypes[1] + "||" + param2.Text + "|\r" +
                           "ARG|3" + serviceArgs[2] + "|" + serviceArgDataTypes[2] + "||" + param3.Text + "|\r" + Path.DirectorySeparatorChar + "\r";
                    }

                    if (numArgs ==4)
                    {
                        execData = "\vDRC|EXEC-SERVICE|" + teamName + "|" + teamID + "|\r" +
                          "SRV||" + serviceName + "||4|||\r" +
                          "ARG|1" + serviceArgs[0] + "|" + serviceArgDataTypes[0] + "||" + param1.Text + "|\r" +
                          "ARG|2" + serviceArgs[1] + "|" + serviceArgDataTypes[1] + "||" + param2.Text + "|\r" +
                          "ARG|3" + serviceArgs[2] + "|" + serviceArgDataTypes[2] + "||" + param3.Text + "|\r" +
                          "ARG|4" + serviceArgs[3] + "|" + serviceArgDataTypes[3] + "||" + param4.Text + "|\r" + Path.DirectorySeparatorChar + "\r";
                    }

                    if (numArgs == 5)
                    {
                        execData = "\vDRC|EXEC-SERVICE|" + teamName + "|" + teamID + "|\r" +
                         "SRV||" + serviceName + "||5|||\r" +
                         "ARG|1" + serviceArgs[0] + "|" + serviceArgDataTypes[0] + "||" + param1.Text + "|\r" +
                         "ARG|2" + serviceArgs[1] + "|" + serviceArgDataTypes[1] + "||" + param2.Text + "|\r" +
                         "ARG|3" + serviceArgs[2] + "|" + serviceArgDataTypes[2] + "||" + param3.Text + "|\r" +
                         "ARG|4" + serviceArgs[3] + "|" + serviceArgDataTypes[3] + "||" + param4.Text + "|\r" +
                         "ARG|5" + serviceArgs[4] + "|" + serviceArgDataTypes[4] + "||" + param5.Text + "|\r" + Path.DirectorySeparatorChar + "\r";
                    }

                }

                catch (SocketException se)
                {
                    MessageBox.Show(se.Message);
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

                }

                catch(SocketException se)
                {
                    MessageBox.Show(se.Message);
                }
            }
        }

        //Close/shutdown all sockets before exiting
        public void OnProcessExit(object sender, EventArgs e)
        {
           
            soa_socket.Shutdown(SocketShutdown.Both);
            soa_socket.Close();
            service_socket.Shutdown(SocketShutdown.Both);
            service_socket.Close();
         
        }
    }
}
