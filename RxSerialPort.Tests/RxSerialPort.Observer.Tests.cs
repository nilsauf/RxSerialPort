namespace System.IO.Ports.Tests
{
	using System;
	using Xunit;

	public class RxSerialPort_Observer_Tests
	{
		[Fact]
		public void CreateObserver_Creation_Normal()
		{
			Action<SerialPort, string> writeAction = (port, line) => { };

			var serialPortObserver = RxSerialPort.CreateObserver(() => new SerialPort(), writeAction);

			Assert.NotNull(serialPortObserver);
		}

		[Fact]
		public void CreatObserver_Creation_WriteActionNull()
		{
			Action<SerialPort, string> writeAction = null;

			Assert.Throws<ArgumentNullException>(() => RxSerialPort.CreateObserver(() => new SerialPort(), writeAction));
		}

		[Fact]
		public void CreatObserver_Creation_FactoryNull()
		{
			Action<SerialPort, string> writeAction = (port, line) => { };
			Func<SerialPort> portFactory = null;

			Assert.Throws<ArgumentNullException>(() => RxSerialPort.CreateObserver(portFactory, writeAction));
		}

		[Fact]
		public void CreatObserver_Creation_FactoryCreatesNull()
		{
			Action<SerialPort, string> writeAction = (port, line) => { };
			Func<SerialPort> portFactory = () => null;

			Assert.Throws<InvalidOperationException>(() => RxSerialPort.CreateObserver(portFactory, writeAction));
		}

		[Fact]
		public void AsObserver_Creation_Normal()
		{
			var serialPort = new SerialPort();
			Action<SerialPort, string> writeAction = (port, line) => { };

			var serialPortObserver = serialPort.AsObserver(writeAction);

			Assert.NotNull(serialPortObserver);
		}

		[Fact]
		public void AsObserver_Creation_WriteActionNull()
		{
			var serialPort = new SerialPort();
			Action<SerialPort, string> writeAction = null;

			Assert.Throws<ArgumentNullException>(() => serialPort.AsObserver(writeAction));
		}

		[Fact]
		public void AsObserver_Creation_SerialPortNull()
		{
			SerialPort serialPort = null;
			Action<SerialPort, string> writeAction = (port, line) => { };

			Assert.Throws<ArgumentNullException>(() => serialPort.AsObserver(writeAction));
		}

#if TEST_WITH_REAL_PORTS
		[Fact]
		public void AsObserver_Function_OnNext_Open()
		{
			string testLine = "Hello World";
			bool writeActionCalled = false;
			using var serialPort = new SerialPort(SerialPort.GetPortNames()[0]);
			serialPort.Open();
			var writeAction = new Action<SerialPort, string>((port, line) =>
			{
				Assert.NotNull(port);
				Assert.Same(serialPort, port);
				Assert.Equal(line, testLine);
				writeActionCalled = true;
			});

			var serialPortObserver = serialPort.AsObserver(writeAction);

			serialPortObserver.OnNext(testLine);

			Assert.True(writeActionCalled);
		}

#endif
	}
}
