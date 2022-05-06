namespace System.IO.Ports
{
	using System;

	public static partial class RxSerialPort
	{
		/// <summary>
		/// Creates and manages a <see cref="SerialPort"/> and wraps it into an <see cref="IObserver{T}"/> which uses <see cref="SerialPort.WriteLine(string)"/> to send data.
		/// </summary>
		/// <param name="portFactory">The factory function to create a <see cref="SerialPort"/>.</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An observer which writes received data to the <see cref="SerialPort"/> created by <paramref name="portFactory"/>.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// The created <see cref="SerialPort"/> will be managed by the stream. 
		/// Don't open, close, dispose or use it anywhere else.
		/// </remarks>
		public static IObserver<string> CreateWriteLineObserver(
			Func<SerialPort> portFactory,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (portFactory is null)
			{
				throw new ArgumentNullException(nameof(portFactory));
			}

			return CreateObserver(
				portFactory,
				(serialPort, line) => serialPort.WriteLine(line),
				errorAction,
				completedAction);
		}

		/// <summary>
		/// Wraps an externally managed <see cref="SerialPort"/> into an <see cref="IObserver{T}"/> which uses <see cref="SerialPort.WriteLine(string)"/> to send data.
		/// </summary>
		/// <param name="serialPort">The <see cref="SerialPort"/> to wrap</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An observer which writes received data to the <paramref name="serialPort"/></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <remarks>
		/// The provided <paramref name="serialPort"/> will NOT be managed by the stream!
		/// It needs to be opend, closed and disposed by the using code.
		/// </remarks>
		public static IObserver<string> AsWriteLineObserver(
			this SerialPort serialPort,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (serialPort is null)
			{
				throw new ArgumentNullException(nameof(serialPort));
			}

			return serialPort.AsObserver(
				(serialPort, data) => serialPort.WriteLine(data),
				errorAction,
				completedAction);
		}
	}
}
