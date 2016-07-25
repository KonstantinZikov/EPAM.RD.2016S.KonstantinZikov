using Entities;
using System;
using System.Collections.Generic;
using RepositoryInterfaces;
using Utils;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;

namespace Services
{
    public class MasterUserService : UserService
    {
        private readonly List<IPEndPoint> _endPoints;
        private DataContractJsonSerializer jsonFormatter 
            = new DataContractJsonSerializer(typeof(User));
        private List<NetworkStream> tcpStreams = new List<NetworkStream>();


        public MasterUserService(int id, List<IPEndPoint> endPoints, IUserRepository repository, ILogger logger) 
            : base(id, repository, logger)
        {
            _endPoints = endPoints;
            foreach (var point in _endPoints)
            {
                // Create local point.
                TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 51111));
                // Connect to end point
                client.Connect(point);

                tcpStreams.Add(client.GetStream());               
            }
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
            foreach(var stream in tcpStreams)
            {
                // Send operation code        
                stream.WriteByte(code);

                // Send serialized user
                jsonFormatter.WriteObject(stream, user);
            }
            
        }
    }
}
