namespace MultithreadTask
{
	using MultithreadTask.Models;
	using System;
	using System.Net.Sockets;
	using System.Text;

	public class DataReciever
	{
		public void PrcessNumber(object stateInfo)
		{
			var state = (DataRecieverTaskState)stateInfo;
			const int maxReadTimeout = 1500;
			try
			{
				var client = new TcpClient();
				client.Connect(state.Host, state.Port);

				using (var stream = client.GetStream())
				{
					stream.ReadTimeout = maxReadTimeout;
					SendMessage(state.Input, stream);

					var result = ReadMessage(stream);

					state.Stack.Push(result);
					state.CountdownEvent.Signal();
				}
				client.Close();

				Console.Out.WriteLine($"processed: {state.Input}");
				return;
			}
			catch (Exception)
			{
				// Connection was lost
				PrcessNumber(stateInfo);
			}
		}

		private void SendMessage(int number, NetworkStream stream)
		{
			var inputString = $"{number}\n";
			var inputBytes = Encoding.UTF8.GetBytes(inputString);
			stream.Write(inputBytes, 0, inputBytes.Length);
		}

		private int ReadMessage(NetworkStream stream)
		{
			var result = 0;
			int parsedValue;
			char readedSymbol;

			do
			{
				var charCode = stream.ReadByte();
				if (charCode == -1)
				{
					// TcpClient dropped the connection
					throw new Exception();
				}
				readedSymbol = (char)charCode;
			}
			while (!int.TryParse(readedSymbol.ToString(), out parsedValue));

			do
			{
				result = result * 10 + parsedValue;

				var charCode = stream.ReadByte();
				if (charCode == -1)
				{
					// TcpClient dropped the connection
					throw new Exception();
				}
				readedSymbol = (char)charCode;
			}
			while (int.TryParse(readedSymbol.ToString(), out parsedValue));

			return result;
		}
	}
}
