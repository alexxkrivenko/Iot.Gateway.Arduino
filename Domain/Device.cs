using System;

namespace Iot.Gateway.Arduino.Domain
{
	public class Device
	{
		#region .ctor
		public Device(Guid deviceId)
		{
			if (deviceId == Guid.Empty)
			{
				throw new ArgumentException("Идентификатор устройства не может быть пустым.", nameof(deviceId));
			}

			DeviceId = deviceId;
		}
		#endregion

		#region Properties
		public Guid DeviceId
		{
			get;
		}

		public Parameter Parameter
		{
			get;
			set;
		}
		#endregion
	}
}
