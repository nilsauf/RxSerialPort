namespace RxSerialPort
{
	using ReactiveMarbles.ObservableEvents;
	using System;
	using System.IO;
	using System.IO.Ports;
	using System.Reactive.Linq;

	public static class RxSerialPort
	{
		public static IObservable<SerialPortEvent> Connect(this SerialPort serialPort, bool autoOpen = true)
		{
			return Observable.Return(serialPort)
				.Do(serialPort =>
				{
					if (serialPort.IsOpen == false && autoOpen == true)
					{
						serialPort.Open();
					}
				})
				.SelectMany(serialPort =>
				{
					var serialPortEvents = serialPort.Events();

					return serialPortEvents.DataReceived
							.Select(line => new SerialPortEvent(serialPort, SerialPortEventType.DataReceived))
						.Merge(serialPortEvents.Disposed
							.Select(_ => new SerialPortEvent(serialPort, SerialPortEventType.Disposed)))
						.Merge(serialPortEvents.ErrorReceived
							.Select(args => new SerialPortEvent(serialPort, SerialPortEventType.ErrorReceived)))
						.Merge(serialPortEvents.PinChanged
							.Select(args => new SerialPortEvent(serialPort, SerialPortEventType.PinChanged)));
				})
				.TakeUntil(@event => @event.EventType == SerialPortEventType.Disposed)
				.Publish()
				.AutoConnect()
				.AsObservable();
		}
	}
}
