namespace System.IO.Ports
{
	using System;

	/// <summary>
	/// An event description of a <see cref="SerialPort"/> event
	/// </summary>
	public struct RxSerialPortEvent
	{
		internal RxSerialPortEvent(SerialPort serialPort, object data)
			: this(serialPort, RxSerialPortEventType.DataRead)
		{
			this.Data = data;
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
			this.Data = null;
			this.SerialData = null;
			this.ErrorType = null;
			this.PinChangeType = null;
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
		/// The received data if <see cref="EventType"/> == <see cref="RxSerialPortEventType.DataRead"/> null otherwise
		/// </summary>
		public object? Data { get; }

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

		/// <inheritdoc/>
		public override string ToString()
		{
			string result = $"{nameof(RxSerialPortEvent)}: " +
				$"{nameof(this.EventType)} = {this.EventType}; " +
				$"{nameof(this.PortName)} = {this.PortName}";

			switch (this.EventType)
			{
				case RxSerialPortEventType.DataReceived:
					return result + $"; {nameof(this.SerialData)} = {this.SerialData}";
				case RxSerialPortEventType.DataRead:
					return result + $"; {nameof(this.Data)} = {this.Data}";
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
