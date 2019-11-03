using System;
using Microsoft.Extensions.Configuration;

namespace Iot.Gateway.Arduino
{
	public class AppConfiguration
	{
		#region Data
		#region Fields
		private readonly IConfiguration _configuration;
		#endregion
		#endregion

		#region .ctor
		public AppConfiguration(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public string NatsServerName
		{
			get => _configuration["NatsSettings:Server"];
		}

		public string NatsServerPort
		{
			get => _configuration["NatsSettings:Port"];
		}

		public string MqttServerName
		{
			get => _configuration["MqttSettings:Server"];
		}

		public string MqttServerPort
		{
			get => _configuration["MqttSettings:Port"];
		}
		public string MqttUser
		{
			get => _configuration["MqttSettings:User"];
		}

		public string MqttPass
		{
			get => _configuration["MqttSettings:Password"];
		}
		#endregion
	}
}
