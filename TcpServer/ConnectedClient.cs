using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static TcpServer.Delegates;

namespace TcpServer
{
   public class ConnectedClient
   {
      private readonly TcpClient _tcpClient;
      private readonly string _clientId;
      public string ClientId => _clientId;
      public event MessageReceived OnMessageReceive;
      public event OnClinetDisconnect OnClientDisconnect;

      public ConnectedClient(string clientId, TcpClient client,
         Action<string> onClientConnect = null)
      {
         this._clientId = clientId;
         this._tcpClient = client;
         onClientConnect?.Invoke(this._clientId);
      }

      public void SendMessage(string message)
      {
         if (string.IsNullOrEmpty(message)) return;
         var byteData = Encoding.ASCII.GetBytes(message);
         var clientStream = this._tcpClient.GetStream();
         clientStream.Write(byteData, 0, byteData.Length);
      }

      public void CheckForMessages()
      {
         try
         {
            var bytes = new Byte[256];
            var stream = this._tcpClient.GetStream();
            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
               var data = Encoding.ASCII.GetString(bytes, 0, i);
               this.OnMessageReceive?.Invoke(this._clientId.ToString(), data);
            }
         }
         catch (Exception)
         {
            OnClientDisconnect?.Invoke(this._clientId.ToString());
         }
      }

   }
}
