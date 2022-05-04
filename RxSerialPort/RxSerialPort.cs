namespace RxSerialPort
{
	using ReactiveMarbles.ObservableEvents;
	using System;
	using System.IO.Ports;
	using System.Reactive.Linq;

	public static class RxSerialPort
	{
		public static IObservable<SerialPortEvent> Connect(string portName)
		{
			return Connect(() => new SerialPort(portName));
		}

		public static IObservable<SerialPortEvent> Connect(Func<SerialPort> portFactory)
		{
			return Observable.Using(portFactory, serialPort =>
			{
				if(serialPort.IsOpen == false)
				{
					serialPort.Open();
				}
				return serialPort.Connect();
			});
		}

		public static IObservable<SerialPortEvent> Connect(
			this SerialPort serialPort)
		{
			var serialPortEvents = serialPort.Events();

			return serialPortEvents.DataReceived
				.Select(line => new SerialPortEvent(serialPort, serialPort.ReadExisting()))
			.Merge(serialPortEvents.Disposed
				.Select(_ => new SerialPortEvent(serialPort)))
			.Merge(serialPortEvents.ErrorReceived
				.Select(errorEventArgs => new SerialPortEvent(serialPort, errorEventArgs.EventType)))
			.Merge(serialPortEvents.PinChanged
				.Select(pinChangedEventArgs => new SerialPortEvent(serialPort, pinChangedEventArgs.EventType)))
			.TakeUntil(@event => @event.EventType == SerialPortEventType.Disposed)
			.AsObservable();
		}
	}
}
