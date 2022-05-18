namespace System.IO.Ports
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Linq;

	public static partial class RxSerialPort
	{
		public static IObservable<RxSerialPortEvent<string>> AndReadLine(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadLine());
		}

		public static IObservable<RxSerialPortEvent<string>> AndReadExisting(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadExisting());
		}

		public static IObservable<RxSerialPortEvent<string>> AndReadTo(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents,
			string value)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadTo(value));
		}

		public static IObservable<RxSerialPortEvent<int>> AndReadByte(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadByte());
		}

		public static IObservable<RxSerialPortEvent<int>> AndReadChar(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadChar());
		}

		public static IObservable<RxSerialPortEvent<TData>> AndRead<TData>(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents,
			Func<SerialPort, TData> readFunction)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			if (readFunction is null)
			{
				throw new ArgumentNullException(nameof(readFunction));
			}

			return serialPortEvents.SelectMany(CastAndRead);

			IEnumerable<RxSerialPortEvent<TData>> CastAndRead(RxSerialPortEvent<Unit> unitEvent)
			{
				yield return unitEvent.CastTo<TData>();
				if (unitEvent.EventType is RxSerialPortEventType.DataReceived)
				{
					yield return new RxSerialPortEvent<TData>(
						unitEvent.serialPort,
						unitEvent.SerialData.GetValueOrDefault(),
						readFunction(unitEvent.serialPort));
				}
			}
		}
	}
}
