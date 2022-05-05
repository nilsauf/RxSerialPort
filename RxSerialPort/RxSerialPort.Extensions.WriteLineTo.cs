namespace System.IO.Ports
{
	using System;

	public static partial class RxSerialPort
	{
		public static IDisposable WriteLineTo(
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

			return source.WriteLineTo(() => new SerialPort(portName), errorAction, completedAction);
		}

		public static IDisposable WriteLineTo(
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

			var serialPort = portFactoy();
			return source.WriteLineTo(serialPort,
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

		public static IDisposable WriteLineTo(
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

			return source.Subscribe(CreateWriteLineObserver(() => serialPort, errorAction, completedAction));
		}
	}
}
