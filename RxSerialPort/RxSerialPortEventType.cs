namespace System.IO.Ports
{
	/// <summary>
	/// The type of event fired by a <see cref="SerialPort"/>
	/// </summary>
	public enum RxSerialPortEventType
	{
		/// <summary>
		/// Data was received
		/// </summary>
		DataReceived,

		/// <summary>
		/// Data was received and read
		/// </summary>
		DataReceivedAndRead,

		/// <summary>
		/// An error occured
		/// </summary>
		ErrorReceived,

		/// <summary>
		/// The pin was changed
		/// </summary>
		PinChanged,

		/// <summary>
		/// The port was disposed
		/// </summary>
		Disposed
	}
}
