namespace MultithreadTask.Models
{
	using System.Collections.Concurrent;
	using System.Threading;

	public class DataRecieverTaskState
	{
		public DataRecieverTaskState(int input, string host, int port, ConcurrentStack<int> stack, CountdownEvent countdownEvent)
		{
			Input = input;
			Host = host;
			Port = port;
			Stack = stack;
			CountdownEvent = countdownEvent;
		}

		public int Input { get; set; }

		public string Host { get; set; }

		public int Port { get; set; }

		public ConcurrentStack<int> Stack { get; set; }

		public CountdownEvent CountdownEvent { get; set; }
	}
}
