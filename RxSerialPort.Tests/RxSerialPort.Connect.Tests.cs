namespace System.IO.Ports.Tests
{
	using System;
	using Xunit;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
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

			Assert.Throws<ArgumentNullException>(() => serialPort!.Connect());
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

			Assert.Throws<ArgumentNullException>(() => RxSerialPort.Connect(serialPortFactory!));
		}

#if TEST_WITH_REAL_PORTS
		[Fact]
		public void Connect_MultipleSubscriptions()
		{
			var portObservable = RxSerialPort.Connect(SerialPort.GetPortNames()[0]);

			using var sub1 = portObservable.Subscribe();
			using var sub2 = portObservable.Subscribe();

			Assert.NotNull(sub1);
			Assert.NotNull(sub2);
		}
#endif
	}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
}