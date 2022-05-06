namespace System.IO.Ports
{
	using System;

	public static partial class RxSerialPort
	{
		public static IDisposable WriteTo(
			this IObservable<string> source,
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

		public static IDisposable WriteTo(
			this IObservable<string> source,
			Func<SerialPort> portFactoy,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (source is null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			if (portFactoy is null)
			{
				throw new ArgumentNullException(nameof(portFactoy));
			}

			return source.Subscribe(CreateWriteObserver(portFactoy, errorAction, completedAction));
		}

		public static IDisposable WriteTo(
			this IObservable<string> source,
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

			return source.Subscribe(serialPort.AsWriteObserver(errorAction, completedAction));
		}
	}
}
