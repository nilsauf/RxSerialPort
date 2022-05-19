namespace System.IO.Ports
{
	/// <summary>
	/// The type of event fired by a <see cref="SerialPort"/>
	/// </summary>
	public enum RxSerialPortEventType
	{
		DataReceived,
		DataReceivedAndRead,
		ErrorReceived,
		PinChanged,
		Disposed
	}
}
