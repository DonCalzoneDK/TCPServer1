using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer1
{
    class Program
    {
        private static TcpListener _welcomingServerSocket;
        private static TcpClient _serverSocket;
        private static Stream _nStream;
        private static StreamWriter _sWriter;
        private static StreamReader _sReader;



        static void Main(string[] args)
        {

            try
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                _welcomingServerSocket = new TcpListener(ip, 6789);
                _welcomingServerSocket.Start();
                Console.WriteLine("Server is waiting the call request of client");
                using (_serverSocket = _welcomingServerSocket.AcceptTcpClient())
                {
                    //Client IP Address
                    var clientIP = ((IPEndPoint) _serverSocket.Client.RemoteEndPoint).Address;
                    //Client Port Number
                    var clientPort = ((IPEndPoint) _serverSocket.Client.RemoteEndPoint).Port;

                    Console.WriteLine("Client has the IP: " + clientIP + "and a Port Number: " + clientPort);
                    Console.WriteLine("The Handshake has now been made");
                    Console.WriteLine("Client and Server are now able to communicate over the network!");

                    using (_nStream = _serverSocket.GetStream())
                    {
                        _sWriter = new StreamWriter(_nStream) {AutoFlush = true};
                        _sReader = new StreamReader(_nStream);
                        var msgFromClient = _sReader.ReadLine();
                        Console.WriteLine("Client Message: " + msgFromClient);
                        var respondingServerMessage = Console.ReadLine();
                        _sWriter.WriteLine(respondingServerMessage);
                        while (respondingServerMessage != "End Chat")
                        {
                            msgFromClient = _sReader.ReadLine();
                            Console.WriteLine("Client Message" + " " + msgFromClient);
                            respondingServerMessage = Console.ReadLine();
                            _sWriter.WriteLine(respondingServerMessage);
                            ;
                        }

                    }
                }

                _welcomingServerSocket.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
        
    }
}
