namespace System.IO.Ports
{
	using System;

	public static partial class RxSerialPort
	{
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
