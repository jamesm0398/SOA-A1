/*
File		:	AsyncService.cs
Project		:	R_DB assignmet 1 
Programmer	:	John Hall
Date		:	2018/Sept/22

*/
using DBM_Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace RD_A1_Server
{
    ///
    /// \Class AsyncService
    ///
    ///	\brief The purpose of this class is to setup and run a server.
    ///
    /// The purpose of this class is to setup and run a server. <br>
    /// The Data Class has members to keep track of clients, server ip, and server port. 
    /// This class also has the ability to change its data member if needed
    ///
    /// \author <i> John Hall <i>
    public class AsyncService
    {

        struct Clients ///< struct for keeping track of all clients
        {
           int count; ///< number of clients
           public List<Client> cList; ///< list of Client objects connected to server

            /**
            * \brief To add a client to cList on the server
            * \details <b>Details</b>
            * \param tcpClient - <b>TcpClient</b> - Connection information for client
            * creates a new backgroundworker assigns a method to the backgroundworker
            * and adds new client to cList
            *
            * \return void
            * */
            public void AddClient(TcpClient tcpClient)
            {                
                count++;
                BackgroundWorker bW = new BackgroundWorker();
                bW.DoWork += Process;
                cList.Add(new Client(tcpClient, count, bW));
            }
        }

        Clients clients; ///< struct contains all clients connected to server
        private IPAddress iPAddress; ///< ip of server
        private int port; ///< port server is listening on


        /**
        * \brief To instantiate a new AsyncService object
        * \details <b>Details</b>
        *
        * \param port - <b>int</b> - port the server will listen on     
        * Sets all data members, sets DataBase data members, gets host ip
        *
        * \return As this is a <i>constructor</i> for the AsyncService class, nothing is returned
        */
        public AsyncService(int port)
        {
            clients = new Clients();
            clients.cList = new List<Client>();

            DataBase.Feilds = new System.Collections.Generic.List<Data>();
            DataBase.Path = "./data.txt";
            this.port = port;

            //get host ip
            string hostName = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
            this.iPAddress = null;

            for (int i = 0; i < iPHostEntry.AddressList.Length; i++)
            {
                if (iPHostEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    this.iPAddress = iPHostEntry.AddressList[i];
                    break;
                }
            }
            if (this.iPAddress == null)
            {
                throw new Exception("No IPv4 address for server!");
            }
        }

        /**
       * \brief runs the server loop
       * \details <b>Details</b>
       *
       * \param none - <b>void</b> - no param     
       * Starts tcpListener, accepts tcpClient connections, and starts client backgroundworker thread.
       *
       * \return Task
       */
        public async Task Run()
        {
            System.Net.Sockets.TcpListener listener =
                new System.Net.Sockets.TcpListener(this.iPAddress, this.port);
            listener.Start();
            Console.WriteLine("Server is now running.\n");
            
            while (true)
            {
                try
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    clients.AddClient(tcpClient);

                    int currentIndex = clients.cList.Count - 1;
                    //run and handle clients
                    clients.cList[currentIndex].BW.RunWorkerAsync(clients.cList[currentIndex]);
                    clients.cList[currentIndex].BW.RunWorkerCompleted +=
                        BW_RunWorkerCompleted;
                    
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);                    
                }
            }
        }

        /**
        * \brief backgroundworker completed handler
        * \details <b>Details</b>
        *
        * \param sender - <b>object</b> - came from
        * \param e - <b>RunWorkerCompletedEventArgs</b> - event information
        * removes finished client from clients.cList
        *
        * \return void
        */
        private void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            for (int i = 0; i < clients.cList.Count; i++)
            {
                if (!clients.cList[i].TcpClient.Connected)
                {
                    clients.cList.Remove(clients.cList[i]);
                }
            }
        }

        /**
        * \brief handles client via backgroundworker
        * \details <b>Details</b>
        *
        * \param sender - <b>object</b> - came from
        * \param e - <b>RunWorkerCompletedEventArgs</b> - event information. contains Client object
        * Sets up stream writer and binaryFormatter,
        * reads from client input stream, writes response to client 
        *
        * \return void
        */
        private static async void Process(object sender, DoWorkEventArgs e)
        {
            Client client = (Client)e.Argument;
            string clientIP = client.TcpClient.Client.RemoteEndPoint.ToString();
            Console.WriteLine("Received connection request from " + clientIP);
            try
            {

                System.Net.Sockets.NetworkStream networkStream = client.TcpClient.GetStream();             
                StreamWriter writer = new StreamWriter(networkStream);
                writer.AutoFlush = true;
                BinaryFormatter formatter = new BinaryFormatter();

                while (true)
                {               

                    try
                    {
                        //read from stream
                        client.TransPortData = (DBM_Server.Data.TransPortData)formatter.Deserialize(networkStream);

                        string response = InterpretRequest(client.TransPortData);
                        Console.WriteLine("Response: " + response + "\n");
                        await writer.WriteLineAsync(response);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                }
                client.TcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (client.TcpClient.Connected)
                {
                    client.TcpClient.Close();
                }
            }
        }

        /**
       * \brief interprets the clients request and calls the requested method
       * \details <b>Details</b>
       *
       * \param tPortData - <b>Data.TransPortData</b> - data from client     
       * interprets the clients request and calls the requested method
       *
       * \return string: user message
       */
        private static string InterpretRequest(Data.TransPortData tPortData)
        {
            string result = "Request was invalid\n";

            const int WAIT_TIME = 8;

            while (DataBase.isBusy)
            {
                Thread.Sleep(WAIT_TIME);
            }

            DataBase.isBusy = true;

            if (tPortData.method == DataBase.INSERT)
            {
                if (DataBase.InsertRecord(tPortData))
                {   
                    DataBase.WriteAllData();
                    result = "Record was inserted.\n"; 
                }
                else
                {
                    result = "ERROR: Record was NOT inserted.\n";
                }
            }
            else if (tPortData.method == DataBase.UPDATE)
            {
                if (DataBase.UpdateRecord(tPortData))
                {
                    result = "Record was Updateded.\n";
                    DataBase.WriteAllData();
                }
                else
                {
                    result = "ERROR: Record was NOT Updated.\n";
                }
            }
            else if (tPortData.method == DataBase.WRITE_ALL)
            {
                if (DataBase.WriteAllData())
                {
                    result = "ALL DATA WRITEN.\n";
                }
                else
                {
                    result = "ERROR: NOT writen.\n";
                }
            }
            else if (tPortData.method == DataBase.FIND)
            {
                result = "Record with id = "+ tPortData.id + " is : " +
                    DataBase.FindRecord(tPortData.id);
            }
            else
            {
                result = "Could not interpret request from client! :(\n";
            }

            DataBase.isBusy = false;

            return result;
        }

    }
}

