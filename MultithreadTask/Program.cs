namespace MultithreadTask
{
	using MultithreadTask.Configuration;
	using MultithreadTask.Models;
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Threading;

	public class Program
	{
		public static void Main(string[] args)
		{
			var appSettings = new AppSettingsBuilder().Build();
			var dataReciever = new DataReciever();
			var stack = new ConcurrentStack<int>();
			const int numbersCount = 2018;

			using (var coundownEvent = new CountdownEvent(numbersCount))
			{
				Enumerable
					.Range(0, numbersCount)
					.ToList()
					.ForEach(i =>
					{
						var state = new DataRecieverTaskState(i + 1, appSettings.Host, appSettings.Port, stack, coundownEvent);
						ThreadPool.QueueUserWorkItem(dataReciever.PrcessNumber, state);
					});

				coundownEvent.Wait();
			}
			var mediana = CalculateMediana(stack);

			Console.Out.WriteLine($"Mediana: {mediana}");
		}

		private static float CalculateMediana(ConcurrentStack<int> stack)
		{
			var list = stack.ToList();

			list.Sort();
			if (list.Count % 2 == 1)
			{
				return list[list.Count / 2];
			}
			else
			{
				return (float)(list[list.Count / 2 - 1] + list[list.Count / 2]) / 2;
			}
		}
	}
}
