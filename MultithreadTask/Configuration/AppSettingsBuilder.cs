namespace MultithreadTask.Configuration
{
	using Microsoft.Extensions.Configuration;
	using MultithreadTask.Appsettings;

	public class AppSettingsBuilder
	{
		public AppSettings Build()
		{
			var appSettings = new AppSettings();
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			var configuration = builder.Build();
			ConfigurationBinder.Bind(configuration.GetSection("AppSettings"), appSettings);

			return appSettings;
		}
	}
}
