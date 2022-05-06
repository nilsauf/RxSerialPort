namespace System.IO.Ports
{
	using System;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Linq;

	public static partial class RxSerialPort
	{
		/// <summary>
		/// Watch the data events of an observable stream of <see cref="RxSerialPortEvent"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent"/></param>
		/// <returns>An observable stream of received data events</returns>
		/// <exception cref="ArgumentNullException"></exception>
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

		/// <summary>
		/// Watch the <see cref="SerialError"/> events of an observable stream of <see cref="RxSerialPortEvent"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent"/></param>
		/// <returns>An observable stream of received <see cref="SerialError"/> events</returns>
		/// <exception cref="ArgumentNullException"></exception>
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

		/// <summary>
		/// Watch the <see cref="SerialPinChange"/> events of an observable stream of <see cref="RxSerialPortEvent"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent"/></param>
		/// <returns>An observable stream of received <see cref="SerialPinChange"/> events</returns>
		/// <exception cref="ArgumentNullException"></exception>
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

		/// <summary>
		/// Watch the disposing event of an observable stream of <see cref="RxSerialPortEvent"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent"/></param>
		/// <returns>An observable stream of received disposing event</returns>
		/// <exception cref="ArgumentNullException"></exception>
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
