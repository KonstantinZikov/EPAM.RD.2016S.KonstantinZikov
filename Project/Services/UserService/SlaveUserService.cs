using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using RepositoryInterfaces;
using ServiceInterfaces;
using Utils;

namespace Services
{
    /// <summary>
    /// UserService which can accept messages from external source with TCP connection.
    /// </summary>
    public class SlaveUserService : UserService
    {
        private readonly TcpListener _listener;

        private DataContractJsonSerializer _jsonFormatter
            = new DataContractJsonSerializer(typeof(Message));

        private CancellationToken _token;

        public SlaveUserService(int id, IPEndPoint endPoint, IUserRepository repository, ILogger logger) 
            : base(id, repository, logger)
        {
            this._listener = new TcpListener(endPoint);
            this._token = new CancellationToken();
            Task a = new Task(this.AcceptMessages, this._token);
            a.Start();
        }

        private void AcceptMessages()
        {
            this._listener.Start();
            TcpClient client = this._listener.AcceptTcpClient();          
            NetworkStream stream = null;
            try
            {
                stream = client.GetStream();
                List<byte> message = new List<byte>();
                List<byte> separatedMessage = new List<byte>();
                byte[] data = new byte[128];
                while (true)
                {
                    if (this._token.IsCancellationRequested)
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
                            if (message[i] == (byte)'>')
                            {
                                if (message.Count > i + 1 && message[i + 1] == (byte)'<')
                                {
                                    end = true;
                                }                                    
                            }
                        }
                    }
                    while (!(message.Contains((byte)'>') && message.Contains((byte)'<') && end));
                    if (message.Count > 2)
                    {
                        int pos = 0;
                        while (message[pos] != (byte)'>' || message[pos + 1] != (byte)'<')
                        {
                            separatedMessage.Add(message[pos]);
                            pos++;
                        }

                        message.RemoveRange(0, pos + 2);
                        using (var ms = new MemoryStream(separatedMessage.ToArray()))
                        {
                            var mes = this._jsonFormatter.ReadObject(ms) as Message;
                            switch (mes.Code)
                            {
                                case MessageCode.Add:
                                    this.Add(mes.User);
                                    break;
                                case MessageCode.Delete:
                                    this.Delete(mes.User);
                                    break;
                            }
                        }

                        separatedMessage.Clear();                                  
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw new UserServiceException("Network error.", ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                    
                if (client != null)
                {
                    client.Close();
                }                  
            }
        }
    }
}
