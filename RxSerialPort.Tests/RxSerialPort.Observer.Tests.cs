namespace System.IO.Ports.Tests
{
	using System;
	using Xunit;

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
	public class RxSerialPort_Observer_Tests
	{
		[Fact]
		public void CreateObserver_Creation_Normal()
		{
			var serialPortObserver = RxSerialPort_Observer.Create<string>(
				() => new SerialPort(),
				RxSerialPort_TestTools.MockWriteFunction);

			Assert.NotNull(serialPortObserver);
		}

		[Fact]
		public void CreatObserver_Creation_WriteActionNull()
		{
			Action<SerialPort, string> writeAction = null;

			Assert.Throws<ArgumentNullException>(() => RxSerialPort_Observer.Create(() => new SerialPort(), writeAction!));
		}

		[Fact]
		public void CreatObserver_Creation_FactoryNull()
		{
			Func<SerialPort> portFactory = null;

			Assert.Throws<ArgumentNullException>(
				() => RxSerialPort_Observer.Create<string>(
					portFactory!,
					RxSerialPort_TestTools.MockWriteFunction));
		}

		[Fact]
		public void CreatObserver_Creation_FactoryCreatesNull()
		{
			static SerialPort portFactory() => null;

			Assert.Throws<InvalidOperationException>(
				() => RxSerialPort_Observer.Create<string>(
					portFactory,
					RxSerialPort_TestTools.MockWriteFunction));
		}

		[Fact]
		public void AsObserver_Creation_Normal()
		{
			var serialPort = new SerialPort();

			var serialPortObserver = serialPort.AsObserver<string>(RxSerialPort_TestTools.MockWriteFunction);

			Assert.NotNull(serialPortObserver);
		}

		[Fact]
		public void AsObserver_Creation_WriteActionNull()
		{
			var serialPort = new SerialPort();
			Action<SerialPort, string> writeAction = null;

			Assert.Throws<ArgumentNullException>(() => serialPort.AsObserver(writeAction!));
		}

		[Fact]
		public void AsObserver_Creation_SerialPortNull()
		{
			SerialPort serialPort = null;

			Assert.Throws<ArgumentNullException>(
				() => serialPort!.AsObserver<string>(RxSerialPort_TestTools.MockWriteFunction));
		}

#if TEST_WITH_REAL_PORTS
		[Fact]
		public void AsObserver_Function_OnNext_Open()
		{
			string testLine = "Hello World";
			bool writeActionCalled = false;
			using var receivingPort = new SerialPort(SerialPort.GetPortNames()[0]);
			receivingPort.OpenSafelyForTest();
			var writeAction = new Action<SerialPort, string>((port, line) =>
			{
				Assert.NotNull(port);
				Assert.Same(receivingPort, port);
				Assert.Equal(line, testLine);
				writeActionCalled = true;
			});

			var serialPortObserver = receivingPort.AsObserver(writeAction);

			serialPortObserver.OnNext(testLine);

			Assert.True(writeActionCalled);
		}

#endif
	}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
}
