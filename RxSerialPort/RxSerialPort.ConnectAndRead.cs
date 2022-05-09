namespace System.IO.Ports
{
	using ReactiveMarbles.ObservableEvents;
	using System;
	using System.Collections.Generic;
	using System.Reactive.Linq;

	public static partial class RxSerialPort
	{
		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/> created by <paramref name="portName"/>.
		/// </summary>
		/// <param name="portName">The name of the port to connect to.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static IObservable<RxSerialPortEvent<TData>> Connect<TData>(
			string portName,
			Func<SerialPort, TData> readFunc = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName), readFunc);
		}

		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/> created by <paramref name="portName"/> and <paramref name="baudRate"/>.
		/// </summary>
		/// <param name="portName">The name of the port to connect to.</param>
		/// <param name="baudRate">The baudrate setting of the port to connect to.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static IObservable<RxSerialPortEvent<TData>> Connect<TData>(
			string portName, int baudRate,
			Func<SerialPort, TData> readFunc = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate), readFunc);
		}

		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/> created by <paramref name="portName"/>, <paramref name="baudRate"/> and <paramref name="parity"/>.
		/// </summary>
		/// <param name="portName">The name of the port to connect to.</param>
		/// <param name="baudRate">The baudrate setting of the port to connect to.</param>
		/// <param name="parity">The parity settings of the port to connect to.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static IObservable<RxSerialPortEvent<TData>> Connect<TData>(
			string portName, int baudRate, Parity parity,
			Func<SerialPort, TData> readFunc = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(
				() => new SerialPort(portName, baudRate, parity), readFunc);
		}

		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/> created by <paramref name="portName"/>, <paramref name="baudRate"/>, <paramref name="parity"/> and <paramref name="dataBits"/>.
		/// </summary>
		/// <param name="portName">The name of the port to connect to.</param>
		/// <param name="baudRate">The baudrate setting of the port to connect to.</param>
		/// <param name="parity">The parity setting of the port to connect to.</param>
		/// <param name="dataBits">The dataBits setting of the port to connect to.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static IObservable<RxSerialPortEvent<TData>> Connect<TData>(
			string portName, int baudRate, Parity parity, int dataBits,
			Func<SerialPort, TData> readFunc = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(
				() => new SerialPort(portName, baudRate, parity, dataBits),
				readFunc);
		}

		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/> created by <paramref name="portName"/>, <paramref name="baudRate"/>, <paramref name="parity"/>, <paramref name="dataBits"/> and <paramref name="stopBits"/>.
		/// </summary>
		/// <param name="portName">The name of the port to connect to.</param>
		/// <param name="baudRate">The baudrate setting of the port to connect to.</param>
		/// <param name="parity">The parity setting of the port to connect to.</param>
		/// <param name="dataBits">The dataBits setting of the port to connect to.</param>
		/// <param name="stopBits">The stopBits setting of the port to connect to.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		public static IObservable<RxSerialPortEvent<TData>> Connect<TData>(
			string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits,
			Func<SerialPort, TData> readFunc = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(
				() => new SerialPort(portName, baudRate, parity, dataBits, stopBits),
				readFunc);
		}

		/// <summary>
		/// Creates, manages and connects to a <see cref="SerialPort"/>. 
		/// </summary>
		/// <param name="portFactory">A factory function to create a <see cref="SerialPort"/>.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/> created by <paramref name="portFactory"/>.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <remarks>
		/// The created <see cref="SerialPort"/> will be managed by the stream. 
		/// Don't open, close, dispose or use it anywhere else.
		/// </remarks>
		public static IObservable<RxSerialPortEvent<TData>> Connect<TData>(
			Func<SerialPort> portFactory,
			Func<SerialPort, TData> readFunc = null)
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
				return serialPort.Connect(readFunc);
			});
		}

		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/>.
		/// </summary>
		/// <param name="serialPort">The <see cref="SerialPort"/> to connect to.</param>
		/// <param name="readFunc">An optional function to read data from <paramref name="serialPort"/> on data reveceived</param>
		/// <returns>An observable stream of serial port events.</returns>
		/// <exception cref="ArgumentNullException"></exception>S
		/// <remarks>
		/// The provided <paramref name="serialPort"/> will NOT be managed by the stream!
		/// It needs to be opend, closed and disposed by the using code.
		/// </remarks>
		public static IObservable<RxSerialPortEvent<TData>> Connect<TData>(
			this SerialPort serialPort,
			Func<SerialPort, TData> readFunc = null)
		{
			if (serialPort is null)
			{
				throw new ArgumentNullException(nameof(serialPort));
			}

			var serialPortEvents = serialPort.Events();

			return Observable.Merge(
					serialPortEvents.DataReceived.SelectMany(CreateReceivedOrRead),
					serialPortEvents.Disposed.Select(CreateDisposed),
					serialPortEvents.ErrorReceived.Select(CreateErrorReceived),
					serialPortEvents.PinChanged.Select(CreatePinChanged))
				.TakeUntil(@event => @event.EventType == RxSerialPortEventType.Disposed)
				.AsObservable();

			IEnumerable<RxSerialPortEvent<TData>> CreateReceivedOrRead(SerialDataReceivedEventArgs dataReceivedArgs)
			{
				yield return new RxSerialPortEvent<TData>(serialPort, dataReceivedArgs.EventType);
				if (readFunc != null)
				{
					yield return new RxSerialPortEvent<TData>(serialPort, dataReceivedArgs.EventType, readFunc(serialPort));
				}
			}

			RxSerialPortEvent<TData> CreateDisposed(EventArgs _)
			{
				return new RxSerialPortEvent<TData>(serialPort);
			}

			RxSerialPortEvent<TData> CreateErrorReceived(SerialErrorReceivedEventArgs errorEventArgs)
			{
				return new RxSerialPortEvent<TData>(serialPort, errorEventArgs.EventType);
			}

			RxSerialPortEvent<TData> CreatePinChanged(SerialPinChangedEventArgs pinChangedEventArgs)
			{
				return new RxSerialPortEvent<TData>(serialPort, pinChangedEventArgs.EventType);
			}
		}
	}
}
