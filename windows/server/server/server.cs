using System;
using System.Net;
using System.Net.Sockets;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;


namespace Server01
{
    class Server
    {
		private static buttons b;
		private static NetworkStream stream;
		private static Int32 port;
		private static IPAddress localAddr;
		private static TcpListener server;
		private static TcpClient client;
		private static byte[] buf;
		
		
        public static void Main()
        {
			Init();
			
			while(true)
			{
				Update();
				
				if(b == buttons.Down) break;
			}

			stream.Close();
		    client.Close();
			Console.WriteLine("end");
        }// Mian
		
		public static void Init(){
			
			buf = new byte[256];
			port = 9999;
			
			localAddr = IPAddress.Parse( "00000000" );// 受けつるip
			
			// 初期化 アドレス ポート 今はポートさえ合っていればどのipでも受け付けてる
			server = new TcpListener( IPAddress.Any, port );
			// 接続待機
			client = new TcpClient();
		}
		
		public static void Update(){
			
			clientconnect();
			int len = stream.Read(buf, 0, buf.Length);
			uint u = BitConverter.ToUInt32(buf,0);
			
			b = (buttons)u;
			
			Console.WriteLine(b);
		}
		
		public static void clientconnect(){
			
			server.Start();
			client = server.AcceptTcpClient();
			server.Stop();
			stream = client.GetStream();
		}
		
		public enum buttons : uint{
			
			Left = 1u,
			Up = 2u,
			Right = 4u,
			Down = 8u,
			Square = 16u,
			Triangle = 32u,
			Circle = 65400u,
			Cross = 131200u,
			Start = 256u,
			Select = 512u,
			L = 1024u,
			R = 2048u,
		}
		
		
    }// class
}// namespace