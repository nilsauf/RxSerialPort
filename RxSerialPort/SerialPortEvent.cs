namespace System.IO.Ports
{
	using System;

	public struct SerialPortEvent
	{
		internal SerialPortEvent(SerialPort serialPort, string data)
			: this(serialPort, SerialPortEventType.DataReceived)
		{
			this.Data = data;
		}

		internal SerialPortEvent(SerialPort serialPort)
			: this(serialPort, SerialPortEventType.Disposed)
		{

		}

		internal SerialPortEvent(SerialPort serialPort, SerialError errorType)
			: this(serialPort, SerialPortEventType.ErrorReceived)
		{
			this.ErrorType = errorType;
		}

		internal SerialPortEvent(SerialPort serialPort, SerialPinChange pinChangeType)
			: this(serialPort, SerialPortEventType.PinChanged)
		{
			this.PinChangeType = pinChangeType;
		}

		private SerialPortEvent(SerialPort sender, SerialPortEventType eventType)
		{
			this.Sender = sender ?? throw new ArgumentNullException(nameof(sender));
			this.EventType = eventType;
			this.Data = null;
			this.ErrorType = null;
			this.PinChangeType = null;
		}

		public SerialPort Sender { get; }
		public SerialPortEventType EventType { get; }
		public string Data { get; }
		public SerialError? ErrorType { get; }
		public SerialPinChange? PinChangeType { get; }

		public override string ToString()
		{
			string result = $"{nameof(SerialPortEvent)}: {nameof(this.EventType)} = {this.EventType}";

			switch (this.EventType)
			{
				case SerialPortEventType.DataReceived:
					return result + $"; {nameof(this.Data)} = {this.Data}";
				case SerialPortEventType.ErrorReceived:
					return result + $"; {nameof(this.ErrorType)} = {this.ErrorType.Value}";
				case SerialPortEventType.PinChanged:
					return result + $"; {nameof(this.PinChangeType)} = {this.PinChangeType.Value}";
				case SerialPortEventType.Disposed:
				default:
					return result;
			}
		}
	}
}
