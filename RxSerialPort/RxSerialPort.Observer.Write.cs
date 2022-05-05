namespace System.IO.Ports
{
	using System;

	public static partial class RxSerialPort
	{
		public static IObserver<string> CreateWriteObserver(
			string portName,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return CreateWriteObserver(() => new SerialPort(portName), errorAction, completedAction);
		}

		public static IObserver<string> CreateWriteObserver(
			string portName,
			int baudRate,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return CreateWriteObserver(() => new SerialPort(portName, baudRate), errorAction, completedAction);
		}

		public static IObserver<string> CreateWriteObserver(
			string portName,
			int baudRate,
			Parity parity,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return CreateWriteObserver(() => new SerialPort(portName, baudRate, parity), errorAction, completedAction);
		}

		public static IObserver<string> CreateWriteObserver(
			string portName,
			int baudRate,
			Parity parity,
			int dataBits,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return CreateWriteObserver(() => new SerialPort(portName, baudRate, parity, dataBits), errorAction, completedAction);
		}

		public static IObserver<string> CreateWriteObserver(
			string portName,
			int baudRate,
			Parity parity,
			int dataBits,
			StopBits stopBits,
			Action<Exception> errorAction = null,
			Action completedAction = null)
		{
			if (string.IsNullOrWhiteSpace(portName))
			{
				throw new ArgumentException($"'{nameof(portName)}' cannot be null or whitespace.", nameof(portName));
			}

			return CreateWriteObserver(() => new SerialPort(portName, baudRate, parity, dataBits, stopBits), errorAction, completedAction);
		}

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
	}
}
