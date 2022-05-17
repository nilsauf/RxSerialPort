namespace System.IO.Ports.Tests
{
	using System;
	using Xunit;

	public class RxSerialPort_Connect_Tests
	{
		[Fact]
		public void Connect_Extension_Creation_Normal()
		{
			var serialPort = new SerialPort();

			var serialPortObservable = serialPort.Connect();

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void Connect_Extension_Creation_PortNull()
		{
			SerialPort serialPort = null;

			Assert.Throws<ArgumentNullException>(() => serialPort.Connect());
		}

		[Fact]
		public void Connect_Creation_Normal()
		{
			static SerialPort serialPortFactory() => new SerialPort();

			var serialPortObservable = RxSerialPort.Connect(serialPortFactory);

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void Connect_Creation_PortFactoryNull()
		{
			Func<SerialPort> serialPortFactory = null;

			Assert.Throws<ArgumentNullException>(() => RxSerialPort.Connect(serialPortFactory));
		}
	}
}