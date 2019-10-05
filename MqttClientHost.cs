using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iot.Gateway.Arduino.ExtServices;
using Iot.Gateway.Arduino.MessageDispatcher;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using NLog;

namespace Iot.Gateway.Arduino
{
	public class MqttClientHost : IHostedService
	{
		#region Data
		#region Static
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
		#endregion

		#region Fields
		private readonly IDeviceConfigurationService _deviceConfigurationService;
		private readonly IMessageDisaptcher _messageHandler;
		private readonly IMqttClient _mqttClient;
		private readonly IMqttClientOptions _options;
		#endregion
		#endregion

		#region .ctor
		public MqttClientHost(IDeviceConfigurationService deviceConfigurationService, IMessageDisaptcher messageHandler)
		{
			_deviceConfigurationService = deviceConfigurationService ??
										  throw new ArgumentNullException(nameof(deviceConfigurationService));
			_messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
			_mqttClient = new MqttFactory().CreateMqttClient();
			_options = new MqttClientOptionsBuilder()
					   .WithTcpServer("m24.cloudmqtt.com", 11077)
					   .WithCredentials("atkstegm", "oYw2scrYVZD6")
					   .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
					   .WithKeepAliveSendInterval(TimeSpan.FromSeconds(15))
					   .Build();
		}
		#endregion

		#region IHostedService members
		/// <summary>
		/// Triggered when the application host is ready to start the service.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var result = await _mqttClient.ConnectAsync(_options);

			if (result.ResultCode != MqttClientConnectResultCode.Success)
			{
				throw new
					InvalidOperationException("Соединение с брокером Mqtt не установлено. Сервис не может быть запущен.");
			}

			_logger.Info("Соединение с брокером Mqtt установлено.");

			_mqttClient.UseApplicationMessageReceivedHandler(Handler);

			await Subscibe();
		}

		private async Task Handler(MqttApplicationMessageReceivedEventArgs arg)
		{
			await _messageHandler.Dispatch(arg);
		}

		/// <summary>
		/// Triggered when the application host is performing a graceful shutdown.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_mqttClient.DisconnectAsync();
			_mqttClient.Dispose();
			return Task.CompletedTask;
		}
		#endregion

		#region Private
		private async Task Subscibe()
		{
			const string systemPrefix = "IOT";
			var subcribeParameters = _deviceConfigurationService.GetDevices()
																.SelectMany(device =>
																	{
																		return device.Parameters.Select(parameter => $"{systemPrefix}/{device.Id}/{parameter.Id}");
																	});

			foreach (var parameter in subcribeParameters)
			{
				await _mqttClient.SubscribeAsync(parameter);
				_logger.Info($"Выполнена подписка на топик: {parameter}");
			}

		}
		#endregion
	}
}
