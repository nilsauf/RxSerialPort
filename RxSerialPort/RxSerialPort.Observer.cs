namespace System.IO.Ports
{
	using System;
	using System.Reactive;

	/// <summary>
	/// Methods to create an observer from a <see cref="SerialPort"/>
	/// </summary>
	public static partial class RxSerialPort_Observer
	{
		/// <summary>
		/// Creates and manages a <see cref="SerialPort"/> and wraps it into an <see cref="IObserver{T}"/>
		/// </summary>
		/// <param name="portFactory">The factory function to create a <see cref="SerialPort"/>.</param>
		/// <param name="writeAction">The action to call on new data to send</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An observer which writes received data to the <see cref="SerialPort"/> created by <paramref name="portFactory"/>.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		/// <remarks>
		/// The created <see cref="SerialPort"/> will be managed by the stream. 
		/// Don't open, close, dispose or use it anywhere else.
		/// </remarks>
		public static IObserver<TData> Create<TData>(
			Func<SerialPort> portFactory,
			Action<SerialPort, TData> writeAction,
			Action<Exception>? errorAction = null,
			Action? completedAction = null)
		{
			if (portFactory is null)
			{
				throw new ArgumentNullException(nameof(portFactory));
			}

			if (writeAction is null)
			{
				throw new ArgumentNullException(nameof(writeAction));
			}

			SerialPort serialPort = portFactory() ?? throw new InvalidOperationException($"{nameof(portFactory)} returned null!");
			return serialPort.AsObserver<TData>((serialPort, data) =>
			{
				if (serialPort.IsOpen == false)
				{
					serialPort.Open();
				}

				writeAction.Invoke(serialPort, data);
			},
			ex =>
			{
				serialPort.Dispose();
				errorAction?.Invoke(ex);
			},
			() =>
			{
				serialPort.Dispose();
				completedAction?.Invoke();
			});
		}

		/// <summary>
		/// Wraps an externally managed <see cref="SerialPort"/> and given <paramref name="writeAction"/> into an <see cref="IObserver{T}"/>.
		/// </summary>
		/// <param name="serialPort">The <see cref="SerialPort"/> to wrap</param>
		/// <param name="writeAction">The action to call on new data to send. It will not be called, if <paramref name="serialPort"/> is closed</param>
		/// <param name="errorAction">The optional action to handle errors in the stream</param>
		/// <param name="completedAction">The optional action the handle the completion of the stream</param>
		/// <returns>An observer which writes received data to the <paramref name="serialPort"/></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <remarks>
		/// The provided <paramref name="serialPort"/> will NOT be managed by the stream!
		/// It needs to be opend, closed and disposed by the using code.
		/// </remarks>
		public static IObserver<TData> AsObserver<TData>(
			this SerialPort serialPort,
			Action<SerialPort, TData> writeAction,
			Action<Exception>? errorAction = null,
			Action? completedAction = null)
		{
			if (serialPort is null)
			{
				throw new ArgumentNullException(nameof(serialPort));
			}

			if (writeAction is null)
			{
				throw new ArgumentNullException(nameof(writeAction));
			}

			return Observer.Create<TData>(
				data =>
				{
					if (serialPort.IsOpen)
					{
						writeAction(serialPort, data);
					}
				},
				ex => errorAction?.Invoke(ex),
				() => completedAction?.Invoke());
		}
	}
}
