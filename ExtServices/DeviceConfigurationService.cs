using System;
using System.Collections.Generic;
using Iot.Common;
using Iot.Common.Parameter;

namespace Iot.Gateway.Arduino.ExtServices
{
	/// <summary>
	/// Представляет DeviceConfigurationService
	/// </summary>
	/// <seealso cref="Iot.Gateway.Arduino.ExtServices.IDeviceConfigurationService" />
	public class DeviceConfigurationService : IDeviceConfigurationService
	{
		#region IDeviceConfigurationService members
		/// <summary>
		/// Возвращает перечень сконфигурированных устройств.
		/// </summary>
		public IReadOnlyCollection<DeviceDto> GetDevices()
		{
			return new[]
			{
				new DeviceDto
				{
					Id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
					Parameters = new List<ParameterDto>
					{
						new AnalogParameterDto
						{
							Id = Guid.Parse("7086ad5b-d9cb-469f-a165-70867728950e")
						},
						new DiscreteParameterDto
						{
							Id = Guid.Parse("7086ad5b-d9cb-469f-a165-708677289502")
						}
					}
				}
			};
		}
		#endregion
	}
}
