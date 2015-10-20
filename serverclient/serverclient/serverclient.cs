using System;
using System.Net;
using System.Net.Sockets;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;


using Sample;


namespace Client01
{
    class Client
    {
		private static GraphicsContext graphics;
		private static GamePadButtons command;
		private static Int32 port;
		private static String server;
		private static TcpClient client;
		private static Byte[] data;
		private static NetworkStream stream;
		

        public static void Main()
        {
			Init();
			
			while(true){
			
				SystemEvents.CheckEvents();
				GamepadRender();
				SampleDraw.Update();
				if(command != 0)CommandTransport(command);
				if(command == GamePadButtons.Down) break;
			}
			
			SampleDraw.Term();
			graphics.Dispose();
			Console.ReadLine();
		}
	
		public static void Init(){
			
			port = 9999;
			server = "192.168.0.17";// ここにipアドレスを入れる
			graphics = new GraphicsContext();
			SampleDraw.Init(graphics);
			data = new byte[256];
			client = new TcpClient(server,port);
			stream = client.GetStream();
		}
		
		public static bool GamepadRender(){
			
			var gamePadData = GamePad.GetData(0);
			
			graphics.SetClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear();
			
			int fontHeight = SampleDraw.CurrentFont.Metrics.Height;
			int positionX = (SampleDraw.Width / 5);
			int positionY = ((SampleDraw.Height / 2) - (428 / 2)) + (fontHeight * 3);
			
			foreach (GamePadButtons item in Enum.GetValues(typeof(GamePadButtons))) {
            
				drawPadState(positionX, positionY, item.ToString(), (gamePadData.Buttons & item) != 0);
				positionY += fontHeight;
			}
			
			positionX = (SampleDraw.Width / 5) * 3;
			positionY = ((SampleDraw.Height / 2) - (428 / 2)) + (fontHeight * 3);
			
			SampleDraw.DrawText("AnalogLeftX : " + gamePadData.AnalogLeftX, 0xffffffff, positionX, positionY);
			positionY += fontHeight;
			SampleDraw.DrawText("AnalogLeftY : " + gamePadData.AnalogLeftY, 0xffffffff, positionX, positionY);
			positionY += fontHeight;      
			SampleDraw.DrawText("AnalogRightX : " + gamePadData.AnalogRightX, 0xffffffff, positionX, positionY);      
			positionY += fontHeight;
			SampleDraw.DrawText("AnalogRightY : " + gamePadData.AnalogRightY, 0xffffffff, positionX, positionY);
			SampleDraw.DrawText("GamePad Sample", 0xffffffff, 0, 0);
			graphics.SwapBuffers();
			
			command = gamePadData.Buttons;
			return true;
		}
		
		private static void drawPadState(int positionX, int positionY, string buttonName, bool isButtonDown)
		{
			uint argb;
			string buttonState;

			if (isButtonDown) {
				argb = 0xffff0000;
				buttonState = "ON";
			} else {
				argb = 0xffffffff;
				buttonState = "OFF";
			}

			SampleDraw.DrawText("" + buttonName + " : " + buttonState, argb, positionX, positionY);
		}
		
		private static void CommandTransport(GamePadButtons button){
			
			//client = new TcpClient(server,port);
            data   = BitConverter.GetBytes((uint)button);
			
			// 通信ストリームの取得
			//NetworkStream stream = client.GetStream();
			
			// サーバーへ通信
			stream.Write( data, 0, data.Length );
			//Console.WriteLine( "transmission: {0}Bytes {1}", data.Length, data);
			if(button == GamePadButtons.Down) client.Close();	
		}
		

		
    }// class
}// namespace
