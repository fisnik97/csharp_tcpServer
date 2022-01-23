using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static TcpServer.Delegates;

namespace TcpServer
{
   public class Server
   {
      private TcpListener _tcpListener;
      private readonly List<ConnectedClient> _connectedClients = new();
      private int _connectedClientsNr = 0;
      private readonly Action _onServerStart;

      public Server(string ipAddress, int port, Action onServerStart = null)
      {
         _tcpListener = new(IPAddress.Parse(ipAddress), port);
         _onServerStart = onServerStart;
      }

      public void Listen()
      {
         _tcpListener.Start();
         _onServerStart?.Invoke();
         while (true)
         {
            var connectedClient = _tcpListener.AcceptTcpClient();
            _connectedClientsNr++;
            var client = new ConnectedClient(_connectedClientsNr.ToString(), connectedClient, OnClientConnect);
            _connectedClients.Add(client);
            client.OnMessageReceive += OnMessageReceive;
            client.OnClientDisconnect += OnClinetDisconnect;
            Task.Run(client.CheckForMessages);
         }
      }


      public int GetNumberOfClients => _connectedClientsNr;
      public List<ConnectedClient> GetConnectedClients => _connectedClients;
      public ConnectedClient GetClient(string clinetId)
      {
         return _connectedClients.FirstOrDefault(client => client.ClientId == clinetId);
      }

      private void OnMessageReceive(string clientId, string message)
      {
         Console.ForegroundColor = ConsoleColor.DarkMagenta;
         Console.WriteLine($"{clientId}: '{message}'");
         Console.ForegroundColor = ConsoleColor.DarkRed;
      }

      private void OnClinetDisconnect(string clientId)
      {
         Console.ForegroundColor = ConsoleColor.DarkYellow;
         _connectedClientsNr--;
         var removedClient = _connectedClients.FirstOrDefault(c => c.ClientId == clientId);
         _connectedClients.Remove(removedClient);
         Console.WriteLine($"Clinet: '{clientId}' disconnected from server!");
         Console.ForegroundColor = ConsoleColor.DarkRed;
      }

      private void OnClientConnect(string clinetId)
      {
         Console.ForegroundColor = ConsoleColor.DarkYellow;
         Console.WriteLine($"Clinet {clinetId} connected to server!");
         Console.ForegroundColor = ConsoleColor.DarkRed;
      }

   }
}
