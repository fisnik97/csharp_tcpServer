using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpServer
{
   class Program
   {
      static readonly Server server = new Server("127.0.0.1", 81, OnServerStart);

      static void Main(string[] args)
      {
         Task.Run(server.Listen);
         Console.WriteLine("Commands: 1 - Reply, 2 - Exit");

         while (true)
         {
            Console.WriteLine("...");
            var command = Console.ReadLine();
            switch (command)
            {
               case "1":
                  ReplyToClient();
                  break;
               default:
                  break;
            }
         }         
      }

      public static void ReplyToClient()
      {
         Console.Write("Which client you want to reply: ");
         var cId = int.Parse(Console.ReadLine());
         var client = server.GetClient(cId.ToString());
         if (client == null)
         {
            Console.WriteLine("There is no client with this ID!");
            return;
         }

         Console.WriteLine("Write the message to the client: ");
         var message = Console.ReadLine();
         client.SendMessage(message);
      }

      public static void OnServerStart()
      {
         Console.WriteLine("Server has started listening ...");
      }
   }
}
