using System.Threading.Tasks;
using MQTTnet;

namespace Iot.Gateway.Arduino.MessageDispatcher
{
	public interface IMessageDisaptcher
	{
		#region Overridable
		Task Dispatch(MqttApplicationMessageReceivedEventArgs arg);
		#endregion
	}
}
