namespace System.IO.Ports.Tests
{
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
	}
}
