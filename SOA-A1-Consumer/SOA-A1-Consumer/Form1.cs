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
        Socket soa_socket;

        public Form1()
        {
            InitializeComponent();
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
                Object regData = "DRC|REG-TEAM|||\rINF|"+teamName.Text+"|||\r" + Path.DirectorySeparatorChar + "\r" ;
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

        }
    }
}
