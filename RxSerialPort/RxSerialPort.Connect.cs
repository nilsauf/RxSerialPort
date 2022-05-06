namespace System.IO.Ports
{
	using ReactiveMarbles.ObservableEvents;
	using System;
	using System.Reactive.Linq;

	public static partial class RxSerialPort
	{
		public static IObservable<RxSerialPortEvent> Connect(string portName)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName));
		}

		public static IObservable<RxSerialPortEvent> Connect(string portName, int baudRate)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate));
		}

		public static IObservable<RxSerialPortEvent> Connect(string portName, int baudRate, Parity parity)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate, parity));
		}

		public static IObservable<RxSerialPortEvent> Connect(string portName, int baudRate, Parity parity, int dataBits)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate, parity, dataBits));
		}

		public static IObservable<RxSerialPortEvent> Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate, parity, dataBits, stopBits));
		}

		public static IObservable<RxSerialPortEvent> Connect(Func<SerialPort> portFactory)
		{
			if (portFactory is null)
			{
				throw new ArgumentNullException(nameof(portFactory));
			}

			return Observable.Using(portFactory, serialPort =>
			{
				if (serialPort.IsOpen == false)
				{
					serialPort.Open();
				}
				return serialPort.Connect();
			});
		}

		public static IObservable<RxSerialPortEvent> Connect(
			this SerialPort serialPort)
		{
			if (serialPort is null)
			{
				throw new ArgumentNullException(nameof(serialPort));
			}

			var serialPortEvents = serialPort.Events();

			return serialPortEvents.DataReceived
				.Select(line => new RxSerialPortEvent(serialPort, serialPort.ReadExisting()))
			.Merge(serialPortEvents.Disposed
				.Select(_ => new RxSerialPortEvent(serialPort)))
			.Merge(serialPortEvents.ErrorReceived
				.Select(errorEventArgs => new RxSerialPortEvent(serialPort, errorEventArgs.EventType)))
			.Merge(serialPortEvents.PinChanged
				.Select(pinChangedEventArgs => new RxSerialPortEvent(serialPort, pinChangedEventArgs.EventType)))
			.TakeUntil(@event => @event.EventType == RxSerialPortEventType.Disposed)
			.AsObservable();
		}
	}
}
