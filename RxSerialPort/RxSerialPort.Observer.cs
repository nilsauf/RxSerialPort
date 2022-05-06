namespace System.IO.Ports
{
	using System;
	using System.Reactive;

	public static partial class RxSerialPort
	{
		public static IObserver<string> CreateObserver(
			Func<SerialPort> portFactory,
			Action<SerialPort, string> writeAction,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (portFactory is null)
			{
				throw new ArgumentNullException(nameof(portFactory));
			}

			if (writeAction is null)
			{
				throw new ArgumentNullException(nameof(writeAction));
			}

			return portFactory()?.AsObserver((serialPort, data) =>
			{
				if (serialPort.IsOpen == false)
				{
					serialPort.Open();
				}

				writeAction.Invoke(serialPort, data);
			},
			errorAction,
			completedAction) ?? throw new InvalidOperationException($"{nameof(portFactory)} returned null!");
		}

		public static IObserver<string> AsObserver(
			this SerialPort serialPort,
			Action<SerialPort, string> writeAction,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (serialPort is null)
			{
				throw new ArgumentNullException(nameof(serialPort));
			}

			if (writeAction is null)
			{
				throw new ArgumentNullException(nameof(writeAction));
			}

			return Observer.Create<string>(
				data => writeAction(serialPort, data),
				ex => errorAction?.Invoke(ex),
				() => completedAction?.Invoke());
		}
	}
}
