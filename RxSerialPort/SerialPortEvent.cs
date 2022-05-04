namespace RxSerialPort
{
	using System.IO.Ports;

	public struct SerialPortEvent
	{
		internal SerialPortEvent(SerialPort sender, SerialPortEventType eventType)
		{
			this.Sender = sender ?? throw new System.ArgumentNullException(nameof(sender));
			this.EventType = eventType;
		}

		public SerialPort Sender { get; }
		public SerialPortEventType EventType { get; }
	}
}
