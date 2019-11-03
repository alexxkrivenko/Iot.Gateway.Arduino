using System;
using System.Linq;
using System.Threading.Tasks;
using Iot.Common.Parameter;
using Iot.Events;
using Iot.Gateway.Arduino.ExtServices;
using MessagePack;
using MQTTnet;
using NATS.Client;
using NLog;

namespace Iot.Gateway.Arduino.MessageDispatcher
{
	public class MessageDispatcher : IMessageDisaptcher
	{
		#region Data
		#region Fields
		private readonly IConnection _connection;
		private readonly IDeviceConfigurationService _deviceConfigService;
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();
		#endregion
		#endregion

		#region .ctor
		public MessageDispatcher(IConnection connection, IDeviceConfigurationService deviceConfigService)
		{
			_connection = connection ?? throw new ArgumentNullException(nameof(connection));
			//TODO нужен кэш конфигураций и подписка на события для его актуализации, неправильно долбить сервис по сети на каждый запрос
			_deviceConfigService = deviceConfigService ?? throw new ArgumentNullException(nameof(deviceConfigService));
		}
		#endregion

		#region IMessageDisaptcher members
		public async Task Dispatch(MqttApplicationMessageReceivedEventArgs arg)
		{
			_logger.Info("Сообщение от устройства получено.");

			if (TryParseEvent(arg, out var @event))
			{
				Publish(@event);
				_logger.Info($"Сообщение {@event.EventDateTime}:{@event.EventId} опубликовано.");
				return;
			}

			_logger.Warn("Входящее сообщение от устройства некорректно.");
		}
		#endregion

		#region Private
		private void Publish(object @event)
		{
			_connection.Publish(@event.GetType()
									  .Name, SerializeMessage(@event));
		}

		private static byte[] SerializeMessage(object message)
		{
			return MessagePackSerializer.Typeless.Serialize(message);
		}

		private bool TryParseEvent(MqttApplicationMessageReceivedEventArgs arg, out ArduinoInputEvent @event)
		{
			@event = null;

			var splitted = arg.ApplicationMessage.Topic.Split('/');
			if (!Guid.TryParse(splitted[1], out var deviceId))
			{
				_logger.Warn("Идентификатор устройства во входящем сообщения некорректен.");
				return false;
			}

			var devices = _deviceConfigService.GetDevices();

			if (devices.All(d => d.Id != deviceId))
			{
				_logger.Warn($"Устройство {deviceId} отсутствует среди сконфигурированных.");
				return false;
			}

			if (!Guid.TryParse(splitted[2], out var paramId))
			{
				_logger.Warn("Идентификатор параметра во входящем сообщения некорректен.");
				return false;
			}

			var parameter = devices.SelectMany(d => d.Parameters)
								   .SingleOrDefault(p => p.Id == paramId);

			if (parameter == null)
			{
				_logger.Warn($"Параметр {paramId} не сконфигурирован для устройства {deviceId}.");
				return false;
			}

			var value = arg.ApplicationMessage.ConvertPayloadToString();
			@event = new ArduinoInputEvent
			{
				DeviceId = deviceId,
				Parameter = new ArduinoParameterDto
				{
					ParameterId = paramId
				}
			};

			switch (parameter)
			{
				case AnalogParameterDto _:
					if (float.TryParse(value, out var floatResult))
					{
						@event.Parameter.Value = floatResult;
						return true;
					}
					_logger.Warn($"Аналоговый параметр {paramId} содержит некорректное значение {value}.");
					break;
				case DiscreteParameterDto _:
					if (bool.TryParse(value, out var boolResult))
					{
						@event.Parameter.Value = boolResult;
						return true;
					}
					_logger.Warn($"Дискретный параметр {paramId} содержит некорректное значение {value}.");
					break;
			}

			@event = null;
			return false;
		}
		#endregion
	}
}
