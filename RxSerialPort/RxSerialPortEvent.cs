namespace System.IO.Ports
{
	using System;

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

		public SerialPort Sender { get; }
		public RxSerialPortEventType EventType { get; }
		public string Data { get; }
		public SerialError? ErrorType { get; }
		public SerialPinChange? PinChangeType { get; }

		public override string ToString()
		{
			string result = $"{nameof(RxSerialPortEvent)}: {nameof(this.EventType)} = {this.EventType}";

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
