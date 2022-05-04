using System;
using System.IO.Ports;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RxSerialPort.Tests
{
	public class RxSerialPort_Tests
	{
		[Fact()]
		public async Task Connect_Test()
		{
			var serialPort = new SerialPort(SerialPort.GetPortNames()[0]);
			var serialPort2 = new SerialPort(SerialPort.GetPortNames()[1]);

			serialPort.Connect()
				.Where(@event => @event.EventType == SerialPortEventType.DataReceived)
				.Select(@event => @event.Data)
				.Subscribe(data => { }, ex => { }, () => { });

			serialPort.Open();
			serialPort2.Open();
			serialPort2.WriteLine("Hello Port");

			await Task.Delay(500);

			serialPort2.Dispose();
			serialPort.Dispose();
		}
	}
}