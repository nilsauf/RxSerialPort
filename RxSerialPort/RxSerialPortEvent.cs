namespace System.IO.Ports
{
	using System;

	/// <summary>s
	/// An event description of a <see cref="SerialPort"/> event
	/// </summary>
	public struct RxSerialPortEvent<TData>
	{
		internal RxSerialPortEvent(SerialPort serialPort, SerialData serialData, TData data, DateTime timestamp)
			: this(serialPort, RxSerialPortEventType.DataReceivedAndRead, serialData: serialData, data: data, timestamp: timestamp)
		{
		}

		internal RxSerialPortEvent(SerialPort serialPort, SerialData serialData)
			: this(serialPort, RxSerialPortEventType.DataReceived, serialData: serialData)
		{
		}

		internal RxSerialPortEvent(SerialPort serialPort, SerialError errorType)
			: this(serialPort, RxSerialPortEventType.ErrorReceived, errorType: errorType)
		{
		}

		internal RxSerialPortEvent(SerialPort serialPort, SerialPinChange pinChangeType)
			: this(serialPort, RxSerialPortEventType.PinChanged, pinChangeType: pinChangeType)
		{
		}

		internal RxSerialPortEvent(SerialPort serialPort)
			: this(serialPort, RxSerialPortEventType.Disposed)
		{
		}

		private RxSerialPortEvent(
			SerialPort serialPort,
			RxSerialPortEventType eventType,
			TData? data = default,
			SerialData? serialData = null,
			SerialError? errorType = null,
			SerialPinChange? pinChangeType = null,
			DateTime? timestamp = null)
		{
			this.serialPort = serialPort ?? throw new ArgumentNullException(nameof(serialPort));
			this.PortName = this.serialPort.PortName;
			this.EventType = eventType;
			this.Data = data;
			this.SerialData = serialData;
			this.ErrorType = errorType;
			this.PinChangeType = pinChangeType;
			this.TimeStamp = timestamp ?? DateTime.Now;
		}

		internal readonly SerialPort serialPort;

		/// <summary>
		/// The name of the serialport which sended this event
		/// </summary>
		public string PortName { get; }

		/// <summary>
		/// The type of this event
		/// </summary>
		public RxSerialPortEventType EventType { get; }

		/// <summary>
		/// The received data if <see cref="EventType"/> == <see cref="RxSerialPortEventType.DataReceivedAndRead"/> null otherwise
		/// </summary>
		public TData? Data { get; }

		/// <summary>
		/// The type of serial data received if <see cref="EventType"/> == <see cref="RxSerialPortEventType.DataReceived"/> null otherwise
		/// </summary>
		public SerialData? SerialData { get; }

		/// <summary>
		/// The type of error if <see cref="EventType"/> == <see cref="RxSerialPortEventType.ErrorReceived"/> null otherwise
		/// </summary>
		public SerialError? ErrorType { get; }

		/// <summary>
		/// The type of error if <see cref="PinChangeType"/> == <see cref="RxSerialPortEventType.PinChanged"/> null otherwise
		/// </summary>
		public SerialPinChange? PinChangeType { get; }

		/// <summary>
		/// The time this event was created.
		/// </summary>
		public DateTime TimeStamp { get; }

		internal RxSerialPortEvent<TTargetData> CastTo<TTargetData>()
		{
			if (this.EventType is RxSerialPortEventType.DataReceivedAndRead)
				throw new InvalidOperationException("Can't convert event with read data to other event data type");
			return new RxSerialPortEvent<TTargetData>(
				this.serialPort,
				this.EventType,
				default,
				this.SerialData,
				this.ErrorType,
				this.PinChangeType,
				this.TimeStamp);
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			string result = $"{nameof(RxSerialPortEvent<TData>)}: " +
				$"{nameof(this.EventType)} = {this.EventType}; " +
				$"{nameof(this.PortName)} = {this.PortName}" +
				$"{nameof(this.TimeStamp)} = {this.TimeStamp.ToLongDateString()} {this.TimeStamp.ToLongTimeString()}";

			return this.EventType switch
			{
				RxSerialPortEventType.DataReceived => result + $"; {nameof(this.SerialData)} = {this.SerialData}",
				RxSerialPortEventType.DataReceivedAndRead => result + $"; {nameof(this.SerialData)} = {this.SerialData}; {nameof(this.Data)} = {this.Data}",
				RxSerialPortEventType.ErrorReceived => result + $"; {nameof(this.ErrorType)} = {this.ErrorType}",
				RxSerialPortEventType.PinChanged => result + $"; {nameof(this.PinChangeType)} = {this.PinChangeType}",
				_ => result,
			};
		}
	}
}
