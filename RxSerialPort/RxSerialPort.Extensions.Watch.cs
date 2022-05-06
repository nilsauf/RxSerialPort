namespace System.IO.Ports
{
	using System;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Linq;

	public static partial class RxSerialPort
	{
		public static IObservable<string> WatchData(this IObservable<RxSerialPortEvent> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == RxSerialPortEventType.DataReceived)
				.Where(@event => string.IsNullOrEmpty(@event.Data) == false)
				.Select(@event => @event.Data);
		}

		public static IObservable<SerialError> WatchErrors(this IObservable<RxSerialPortEvent> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == RxSerialPortEventType.ErrorReceived)
				.Where(@event => @event.ErrorType.HasValue)
				.Select(@event => @event.ErrorType.Value);
		}

		public static IObservable<SerialPinChange> WatchPinChanges(this IObservable<RxSerialPortEvent> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == RxSerialPortEventType.PinChanged)
				.Where(@event => @event.PinChangeType.HasValue)
				.Select(@event => @event.PinChangeType.Value);
		}

		public static IObservable<Unit> WatchDisposing(this IObservable<RxSerialPortEvent> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == RxSerialPortEventType.Disposed)
				.Select(_ => Unit.Default);
		}
	}
}
