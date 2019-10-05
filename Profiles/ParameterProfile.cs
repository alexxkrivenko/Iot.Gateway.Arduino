using AutoMapper;
using Iot.Events;
using Iot.Gateway.Arduino.Domain;

namespace Iot.Gateway.Arduino.Profiles
{
	public class ParameterProfile : Profile
	{
		#region .ctor
		public ParameterProfile()
		{
			CreateMap<Parameter, ArduinoParameterDto>();
		}
		#endregion
	}
}
