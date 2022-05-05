namespace System.IO.Ports
{
	using System;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Linq;

	public static partial class RxSerialPort
	{
		public static IObservable<string> WatchData(this IObservable<SerialPortEvent> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == SerialPortEventType.DataReceived)
				.Where(@event => string.IsNullOrEmpty(@event.Data) == false)
				.Select(@event => @event.Data);
		}

		public static IObservable<SerialError> WatchErrors(this IObservable<SerialPortEvent> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == SerialPortEventType.ErrorReceived)
				.Where(@event => @event.ErrorType.HasValue)
				.Select(@event => @event.ErrorType.Value);
		}

		public static IObservable<SerialPinChange> WatchPinChanges(this IObservable<SerialPortEvent> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == SerialPortEventType.PinChanged)
				.Where(@event => @event.PinChangeType.HasValue)
				.Select(@event => @event.PinChangeType.Value);
		}

		public static IObservable<Unit> WatchDisposing(this IObservable<SerialPortEvent> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == SerialPortEventType.Disposed)
				.Select(_ => Unit.Default);
		}
	}
}
