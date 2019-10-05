using System.Collections.Generic;
using Iot.Common;


namespace Iot.Gateway.Arduino.ExtServices
{
	/// <summary>
	/// Предоставляет механизм для получения сконфигурированных устройств.
	/// </summary>
	public interface IDeviceConfigurationService
	{
		#region Overridable
		/// <summary>
		/// Возвращает перечень сконфигурированных устройств.
		/// </summary>
		IReadOnlyCollection<DeviceDto> GetDevices();
		#endregion
	}
}
