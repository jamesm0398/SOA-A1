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


namespace SOA_A1_GIORP
{
    public partial class Form1 : Form
    {
        public Socket giorpSocketListener;
        public Socket giorpSocketWorker;
        public Socket registerSocket;
        public AsyncCallback pfnWorkerCallBack;
        int teamID = 0;

        public Form1()
        {
            InitializeComponent();
        }

        //Form1_Load
        //Summary: Upon loading the form, register team, then create the listening socket and begin listening for messages
        //Parms: sender, e
        //Returns: none
        private void Form1_Load(object sender, EventArgs e)
        {
            
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
                giorpSocketWorker.Close();
                WaitForData(giorpSocketWorker);
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
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
            }
        }

        private void regTeam_Click(object sender, EventArgs e)
        {
            //attempt to connect to the registry IP
            try
            {
                registerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                String ip_addr = regIP.Text;
                String s_port = "3245";

                int port = System.Convert.ToInt16(s_port, 10);
                System.Net.IPAddress remoteIP = System.Net.IPAddress.Parse(ip_addr);
                System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIP, port);
                registerSocket.Connect(remoteEndPoint);
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }

            //attempt to send team name to registry
            try
            {
                Object regData;

             
                regData = "DRC|REG-TEAM|||\rINF|" + teamName.Text + "|||\r" + Path.DirectorySeparatorChar + "\r";
                
                


                byte[] byteData = System.Text.Encoding.ASCII.GetBytes(regData.ToString());
                registerSocket.Send(byteData);
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }

            //received message from registry
            try
            {
                byte[] buffer = new byte[1024];
                int iRx = registerSocket.Receive(buffer);
                char[] chars = new char[iRx];

                Decoder d = Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(buffer, 0, iRx, chars, 0);
                String szData = new System.String(chars);
               
                if(szData.Contains("OK"))
                {

                    string[] dataParts = szData.Split('|');
                    teamID = int.Parse(dataParts[2]);

                    BeginListening();
                    //log message
                }

                else
                {
                    MessageBox.Show("Error: " + szData);
                    //log message
                }
                
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void BeginListening()
        {
            try
            {
                //create the listening socket...
                giorpSocketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 3245);
                //bind to local IP Address...
                giorpSocketListener.Bind(ipLocal);
                //start listening...
                giorpSocketListener.Listen(4);
                // create the call back for any client connections...
                giorpSocketListener.BeginAccept(new AsyncCallback(OnClientConnect), null);
                //  cmdListen.Enabled = false;

            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        private void publish_Click(object sender, EventArgs e)
        {
            try
            {
                //create a new client socket ...
                giorpSocketWorker = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                String szIPSelected = regIP.Text;
                String szPort = "3245";
                int alPort = System.Convert.ToInt16(szPort, 10);

                IPAddress remoteIPAddress = System.Net.IPAddress.Parse(szIPSelected);
                IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, alPort);
                giorpSocketWorker.Connect(remoteEndPoint);
            }
            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }

            try
            {
                Object data;

                data = "\vDRC|PUB-SERVICE|Chaos|" + teamID + "|\r" +
                    "SRV|GIORP-TOTAL|GIORPPurchaseTotalizer|2|2|5|Service to calculate tax amounts for a purchase\r" +
                    "ARG|1|province|string|mandatory||\r" +
                    "ARG|2|purchaseAmount|double|mandatory||\r" +
                    "RSP|1|SubTotal|double||\r" +
                    "RSP|2|PSTAmount|double||\r" +
                    "RSP|3|HSTAmount|double||\r" +
                    "RSP|4|GSTAmount|double||\r" +
                    "RSP|5|TotalAmount|double||\r" +
                    "MCH|" + regIP.Text + "|3245|\r" + Path.DirectorySeparatorChar + "\r";
            }

            catch (System.Net.Sockets.SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
    }
}
