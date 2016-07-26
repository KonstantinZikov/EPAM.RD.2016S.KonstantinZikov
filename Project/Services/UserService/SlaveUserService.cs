using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoryInterfaces;
using Utils;
using System.Net;
using System.Net.Sockets;
using ServiceInterfaces;
using System.Runtime.Serialization.Json;
using Entities;
using System.Threading;
using System.IO;

namespace Services
{
    public class SlaveUserService : UserService
    {
        private readonly TcpListener _listener;
        private DataContractJsonSerializer jsonFormatter
            = new DataContractJsonSerializer(typeof(User));
        private CancellationToken _token;


        public SlaveUserService(int id, IPEndPoint endPoint, IUserRepository repository, ILogger logger) 
            : base(id, repository, logger)
        {
            _listener = new TcpListener(endPoint);
            _token = new CancellationToken();
            Task a = new Task(AcceptMessages, _token);
            a.Start();
        }


        public void AcceptMessages()
        {
            _listener.Start();
            TcpClient client = _listener.AcceptTcpClient();          
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                List<byte> message = new List<byte>();
                List<byte> separatedMessage = new List<byte>();
                byte[] data = new byte[64];
                while (true)
                {
                    if (_token.IsCancellationRequested)
                    {
                        break;
                    }
                    bool end;
                    do
                    {
                        end = false;
                        var bytes = 0;
                        if (stream.DataAvailable)
                        {
                            bytes = stream.Read(data, 0, data.Length);
                        }                       
                        if (bytes != data.Length)
                        {
                            for (int i = 0; i < bytes; i++)
                            {
                                message.Add(data[i]);
                            }
                        }
                        else
                        {
                            message.AddRange(data);
                        }
                        for (int i = 0; i < message.Count; i++)
                        {
                            if (message[i] == (byte)'<')
                            {
                                if (message.Count > i + 1 && message[i + 1] == (byte)'>')
                                    end = true;
                            }
                        }
                    }
                    while (!(message.Contains((byte)'<') && message.Contains((byte)'>') && end));
                    if (message.Count > 2)
                    {
                        var code = (char)message[0];
                        message.RemoveAt(0);
                        int pos = 0;
                        while (message[pos] != (byte)'<' || message[pos + 1] != (byte)'>')
                        {
                            separatedMessage.Add(message[pos]);
                            pos++;
                        }
                        message.RemoveRange(0, pos + 2);
                        using (var ms = new MemoryStream(separatedMessage.ToArray()))
                        {
                            var user = jsonFormatter.ReadObject(ms) as User;
                            switch (code)
                            {
                                case 'A':
                                    Add(user);
                                    break;
                                case 'D':
                                    Delete(user);
                                    break;
                            }
                        }
                            
                        
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw new UserServiceException("Network error.",ex);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }

        }

    }
}
