namespace System.IO.Ports
{
	public static partial class RxSerialPort
	{
		/// <summary>
		/// Sends the <paramref name="source"/> data to a <see cref="SerialPort"/> using <see cref="SerialPort.Write(string)"/>.
		/// </summary>
		/// <param name="source">The source observable.</param>
		/// <param name="portName">The name of the port.</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An <see cref="IDisposable"/> to dispose the subscription.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static IDisposable WriteTo(
			this IObservable<char[]> source,
			string portName,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return source.WriteTo(() => new SerialPort(portName), errorAction, completedAction);
		}

		/// <summary>
		/// Sends the <paramref name="source"/> data to a <see cref="SerialPort"/> using <see cref="SerialPort.Write(string)"/>.
		/// </summary>
		/// <param name="source">The source observable.</param>
		/// <param name="portName">The name of the port.</param>
		/// <param name="baudRate">The baud rate of the port.</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An <see cref="IDisposable"/> to dispose the subscription.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static IDisposable WriteTo(
			this IObservable<char[]> source,
			string portName,
			int baudRate,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return source.WriteTo(() => new SerialPort(portName, baudRate), errorAction, completedAction);
		}

		/// <summary>
		/// Sends the <paramref name="source"/> data to a <see cref="SerialPort"/> using <see cref="SerialPort.Write(string)"/>.
		/// </summary>
		/// <param name="source">The source observable.</param>
		/// <param name="portName">The name of the port.</param>
		/// <param name="baudRate">The baud rate of the port.</param>
		/// <param name="parity">The parity bit of the port.</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An <see cref="IDisposable"/> to dispose the subscription.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static IDisposable WriteTo(
			this IObservable<char[]> source,
			string portName,
			int baudRate,
			Parity parity,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return source.WriteTo(() => new SerialPort(portName, baudRate, parity), errorAction, completedAction);
		}

		/// <summary>
		/// Sends the <paramref name="source"/> data to a <see cref="SerialPort"/> using <see cref="SerialPort.Write(string)"/>.
		/// </summary>
		/// <param name="source">The source observable.</param>
		/// <param name="portName">The name of the port.</param>
		/// <param name="baudRate">The baud rate of the port.</param>
		/// <param name="parity">The parity bit of the port.</param>
		/// <param name="dataBits">The data bits of the port.</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An <see cref="IDisposable"/> to dispose the subscription.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static IDisposable WriteTo(
			this IObservable<char[]> source,
			string portName,
			int baudRate,
			Parity parity,
			int dataBits,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return source.WriteTo(() => new SerialPort(portName, baudRate, parity, dataBits), errorAction, completedAction);
		}

		/// <summary>
		/// Sends the <paramref name="source"/> data to a <see cref="SerialPort"/> using <see cref="SerialPort.Write(string)"/>.
		/// </summary>
		/// <param name="source">The source observable.</param>
		/// <param name="portName">The name of the port.</param>
		/// <param name="baudRate">The baud rate of the port.</param>
		/// <param name="parity">The parity bit of the port.</param>
		/// <param name="dataBits">The data bits of the port.</param>
		/// <param name="stopBits">The stop bits of the port.</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An <see cref="IDisposable"/> to dispose the subscription.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public static IDisposable WriteTo(
			this IObservable<char[]> source,
			string portName,
			int baudRate,
			Parity parity,
			int dataBits,
			StopBits stopBits,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return source.WriteTo(() => new SerialPort(portName, baudRate, parity, dataBits, stopBits), errorAction, completedAction);
		}

		/// <summary>
		/// Sends the <paramref name="source"/> data to the created and internaly managed <see cref="SerialPort"/> using <see cref="SerialPort.Write(string)"/>.
		/// </summary>
		/// <param name="source">The source observable.</param>
		/// <param name="portFactory">The factory function to create a <see cref="SerialPort"/>.</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An <see cref="IDisposable"/> to dispose the subscription to the <paramref name="source"/> and the created <see cref="SerialPort"/>.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// The created <see cref="SerialPort"/> will be managed by the stream. 
		/// Don't open, close, dispose or use it anywhere else.
		/// </remarks>
		public static IDisposable WriteTo(
			this IObservable<char[]> source,
			Func<SerialPort> portFactory,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (portFactory is null)
			{
				throw new ArgumentNullException(nameof(portFactory));
			}

			return source.Subscribe(CreateCharsObserver(portFactory, errorAction, completedAction));
		}

		/// <summary>
		/// Sends the <paramref name="source"/> data to the externally managed <see cref="SerialPort"/> using <see cref="SerialPort.Write(string)"/>.
		/// </summary>
		/// <param name="source">The source observable.</param>
		/// <param name="serialPort">the <see cref="SerialPort"/> to write to.</param>
		/// <param name="errorAction">The optional action to handle errors in the stream.</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream.</param>
		/// <returns>An <see cref="IDisposable"/> to dispose the subscription to the <paramref name="source"/> but NOT the <paramref name="serialPort"/>.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <remarks>
		/// The provided <paramref name="serialPort"/> will NOT be managed by the stream!
		/// It needs to be opend, closed and disposed by the using code.
		/// </remarks>
		public static IDisposable WriteTo(
			this IObservable<char[]> source,
			SerialPort serialPort,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (serialPort is null)
			{
				throw new ArgumentNullException(nameof(serialPort));
			}

			return source.Subscribe(serialPort.AsCharsObserver(errorAction, completedAction));
		}
	}
}
