﻿namespace System.IO.Ports.Tests
{
	using System;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using Xunit;

	public class RxSerialPort_Connect_Tests
	{
		[Fact]
		public void ConnectAndRead_Ex_Creation_Normal()
		{
			var serialPort = new SerialPort();
			Func<SerialPort, string> readFunction = port => string.Empty;

			var serialPortObservable = serialPort.Connect(readFunction);

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void ConnectAndRead_Ex_Creation_PortNull()
		{
			SerialPort serialPort = null;
			Func<SerialPort, string> readFunction = port => string.Empty;

			Assert.Throws<ArgumentNullException>(() => serialPort.Connect(readFunction));
		}

		[Fact]
		public void ConnectAndRead_Creation_Normal()
		{
			Func<SerialPort, string> readFunction = port => string.Empty;
			Func<SerialPort> serialPortFactory = () => new SerialPort();

			var serialPortObservable = RxSerialPort.Connect(serialPortFactory, readFunction);

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void ConnectAndRead_Creation_PortFactoryNull()
		{
			Func<SerialPort, string> readFunction = port => string.Empty;
			Func<SerialPort> serialPortFactory = null;

			Assert.Throws<ArgumentNullException>(() => RxSerialPort.Connect(serialPortFactory, readFunction));
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

		[Fact]
		public void Connect_Ex_Creation_Normal()
		{
			var serialPort = new SerialPort();

			var serialPortObservable = serialPort.Connect();

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void Connect_Ex_Creation_PortNull()
		{
			SerialPort serialPort = null;

			Assert.Throws<ArgumentNullException>(() => serialPort.Connect());
		}

		[Fact]
		public void Connect_Creation_Normal()
		{
			Func<SerialPort> serialPortFactory = () => new SerialPort();

			var serialPortObservable = RxSerialPort.Connect(serialPortFactory);

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void Connect_Creation_PortFactoryNull()
		{
			Func<SerialPort> serialPortFactory = null;

			Assert.Throws<ArgumentNullException>(() => RxSerialPort.Connect(serialPortFactory));
		}

#if TEST_WITH_REAL_PORTS
		[Fact()]
		public async Task ConnectAndRead_Ex_ReceivingData_Normal()
		{
			string testLine = "Hello Port";
			bool receivedCalled = false;
			using var serialPort = new SerialPort(SerialPort.GetPortNames()[0]);
			using var serialPort2 = new SerialPort(SerialPort.GetPortNames()[1]);

			serialPort.Open();
			serialPort2.Open();

			using var sub = serialPort.Connect(port => port.ReadExisting().Trim('\n'))
				.WatchData()
				.Subscribe(data =>
				{
					Assert.Equal(testLine, data);
					receivedCalled = true;
				});

			serialPort2.WriteLine(testLine);

			await Task.Delay(500);

			Assert.True(receivedCalled);
		}

		[Fact()]
		public async Task ConnectAndRead_ReceivingData_Normal()
		{
			string testLine = "Hello Port";
			bool receivedCalled = false;
			using var serialPort2 = new SerialPort(SerialPort.GetPortNames()[1]);
			serialPort2.Open();

			using var sub = RxSerialPort.Connect(
					() => new SerialPort(SerialPort.GetPortNames()[0]),
					port => port.ReadExisting().Trim('\n'))
				.WatchData()
				.Subscribe(data =>
				{
					Assert.Equal(testLine, data);
					receivedCalled = true;
				});

			serialPort2.WriteLine("Hello Port");

			await Task.Delay(500);

			Assert.True(receivedCalled);
		}
#endif
	}
}