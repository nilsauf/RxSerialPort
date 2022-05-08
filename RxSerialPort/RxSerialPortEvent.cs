namespace System.IO.Ports
{
	using System;

	/// <summary>
	/// An event description of a <see cref="SerialPort"/> event
	/// </summary>
	public struct RxSerialPortEvent<TData>
	{
		internal RxSerialPortEvent(SerialPort serialPort, SerialData serialData, TData data)
			: this(serialPort, RxSerialPortEventType.DataReceivedAndRead)
		{
			this.Data = data;
			this.SerialData = serialData;
		}

		internal RxSerialPortEvent(SerialPort serialPort, SerialData serialData)
			: this(serialPort, RxSerialPortEventType.DataReceived)
		{
			this.SerialData = serialData;
		}

		internal RxSerialPortEvent(SerialPort serialPort, SerialError errorType)
			: this(serialPort, RxSerialPortEventType.ErrorReceived)
		{
			this.ErrorType = errorType;
		}

		internal RxSerialPortEvent(SerialPort serialPort, SerialPinChange pinChangeType)
			: this(serialPort, RxSerialPortEventType.PinChanged)
		{
			this.PinChangeType = pinChangeType;
		}

		internal RxSerialPortEvent(SerialPort serialPort)
			: this(serialPort, RxSerialPortEventType.Disposed)
		{
		}

		private RxSerialPortEvent(SerialPort sender, RxSerialPortEventType eventType)
		{
			this.Sender = sender ?? throw new ArgumentNullException(nameof(sender));
			this.EventType = eventType;
			this.Data = default(TData?);
			this.SerialData = null;
			this.ErrorType = null;
			this.PinChangeType = null;
			this.TimeStamp = DateTime.Now;
		}

		internal SerialPort Sender { get; }

		/// <summary>
		/// The name of the serialport which sended this event
		/// </summary>
		public string PortName => this.Sender.PortName;

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

		public DateTime TimeStamp { get; }

		/// <inheritdoc/>
		public override string ToString()
		{
			string result = $"{nameof(RxSerialPortEvent<TData>)}: " +
				$"{nameof(this.EventType)} = {this.EventType}; " +
				$"{nameof(this.PortName)} = {this.PortName}" +
				$"{nameof(this.TimeStamp)} = {this.TimeStamp.ToLongDateString()} {this.TimeStamp.ToLongTimeString()}";

			switch (this.EventType)
			{
				case RxSerialPortEventType.DataReceived:
					return result + $"; {nameof(this.SerialData)} = {this.SerialData}";
				case RxSerialPortEventType.DataReceivedAndRead:
					return result + $"; {nameof(this.SerialData)} = {this.SerialData}; {nameof(this.Data)} = {this.Data}";
				case RxSerialPortEventType.ErrorReceived:
					return result + $"; {nameof(this.ErrorType)} = {this.ErrorType}";
				case RxSerialPortEventType.PinChanged:
					return result + $"; {nameof(this.PinChangeType)} = {this.PinChangeType}";
				case RxSerialPortEventType.Disposed:
				default:
					return result;
			}
		}
	}
}
