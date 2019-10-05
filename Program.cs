using System;
using System.Threading.Tasks;
using AutoMapper;
using Iot.Gateway.Arduino.ExtServices;
using Iot.Gateway.Arduino.MessageDispatcher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NATS.Client;
using NLog;

namespace Iot.Gateway.Arduino
{
	internal class Program
	{
		#region Delegates and events
		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs ex)
		{
			LogManager.GetCurrentClassLogger()
					  .Fatal(Resource.Fatal_ApplicationFatalError, Environment.NewLine, ex.ExceptionObject.ToString(),
							 Environment.NewLine);
			Environment.Exit(1);
		}
		#endregion

		#region Private
		private static async Task Main()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

			var host = new HostBuilder().ConfigureAppConfiguration((context, app) =>
											{
												app.AddJsonFile("appsettings.json", true);
											})
										.ConfigureServices((hostContext, services) =>
											{
												services.AddHostedService<MqttClientHost>();
												var configuration = new AppConfiguration(hostContext.Configuration);
												services.AddSingleton(configuration);
												services.AddTransient<IDeviceConfigurationService, DeviceConfigurationService>();
												services.AddTransient<IMessageDisaptcher, MessageDispatcher.MessageDispatcher>();
												services.AddSingleton<IConnection>(provider => new ConnectionFactory()
																					   .CreateConnection($"{configuration.ServerName}:{configuration.ServerPort}"));
												services.AddAutoMapper();
											})
										.Build();
			await host.RunAsync();
		}
		#endregion
	}
}
