using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

//SOA-A1-GIORP
//Summary: This is the C# purchase totalizer service, it contains sockets for communicating with 
//         other teams' consumers as well as the registry, along with the totalizer logic for calculating tax amounts
//Programmers: James Milne, John Hall
//Date: Dec 5th 2019

namespace SOA_A1_GIORP
{
    public partial class Form1 : Form
    {
        public Socket giorpSocketListener;              //socket for listening for messages from other teams
        public TcpListener giorpListener;
        public Socket giorpSocketWorker;                //socket for sending messages back to the teams
        public TcpClient giorpClient;
        public Socket registerSocket;                   //socket for communicating with registry
        public AsyncCallback pfnWorkerCallBack;
       
        string giorpResponse = "";
        char fs = (char)28; //file seperator char

        public Form1()
        {
            InitializeComponent();
           

        }

        //Logging function
        //Log messages to the log file
        //Param: log: the text to add to the file
        //Returns: none
        public void Logging(string log)
        {
            string filename = @"SOAGIORPServiceLogging.txt";
            DateTime date = DateTime.Now;
            string logtext = $"{log}\t{date.ToString("MM/dd/yyyy HH:mm:ss")}" + Environment.NewLine;

            if (!File.Exists(filename))
            {
                File.Create(filename);
                File.AppendAllText(filename, "--GIORP APP LOG-- \n Team: jTeam (James M, John H");
            }

            File.AppendAllText(filename, logtext);
        }

        //Form1_Load
        //Summary: Upon loading the form, register team, then create the listening socket and begin listening for messages
        //Parms: sender, e
        //Returns: none
        private void Form1_Load(object sender, EventArgs e)
        {
          
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
        }

        //OnClientConnect
        //Summary: Called when a client has connected
        //Params: IAsyncResult asyn
        //Returns: none
        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                giorpSocketWorker = giorpSocketListener.EndAccept(asyn);
                //giorpSocketWorker.Close();
                WaitForData(giorpSocketWorker);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

        }

        //Class: CSocketPacket
        //Summary: Packet of data for the current socket
        public class CSocketPacket
        {
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[1];
        }

        //WaitForData
        //Summary: Sets up the call back and begins receiving data
        //Params: Socket soc: the current socket to be used for retreiving data
        //Returns: none
        public void WaitForData(System.Net.Sockets.Socket soc)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                {
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                }
                CSocketPacket theSocPkt = new CSocketPacket();
                theSocPkt.thisSocket = soc;
                // now start to listen for any data...
                soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

        }

        //OnDataReceived
        //Summary: Data is finished being received, which will be the HL7 message with the arguments for the service
        //Params: IAsyncResult asyn
        //Returns: none
        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                CSocketPacket theSockId = (CSocketPacket)asyn.AsyncState;
                //end receive...
                int iRx = 0;
            
                iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                Decoder d = Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                string szData = new string(chars);

                //parse the execute message, split based on the position of the | character
                string[] szDataParts = szData.Split('|');
                string province = szDataParts[16];
                double purchAmount;
                int errorCode = 0;
                bool isDouble = Double.TryParse(szDataParts[22], out purchAmount);
                bool teamIsValid = false;

                //verify that team can use this service with query-team message
                string queryTeamName = "";
                string queryTeamID = "";

                queryTeamName = szDataParts[2]; //the name of the team asking to use this service
                queryTeamID = szDataParts[3]; //the id of the team asking to use this service

                try
                {
                    Object queryObj = "\vDRC|QUERY-TEAM|"+teamName.Text+ "|" + teamIDText.Text + "|\r" +
                                        "INF|" + queryTeamName + "|" + queryTeamID + "|GIORP-TOTAL|\r" + fs + "\r";
                    byte[] queryBytes = Encoding.ASCII.GetBytes(queryObj.ToString());
                    registerSocket.Send(queryBytes);
                }

                catch (SocketException e)
                {
                    MessageBox.Show(e.Message);
                    Logging(e.Message);
                }

                try
                {
                    byte[] queryResp = new byte[1024];
                    int iRqx = registerSocket.Receive(queryResp);
                    char[] Qchars = new char[iRqx];

                    Decoder Queryd = Encoding.UTF8.GetDecoder();
                    int QcharLen = Queryd.GetChars(queryResp, 0, iRx, chars, 0);
                    String qData = new string(chars);
                    if(!(qData.Contains("NOT")))
                    {
                        teamIsValid = true;
                    }

                    else
                    {
                        errorCode = 4;
                        giorpResponse = "\vPUB|NOT-OK|" + errorCode + "|Your team has insufficient permissions to use this service||\r" + fs + "\r";
                    }
                }

                catch(SocketException e)
                {
                    MessageBox.Show(e.Message);
                }


                if(isDouble && teamIsValid)
                {
                    // purchAmount = double.Parse(szDataParts[22]);
                    giorpResponse = Totalizer(province, purchAmount);
                }
                else
                {
                    errorCode = 3;
                    giorpResponse = "\vPUB|NOT-OK|" + errorCode + "|Purchase amount not a double||\r" + fs + "\r";
                }
                
                //try to send response message
                try
                {
                    Object objResp = giorpResponse;
                    byte[] byResp = Encoding.ASCII.GetBytes(objResp.ToString());
                    giorpSocketWorker.Send(byResp);
                    Logging("Sending response to client");
                    Logging(giorpResponse);
                }
               
                catch(SocketException e)
                {
                    MessageBox.Show(e.Message);
                    Logging(e.Message);
                }


             //   txtDataRx.Text = txtDataRx.Text + szData;
                WaitForData(giorpSocketWorker);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }
        }

        //Regteam_click
        //Summary: This is called when the team has to register the service on the registry
        //Params: sender, e
        //Returns: none
        private void regTeam_Click(object sender, EventArgs e)
        {
                    }


        //BeginListening
        //Summary: Listen for any clients wanting to connect
        //Params: none
        //Returns: none
        public void BeginListening()
        {


            String regIpL = "10.192.39.249";
            IPAddress remoteIPAddressL = System.Net.IPAddress.Parse(regIpL);


            try
            {
                //create the listening socket...
                giorpSocketListener = new Socket(remoteIPAddressL.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(remoteIPAddressL, 47888);
                //bind to local IP Address...
                giorpSocketListener.Bind(ipLocal);
                //start listening...
                giorpSocketListener.Listen(100);
                // create the call back for any client connections...
                giorpSocketListener.BeginAccept(new AsyncCallback(OnClientConnect), giorpSocketListener);
                //  cmdListen.Enabled = false;

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }
        }

        //Publish_click
        //Summary: Called when the service is to be published to the registry
        //Params: sender, e
        //Returns: none
        private void publish_Click(object sender, EventArgs e)
        {
            try
            {





                //create a new client socket ...
                registerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                String szIPSelected = pubIPText.Text;
                String szPort = pubPortText.Text;
                int alPort = System.Convert.ToInt32(szPort, 10);
                String regIp = regIP.Text;
                String regPort = regPortTxt.Text;
                int registryPort = Convert.ToInt32(regPort, 10);
                IPAddress remoteIPAddress = System.Net.IPAddress.Parse(regIp);
                IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, registryPort);
                registerSocket.Connect(remoteEndPoint);
            }
            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }

            try
            {
                Object data;

                data = "\vDRC|PUB-SERVICE|jTeam|" + teamIDText.Text + "|\r" +
                    "SRV|GIORP-TOTAL|GIORPPurchaseTotalizer|2|2|5|Service to calculate tax amounts for a purchase\r" +
                    "ARG|1|province|string|mandatory||\r" +
                    "ARG|2|purchaseAmount|double|mandatory||\r" +
                    "RSP|1|SubTotal|double||\r" +
                    "RSP|2|PSTAmount|double||\r" +
                    "RSP|3|HSTAmount|double||\r" +
                    "RSP|4|GSTAmount|double||\r" +
                    "RSP|5|TotalAmount|double||\r" +
                    "MCH|" + pubIPText.Text + "|47888|\r" + fs + "\r";

                byte[] byteData = Encoding.ASCII.GetBytes(data.ToString());
                registerSocket.Send(byteData);
                registerSocket.Disconnect(false);
                
                Logging("Sending publish message to registry");
                Logging(data.ToString());
               
             //  giorpSocketWorker.Disconnect(true);
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }

            
            //received message from registry
            try
            {

                byte[] buffer = new byte[1024];
                int iRx = registerSocket.Receive(buffer);
                char[] chars = new char[iRx];

                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
                String szData = new System.String(chars);
                regResponse.Text = szData;

                registerSocket.Disconnect(false);


            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
                Logging(se.Message);
            }
        }

        //Totalizer
        //Summary: Called when all of the parameters from an execute message have been received, it will actually do the calculations for tax for each province
        //Params: prov: The province used to determine the tax amount, purchaseAmount: the pre-tax amount
        //Returns: string: the response to send back to the client
        public string Totalizer(string prov, double purchaseAmount)
        {
            bool pass = true;
            string errorMsg = "";
            double totalPrice = 0;
            double hstAdded = 0;
            double pstAdded = 0;
            double gstAdded = 0;
            string respString = "";
            int errorCode = 0;

            if(purchaseAmount < 0)
            {
                errorMsg = "Purchase amount cannot be less than 0";
                errorCode = 2;
            }

            switch(prov)
            {
                case "NL":
                    totalPrice = purchaseAmount + 0.13 * purchaseAmount;
                    hstAdded = totalPrice - purchaseAmount;
                    break;

                case "NS":
                    totalPrice = purchaseAmount + 0.15 * purchaseAmount;
                    hstAdded = totalPrice - purchaseAmount;
                    break;

                case "NB":
                    totalPrice = purchaseAmount + 0.13 * purchaseAmount;
                    hstAdded = totalPrice - purchaseAmount;
                    break;

                case "PE":
                    totalPrice = (purchaseAmount + (0.05 * purchaseAmount));
                    gstAdded = totalPrice - purchaseAmount;
                    totalPrice = totalPrice * 0.10;
                    pstAdded = totalPrice - purchaseAmount;
                    break;

                case "QC":
                    totalPrice = (purchaseAmount + (0.05 * purchaseAmount));
                    gstAdded = totalPrice - purchaseAmount;
                    totalPrice = totalPrice * 0.095;
                    pstAdded = totalPrice - purchaseAmount;
                    break;

                case "ON":
                    totalPrice = purchaseAmount + 0.13 * purchaseAmount;
                    hstAdded = totalPrice - purchaseAmount;
                    break;

                case "MB":
                    totalPrice = purchaseAmount + 0.07 * purchaseAmount;
                    pstAdded = totalPrice - purchaseAmount;
                    totalPrice = purchaseAmount + 0.05 * purchaseAmount;
                    gstAdded = totalPrice - purchaseAmount;
                    totalPrice = purchaseAmount + pstAdded + gstAdded;
                    break;

                case "SK":
                    totalPrice = purchaseAmount + 0.05 * purchaseAmount;
                    pstAdded = totalPrice - purchaseAmount;
                    totalPrice = purchaseAmount + 0.05 * purchaseAmount;
                    gstAdded = totalPrice - purchaseAmount;
                    totalPrice = purchaseAmount + pstAdded + gstAdded;
                    break;

                case "AB":
                    totalPrice = purchaseAmount + 0.05 * purchaseAmount;
                    gstAdded = totalPrice - purchaseAmount;
                    break;

                case "BC":
                    totalPrice = purchaseAmount + 0.12 * purchaseAmount;
                    hstAdded = totalPrice - purchaseAmount;
                    break;

                case "YT":
                    totalPrice = purchaseAmount + 0.05 * purchaseAmount;
                    gstAdded = totalPrice - purchaseAmount;
                    break;

                case "NT":
                    totalPrice = purchaseAmount + 0.05 * purchaseAmount;
                    gstAdded = totalPrice - purchaseAmount;
                    break;

                case "NU":
                    totalPrice = purchaseAmount + 0.05 * purchaseAmount;
                    gstAdded = totalPrice - purchaseAmount;
                    break;

                default:
                    errorMsg = "Not a valid province";
                    errorCode = 1;
                    pass = false;
                    break;
            }

            if(pass)
            {
                respString = "\vPUB|OK|||5|\r" +
                    "RSP|1|subTotal|double|" + purchaseAmount + "|\r" +
                    "RSP|2|pstAmount|double|" + pstAdded + "|\r" +
                    "RSP|3|pstAmount|double|" + hstAdded + "|\r" +
                    "RSP|4|pstAmount|double|" + gstAdded + "|\r" +
                    "RSP|5|pstAmount|double|" + totalPrice + "|\r" + fs + "\r";
            }

            else
            {
                respString = "\vPUB|NOT-OK|" + errorCode + "|" + errorMsg + "||\r" + fs + "\r";

            }
            Logging("Finished processing execute message");
            Logging(respString);

            return respString;
        }

        //Close/shutdown all sockets before exiting
        public void OnProcessExit(object sender, EventArgs e)
        {
           
        }

        private void beginListen_Click(object sender, EventArgs e)
        {
            String regIpL = pubIPText.Text;
            IPAddress remoteIPAddressL = System.Net.IPAddress.Parse(regIpL);


            try
            {
                //create the listening socket...
                giorpSocketListener = new Socket(remoteIPAddressL.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(remoteIPAddressL, Convert.ToInt32(pubPortText.Text));
                //bind to local IP Address...
                giorpSocketListener.Bind(ipLocal);
                //start listening...
                giorpSocketListener.Listen(100);
                // create the call back for any client connections...
                giorpSocketListener.BeginAccept(new AsyncCallback(OnClientConnect), giorpSocketListener);
                //  cmdListen.Enabled = false;

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
    }
}
