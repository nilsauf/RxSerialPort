namespace System.IO.Ports
{
	using System;

	public static partial class RxSerialPort
	{
		public static IObserver<string> CreateWriteObserver(
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
				(serialPort, data) => serialPort.Write(data),
				errorAction,
				completedAction);
		}

		public static IObserver<string> AsWriteObserver(
			this SerialPort serialPort,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (serialPort is null)
			{
				throw new ArgumentNullException(nameof(serialPort));
			}

			return serialPort.AsObserver(
				(serialPort, data) => serialPort.Write(data),
				errorAction,
				completedAction);
		}
	}
}
