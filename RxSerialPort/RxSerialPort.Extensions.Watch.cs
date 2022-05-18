namespace System.IO.Ports
{
	using System;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Linq;

	public static partial class RxSerialPort_Extensions
	{
#pragma warning disable CS8629 // Nullable value type may be null.

		/// <summary>
		/// Watch the data read events of an observable stream of <see cref="RxSerialPortEvent{TData}"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent{TData}"/></param>
		/// <returns>An observable stream of read data events</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<TData> WatchData<TData>(this IObservable<RxSerialPortEvent<TData>> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == RxSerialPortEventType.DataReceivedAndRead)
				.Where(@event => @event.Data != null)
				.Select(@event => @event.Data);
		}

		/// <summary>
		/// Watch the data events of an observable stream of <see cref="RxSerialPortEvent{TData}"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent{TData}"/></param>
		/// <returns>An observable stream of received data events</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<SerialData> WatchDataReceived<TData>(this IObservable<RxSerialPortEvent<TData>> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == RxSerialPortEventType.DataReceived)
				.Where(@event => @event.SerialData.HasValue)
				.Select(@event => @event.SerialData.Value);
		}

		/// <summary>
		/// Watch the <see cref="SerialError"/> events of an observable stream of <see cref="RxSerialPortEvent{TData}"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent{TData}"/></param>
		/// <returns>An observable stream of received <see cref="SerialError"/> events</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<SerialError> WatchErrors<TData>(this IObservable<RxSerialPortEvent<TData>> portEvents)
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
		/// Watch the <see cref="SerialPinChange"/> events of an observable stream of <see cref="RxSerialPortEvent{TData}"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent{TData}"/></param>
		/// <returns>An observable stream of received <see cref="SerialPinChange"/> events</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<SerialPinChange> WatchPinChanges<TData>(this IObservable<RxSerialPortEvent<TData>> portEvents)
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
		/// Watch the disposing event of an observable stream of <see cref="RxSerialPortEvent{TData}"/>
		/// </summary>
		/// <param name="portEvents">The source observable of <see cref="RxSerialPortEvent{TData}"/></param>
		/// <returns>An observable stream of received disposing event</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static IObservable<Unit> WatchDisposing<TData>(this IObservable<RxSerialPortEvent<TData>> portEvents)
		{
			if (portEvents is null)
			{
				throw new ArgumentNullException(nameof(portEvents));
			}

			return portEvents.Where(@event => @event.EventType == RxSerialPortEventType.Disposed)
				.Select(_ => Unit.Default);
		}

#pragma warning restore CS8629 // Nullable value type may be null.
	}
}
