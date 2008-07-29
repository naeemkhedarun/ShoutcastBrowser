using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace PortProberLib
{
    public class PortProber
    {
        private readonly TcpClient _client = new TcpClient();
        private readonly IPEndPoint ipEndPoint;

        public PortProber(IPAddress address, int port)
        {
            Address = address;
            Port = port;
            ipEndPoint = new IPEndPoint(Address, Port);
        }

        public PortProber(IPEndPoint ipEndPoint)
        {
            this.ipEndPoint = ipEndPoint;
        }

        public IPAddress Address { get; set; }
        public int Port { get; set; }


        public bool ProbeMachine()
        {
            bool probeSuccessfull = false;

            Timer timer = new Timer(20000);
            timer.Elapsed += Timer_OnElapsed;

            try
            {
                timer.Start();

                _client.Connect(ipEndPoint);

                probeSuccessfull = true;
            }
            catch (ArgumentNullException e)
            {
                Console.Out.WriteLine("e = {0}", e);
            }
            catch (SocketException e)
            {
                Console.Out.WriteLine("e = {0}", e);
            }
            catch (ObjectDisposedException e)
            {
                Console.Out.WriteLine("e = {0}", e);
            }

            return probeSuccessfull;
        }

        private void Timer_OnElapsed(object sender, ElapsedEventArgs e)
        {
            _client.Close();
        }
    }
}