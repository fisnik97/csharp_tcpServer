using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
   public static class Delegates
   {
      public delegate void MessageReceived(string clientId, string message);
      public delegate void OnClinetDisconnect(string clinetId);
   }
}
