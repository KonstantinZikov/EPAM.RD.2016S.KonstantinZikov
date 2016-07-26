using Entities;
using System;
using System.Collections.Generic;
using RepositoryInterfaces;
using Utils;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using ServiceInterfaces;
using System.Text;

namespace Services
{
    public class MasterUserService : UserService, IUserService
    {
        private readonly List<IPEndPoint> _endPoints;
        private readonly List<IPEndPoint> _masterPoints;
        private DataContractJsonSerializer jsonFormatter 
            = new DataContractJsonSerializer(typeof(User));
        private List<TcpClient> tcpClients = new List<TcpClient>();
        private bool _conected;
        public MasterUserService(int id, List<IPEndPoint> slavePoints, List<IPEndPoint> masterPoints, IUserRepository repository, ILogger logger) 
            : base(id, repository, logger)
        {
            _endPoints = slavePoints;
            _masterPoints = masterPoints;
        }

        public new int Add(User user)
        {
            int result = base.Add(user);
            SendMessage((byte)'A', user);
            return result;
        }

        public new void Delete(User user)
        {
            base.Delete(user);
            SendMessage((byte)'D', user);
        }

        private void SendMessage(byte code, User user)
        {
            if (!_conected)
            {
                foreach (var point in _endPoints)
                {
                    // Create local point.
                    TcpClient client = new TcpClient(_masterPoints[0]);
                    // Connect to end point
                    client.Connect(point);
                    tcpClients.Add(client);
                    _masterPoints.RemoveAt(0);
                }
                _conected = true;
            }
            
            foreach (var client in tcpClients)
            {
                _logger.Log(System.Diagnostics.TraceEventType.Information, 
                    $"Service {Id} send user to point {client.Client.RemoteEndPoint} with code {(char)code}.");
                var stream = client.GetStream();
                // Send operation code        
                stream.WriteByte(code);
                // Send serialized user
                jsonFormatter.WriteObject(stream, user);
                stream.WriteByte((byte)'<');
                stream.WriteByte((byte)'>');
                
            }
            
        }

        ~MasterUserService()
        {
            foreach(var client in tcpClients)
            {
                try
                {
                    client?.GetStream()?.Close();
                    client?.Close();
                }
                catch { }
            }
        }
    }
}
