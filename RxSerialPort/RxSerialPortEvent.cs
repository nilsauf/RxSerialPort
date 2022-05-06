namespace System.IO.Ports
{
	using System;

	/// <summary>
	/// An event description of a <see cref="SerialPort"/> event
	/// </summary>
	public struct RxSerialPortEvent
	{
		internal RxSerialPortEvent(SerialPort serialPort, string data)
			: this(serialPort, RxSerialPortEventType.DataReceived)
		{
			this.Data = data;
		}

		internal RxSerialPortEvent(SerialPort serialPort)
			: this(serialPort, RxSerialPortEventType.Disposed)
		{

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

		private RxSerialPortEvent(SerialPort sender, RxSerialPortEventType eventType)
		{
			this.Sender = sender ?? throw new ArgumentNullException(nameof(sender));
			this.EventType = eventType;
			this.Data = null;
			this.ErrorType = null;
			this.PinChangeType = null;
		}

		/// <summary>
		/// The <see cref="SerialPort"/> which sended this event
		/// </summary>
		public SerialPort Sender { get; }

		/// <summary>
		/// The type of this event
		/// </summary>
		public RxSerialPortEventType EventType { get; }

		/// <summary>
		/// The received data if <see cref="EventType"/> == <see cref="RxSerialPortEventType.DataReceived"/> null otherwise
		/// </summary>
		public string Data { get; }

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
				$"{nameof(this.Sender)} = {this.Sender.PortName}";

			switch (this.EventType)
			{
				case RxSerialPortEventType.DataReceived:
					return result + $"; {nameof(this.Data)} = {this.Data}";
				case RxSerialPortEventType.ErrorReceived:
					return result + $"; {nameof(this.ErrorType)} = {this.ErrorType.Value}";
				case RxSerialPortEventType.PinChanged:
					return result + $"; {nameof(this.PinChangeType)} = {this.PinChangeType.Value}";
				case RxSerialPortEventType.Disposed:
				default:
					return result;
			}
		}
	}
}
