using System;

namespace Iot.Gateway.Arduino.Domain
{
	public class Parameter
	{
		#region Data
		#region Fields
		private object _value;
		#endregion
		#endregion

		#region .ctor
		public Parameter(Guid parameterId)
		{
			ParameterId = parameterId;
		}
		#endregion

		#region Properties
		public Guid ParameterId
		{
			get;
		}

		public object Value
		{
			get => _value;
			set => _value = value ?? throw new ArgumentNullException(nameof(value),
																	 "Значение параметра не может быть пустым");
		}
		#endregion
	}
}
