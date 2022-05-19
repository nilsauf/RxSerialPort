namespace System.IO.Ports
{
	using ReactiveMarbles.ObservableEvents;
	using System.Reactive;
	using System.Reactive.Linq;

	/// <summary>
	/// Extensionmethods to make a <see cref="SerialPort"/> reactive and static methods to create observable streams of <see cref="SerialPort"/> events
	/// </summary>
	public static partial class RxSerialPort
	{
		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/> created by <paramref name="portName"/>.
		/// </summary>
		/// <param name="portName">The name of the port to connect to.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <remarks>
		/// This overload does NOT read data if data was received! 
		/// Use <see cref="RxSerialPort.Connect{TData}(string, Func{SerialPort, TData})"/> if data should be read.
		/// </remarks>
		public static IObservable<RxSerialPortEvent<Unit>> Connect(
			string portName)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName));
		}

		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/> created by <paramref name="portName"/> and <paramref name="baudRate"/>.
		/// </summary>
		/// <param name="portName">The name of the port to connect to.</param>
		/// <param name="baudRate">The baudrate setting of the port to connect to.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <remarks>
		/// This overload does NOT read data if data was received! 
		/// Use <see cref="RxSerialPort.Connect{TData}(string, int, Func{SerialPort, TData})"/> if data should be read.
		/// </remarks>
		public static IObservable<RxSerialPortEvent<Unit>> Connect(
			string portName, int baudRate)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate));
		}

		/// <summary>
		/// Connects to the events of a <see cref="SerialPort"/> created by <paramref name="portName"/>, <paramref name="baudRate"/> and <paramref name="parity"/>.
		/// </summary>
		/// <param name="portName">The name of the port to connect to.</param>
		/// <param name="baudRate">The baudrate setting of the port to connect to.</param>
		/// <param name="parity">The parity settings of the port to connect to.</param>
		/// <returns>An observable stream of serial port events of the <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <remarks>
		/// This overload does NOT read data if data was received! 
		/// Use <see cref="RxSerialPort.Connect{TData}(string, int, Parity, Func{SerialPort, TData})"/> if data should be read.
		/// </remarks>
		public static IObservable<RxSerialPortEvent<Unit>> Connect(
			string portName, int baudRate, Parity parity)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate, parity));
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
		/// <remarks>
		/// This overload does NOT read data if data was received! 
		/// Use <see cref="RxSerialPort.Connect{TData}(string, int, Parity, int, Func{SerialPort, TData})"/> if data should be read.
		/// </remarks>
		public static IObservable<RxSerialPortEvent<Unit>> Connect(
			string portName, int baudRate, Parity parity, int dataBits)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate, parity, dataBits));
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
		/// <remarks>
		/// This overload does NOT read data if data was received! 
		/// Use <see cref="RxSerialPort.Connect{TData}(string, int, Parity, int, StopBits, Func{SerialPort, TData})"/> if data should be read.
		/// </remarks>
		public static IObservable<RxSerialPortEvent<Unit>> Connect(
			string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return Connect(() => new SerialPort(portName, baudRate, parity, dataBits, stopBits));
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
		public static IObservable<RxSerialPortEvent<Unit>> Connect(
			Func<SerialPort> portFactory)
		{
			if (portFactory is null)
			{
				throw new ArgumentNullException(nameof(portFactory));
			}

			return Observable.Using(portFactory, serialPort =>
			{
				if (serialPort is null)
				{
					return Observable.Throw<RxSerialPortEvent<Unit>>(
						new InvalidOperationException($"{nameof(portFactory)} of Connect returned null!"));
				}

				if (serialPort.IsOpen == false)
				{
					try
					{
						serialPort.Open();
					}
					catch (Exception ex)
					{
						return Observable.Throw<RxSerialPortEvent<Unit>>(ex);
					}
				}
				return serialPort.Connect();
			})
			.Publish()
			.AutoConnect();
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
		public static IObservable<RxSerialPortEvent<Unit>> Connect(
			this SerialPort serialPort)
		{
			if (serialPort is null)
			{
				throw new ArgumentNullException(nameof(serialPort));
			}

			var serialPortEvents = serialPort.Events();

			return Observable.Merge(
					serialPortEvents.DataReceived.Select(CreateReceived),
					serialPortEvents.Disposed.Select(CreateDisposed),
					serialPortEvents.ErrorReceived.Select(CreateErrorReceived),
					serialPortEvents.PinChanged.Select(CreatePinChanged))
				.TakeUntil(@event => @event.EventType == RxSerialPortEventType.Disposed)
				.AsObservable();

			RxSerialPortEvent<Unit> CreateReceived(SerialDataReceivedEventArgs dataReceivedArgs)
			{
				return new RxSerialPortEvent<Unit>(serialPort, dataReceivedArgs.EventType);
			}

			RxSerialPortEvent<Unit> CreateDisposed(EventArgs _)
			{
				return new RxSerialPortEvent<Unit>(serialPort);
			}

			RxSerialPortEvent<Unit> CreateErrorReceived(SerialErrorReceivedEventArgs errorEventArgs)
			{
				return new RxSerialPortEvent<Unit>(serialPort, errorEventArgs.EventType);
			}

			RxSerialPortEvent<Unit> CreatePinChanged(SerialPinChangedEventArgs pinChangedEventArgs)
			{
				return new RxSerialPortEvent<Unit>(serialPort, pinChangedEventArgs.EventType);
			}
		}
	}
}
