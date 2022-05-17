namespace System.IO.Ports.Tests
{
	using System.Diagnostics.CodeAnalysis;

	public static class RxSerialPort_TestTools
	{
		public static void OpenSafelyForTest(this SerialPort serialPort)
		{
			while (serialPort.IsOpen == false)
			{
				try
				{
					serialPort.Open();
				}
				catch (UnauthorizedAccessException)
				{

				}
			}
		}

		[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "its ok here")]
		public static string MockReadFunction(SerialPort port) => string.Empty;

		[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "its ok here")]
		public static void MockWriteFunction(SerialPort port, string payload) { }
	}
}
