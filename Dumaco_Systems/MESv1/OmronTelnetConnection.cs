using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MESv1
{
    public class OmronTelnetConnection
    {
        private const string _ipAddress = "10.38.4.12";
        private const int _port = 7171;
        private const string _password = "adept";
        private TcpClient _client;

        public void StartConnection()
        {
            try
            {
                _client = new TcpClient(_ipAddress, _port);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CloseConnection()
        {
            _client.Close();
        }

        private void SendCommand(string command)
        {
            /*try
            {
                NetworkStream stream = _client.GetStream();
                byte[] data = Encoding.ASCII.GetBytes(command);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception)
            {

                throw;
            }*/
        }
    }
}
