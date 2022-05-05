﻿namespace System.IO.Ports
{
	using ReactiveMarbles.ObservableEvents;
	using System;
	using System.Reactive.Linq;

	public static partial class RxSerialPort
	{
		public static IObservable<SerialPortEvent> Connect(string portName)
		{
			return Connect(() => new SerialPort(portName));
		}

		public static IObservable<SerialPortEvent> Connect(string portName, int baudRate)
		{
			return Connect(() => new SerialPort(portName, baudRate));
		}

		public static IObservable<SerialPortEvent> Connect(string portName, int baudRate, Parity parity)
		{
			return Connect(() => new SerialPort(portName, baudRate, parity));
		}

		public static IObservable<SerialPortEvent> Connect(string portName, int baudRate, Parity parity, int dataBits)
		{
			return Connect(() => new SerialPort(portName, baudRate, parity, dataBits));
		}

		public static IObservable<SerialPortEvent> Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		{
			return Connect(() => new SerialPort(portName, baudRate, parity, dataBits, stopBits));
		}

		public static IObservable<SerialPortEvent> Connect(Func<SerialPort> portFactory)
		{
			return Observable.Using(portFactory, serialPort =>
			{
				if(serialPort.IsOpen == false)
				{
					serialPort.Open();
				}
				return serialPort.Connect();
			});
		}

		public static IObservable<SerialPortEvent> Connect(
			this SerialPort serialPort)
		{
			var serialPortEvents = serialPort.Events();

			return serialPortEvents.DataReceived
				.Select(line => new SerialPortEvent(serialPort, serialPort.ReadExisting()))
			.Merge(serialPortEvents.Disposed
				.Select(_ => new SerialPortEvent(serialPort)))
			.Merge(serialPortEvents.ErrorReceived
				.Select(errorEventArgs => new SerialPortEvent(serialPort, errorEventArgs.EventType)))
			.Merge(serialPortEvents.PinChanged
				.Select(pinChangedEventArgs => new SerialPortEvent(serialPort, pinChangedEventArgs.EventType)))
			.TakeUntil(@event => @event.EventType == SerialPortEventType.Disposed)
			.AsObservable();
		}
	}
}