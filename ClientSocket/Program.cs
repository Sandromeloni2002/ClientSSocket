using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipadd;
            int port;
            string ipaddstr, portstr;

            do
            {
                Console.WriteLine(" ClientSocket");
                Console.WriteLine();

                do
                {
                    Console.Write("IP: ");
                    ipaddstr = Console.ReadLine();
                    if (ipaddstr == "localhost") ipaddstr = "127.0.0.1";

                } while (!IPAddress.TryParse(ipaddstr.Trim(), out ipadd));

                Console.WriteLine();

                do
                {
                    do
                    {
                        Console.Write("Port: ");
                        portstr = Console.ReadLine();

                    } while (!int.TryParse(portstr, out port));
                } while (port < 0 || port > 65535);

                Console.WriteLine();
                Console.WriteLine("Endpoint: " + ipaddstr + ":" + port);

                try { socket.Connect(ipadd, port); }
                catch
                {
                    ipadd = null;
                    Console.Clear();
                }
            } while (ipadd == null);

            Console.WriteLine("Connessione riuscita!");
            Console.WriteLine();

            Console.Clear();

            try
            {
                while (true)
                {
                    sendtxt(socket);
                    receivetxt(socket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                if (socket != null)
                {
                    if (socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    socket.Close();
                    socket.Dispose();
                }
            }

            Console.WriteLine("Premi Invio per chiudere...");
            Console.ReadLine();
        }

        static void receivetxt(Socket socket)
        {
            string message;
            byte[] buff = null;

            socket.Receive(buff);
            message = Encoding.ASCII.GetString(buff);
            Console.WriteLine(message);
        }

        static void sendtxt(Socket socket)
        {
            string message;
            byte[] buff = null;

            message = Console.ReadLine();
            message = message.ToUpper().Trim();

            buff = Encoding.ASCII.GetBytes(message);
            socket.Send(buff);
        }
    }
}
