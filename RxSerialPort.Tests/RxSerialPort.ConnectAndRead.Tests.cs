namespace System.IO.Ports.Tests
{
	using System;
	using Xunit;

	public class RxSerialPort_ConnectAndRead_Tests
	{
		[Fact]
		public void ConnectAndRead_Extension_Creation_Normal()
		{
			var serialPort = new SerialPort();

			var serialPortObservable = serialPort.Connect(RxSerialPort_TestTools.mockReadFunction);

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void ConnectAndRead_Extension_Creation_PortNull()
		{
			SerialPort serialPort = null;

			Assert.Throws<ArgumentNullException>(() => serialPort.Connect(RxSerialPort_TestTools.mockReadFunction));
		}

		[Fact]
		public void ConnectAndRead_Creation_Normal()
		{
			Func<SerialPort> serialPortFactory = () => new SerialPort();

			var serialPortObservable = RxSerialPort.Connect(serialPortFactory, RxSerialPort_TestTools.mockReadFunction);

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void ConnectAndRead_Creation_PortFactoryNull()
		{
			Func<SerialPort> serialPortFactory = null;

			Assert.Throws<ArgumentNullException>(() => RxSerialPort.Connect(serialPortFactory, RxSerialPort_TestTools.mockReadFunction));
		}

		[Fact]
		public void ConnectAndRead_Creation_PortFactoryReturnsNull()
		{
			Func<SerialPort, string> readFunction = port => string.Empty;
			Func<SerialPort> serialPortFactory = () => null;
			bool errorCalled = false;

			var sub = RxSerialPort.Connect(serialPortFactory, readFunction)
				.Subscribe(
					@event => Assert.True(false),
					ex =>
					{
						errorCalled = true;
						Assert.IsType<InvalidOperationException>(ex);
					},
					() => Assert.True(false));

			sub.Dispose();

			Assert.True(errorCalled);
		}
	}
}
