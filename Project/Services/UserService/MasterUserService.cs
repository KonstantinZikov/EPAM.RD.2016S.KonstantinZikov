using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using Entities;
using RepositoryInterfaces;
using ServiceInterfaces;
using Utils;

namespace Services
{

    /// <summary>
    /// UserService which can send messages to another services with TCP connection
    /// </summary>
    public class MasterUserService : UserService, IUserService
    {
        private readonly object _lockObj = new object();
        private readonly List<IPEndPoint> _endPoints;
        private readonly List<IPEndPoint> _masterPoints;
        private DataContractJsonSerializer _jsonFormatter 
            = new DataContractJsonSerializer(typeof(Message));

        private List<TcpClient> _tcpClients = new List<TcpClient>();

        private bool _conected;

        public MasterUserService(int id, List<IPEndPoint> slavePoints, List<IPEndPoint> masterPoints, IUserRepository repository, ILogger logger) 
            : base(id, repository, logger)
        {
            this._endPoints = slavePoints;
            this._masterPoints = masterPoints;
        }

        ~MasterUserService()
        {
            foreach (var client in this._tcpClients)
            {
                try
                {
                    client?.GetStream()?.Close();
                    client?.Close();
                }
                catch
                {
                }
            }
        }

        public new int Add(User user)
        {
            int result = base.Add(user);
            lock (this._lockObj)
            {
                this.SendMessage(MessageCode.Add, user);
            }  
                     
            return result;
        }

        public new void Delete(User user)
        {
            base.Delete(user);
            lock (this._lockObj)
            {
                this.SendMessage(MessageCode.Delete, user);
            }
        }

        private void SendMessage(MessageCode code, User user)
        {
            if (!this._conected)
            {
                foreach (var point in this._endPoints)
                {
                    // Create local point.
                    TcpClient client = new TcpClient(this._masterPoints[0]);

                    // Connect to end point
                    client.Connect(point);
                    this._tcpClients.Add(client);
                    this._masterPoints.RemoveAt(0);
                }

                this._conected = true;
            }
            
            foreach (var client in this._tcpClients)
            {
                this._logger.Log(
                    System.Diagnostics.TraceEventType.Information, 
                    $"Service {Id} send user to point {client.Client.RemoteEndPoint} with code {code}.");
                var stream = client.GetStream();
                Message mes = new Message() { Code = code, User = user };
                this._jsonFormatter.WriteObject(stream, mes);
                stream.WriteByte((byte)'>');
                stream.WriteByte((byte)'<');
            }           
        }       
    }
}
