namespace System.IO.Ports
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Linq;

	/// <summary>
	/// Extensions for an event stream from a <see cref="SerialPort"/>
	/// </summary>
	public static partial class RxSerialPort_Extensions
	{
		/// <summary>
		/// Reads line by line if data was received.
		/// </summary>
		/// <param name="serialPortEvents">The observable stream of events of a <see cref="SerialPort"/></param>
		/// <returns><paramref name="serialPortEvents"/> plus events with the read lines</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<RxSerialPortEvent<string>> AndReadLine(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadLine());
		}

		/// <summary>
		/// Reads existing data as a <see cref="string"/> if data was received.
		/// </summary>
		/// <param name="serialPortEvents">The observable stream of events of a <see cref="SerialPort"/></param>
		/// <returns><paramref name="serialPortEvents"/> plus events with the read existing data as a <see cref="string"/></returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<RxSerialPortEvent<string>> AndReadExisting(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadExisting());
		}

		/// <summary>
		/// Reads a <see cref="string"/> till a given <paramref name="value"/> if data was received.
		/// </summary>
		/// <param name="serialPortEvents">The observable stream of events of a <see cref="SerialPort"/></param>
		/// <param name="value">The <see cref="string"/> to read to</param>
		/// <returns><paramref name="serialPortEvents"/> plus events with the read data as a <see cref="string"/></returns>
		/// <exception cref="ArgumentNullException"></exception>
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

		/// <summary>
		/// Reads a single byte if data was received.
		/// </summary>
		/// <param name="serialPortEvents">The observable stream of events of a <see cref="SerialPort"/></param>
		/// <returns><paramref name="serialPortEvents"/> plus events with the read byte</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<RxSerialPortEvent<int>> AndReadByte(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadByte());
		}

		/// <summary>
		/// Reads a single char if data was received.
		/// </summary>
		/// <param name="serialPortEvents">The observable stream of events of a <see cref="SerialPort"/></param>
		/// <returns><paramref name="serialPortEvents"/> plus events with the read char</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<RxSerialPortEvent<int>> AndReadChar(
			this IObservable<RxSerialPortEvent<Unit>> serialPortEvents)
		{
			if (serialPortEvents is null)
			{
				throw new ArgumentNullException(nameof(serialPortEvents));
			}

			return serialPortEvents.AndRead(port => port.ReadChar());
		}

		/// <summary>
		/// Reads data with a given <paramref name="readFunction"/> if data was received
		/// </summary>
		/// <param name="serialPortEvents">The observable stream of events of a <see cref="SerialPort"/></param>
		/// <param name="readFunction">The function to read the data from the <see cref="SerialPort"/></param>
		/// <typeparam name="TData">The type of data to read</typeparam>
		/// <returns><paramref name="serialPortEvents"/> plus events with the read data</returns>
		/// <exception cref="ArgumentNullException"></exception>
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
					while (unitEvent.serialPort.BytesToRead > 0)
					{
						yield return new RxSerialPortEvent<TData>(
							unitEvent.serialPort,
							unitEvent.SerialData.GetValueOrDefault(),
							readFunction(unitEvent.serialPort),
							unitEvent.TimeStamp);
					}
				}
			}
		}
	}
}
