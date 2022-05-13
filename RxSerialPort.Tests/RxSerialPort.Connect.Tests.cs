namespace System.IO.Ports.Tests
{
	using System;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using Xunit;

	public class RxSerialPort_Connect_Tests
	{
		[Fact]
		public void ConnectAndRead_Extension_Creation_Normal()
		{
			var serialPort = new SerialPort();
			Func<SerialPort, string> readFunction = port => string.Empty;

			var serialPortObservable = serialPort.Connect(readFunction);

			Assert.NotNull(serialPortObservable);
		}

		[Fact]
		public void ConnectAndRead_Extension_Creation_PortNull()
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
		[Fact]
		public async Task Connect_Extension_ReceivingData()
		{
			string testLine = "Hello Port";
			bool receivedCalled = false;
			using var receivingPort = new SerialPort(SerialPort.GetPortNames()[0]);
			using var sendingPort = new SerialPort(SerialPort.GetPortNames()[1]);

			receivingPort.OpenSafelyForTest();
			sendingPort.OpenSafelyForTest();

			using var sub = receivingPort.Connect()
				.WatchDataReceived()
				.Subscribe(_ => receivedCalled = true);

			sendingPort.WriteLine(testLine);

			await Task.Delay(500);

			Assert.True(receivedCalled);
		}

		[Fact]
		public void Connect_Extension_DisposingEvent()
		{
			bool disposeEventCalled = false;
			var receivingPort = new SerialPort("IrrelevantPortName");

			using var sub = receivingPort.Connect()
				.WatchDisposing()
				.Subscribe(_ => disposeEventCalled = true);

			receivingPort.Dispose();

			Assert.True(disposeEventCalled);
		}

		[Fact()]
		public async Task Connect_ReceivingData_Normal()
		{
			string testLine = "Hello Port";
			bool receivedCalled = false;
			using var sendingPort = new SerialPort(SerialPort.GetPortNames()[1]);
			sendingPort.OpenSafelyForTest();

			using var sub = RxSerialPort.Connect(() => new SerialPort(SerialPort.GetPortNames()[0]))
				.WatchDataReceived()
				.Subscribe(_ => receivedCalled = true);

			sendingPort.WriteLine(testLine);

			await Task.Delay(500);

			Assert.True(receivedCalled);
		}

		[Fact()]
		public async Task ConnectAndRead_Extension_ReceivingData_Normal()
		{
			string testLine = "Hello Port";
			bool receivedCalled = false;
			using var receivingPort = new SerialPort(SerialPort.GetPortNames()[0]);
			using var sendingPort = new SerialPort(SerialPort.GetPortNames()[1]);

			receivingPort.OpenSafelyForTest();
			sendingPort.OpenSafelyForTest();

			using var sub = receivingPort.Connect(port => port.ReadLine())
				.WatchData()
				.Subscribe(data =>
				{
					Assert.Equal(testLine, data);
					receivedCalled = true;
				});

			sendingPort.WriteLine(testLine);

			await Task.Delay(500);

			Assert.True(receivedCalled);
		}

		[Fact()]
		public async Task ConnectAndRead_ReceivingData_Normal()
		{
			string testLine = "Hello Port";
			bool receivedCalled = false;
			using var sendingPort = new SerialPort(SerialPort.GetPortNames()[1]);
			sendingPort.OpenSafelyForTest();

			using var sub = RxSerialPort.Connect(
					() => new SerialPort(SerialPort.GetPortNames()[0]),
					port => port.ReadLine())
				.WatchData()
				.Subscribe(data =>
				{
					Assert.Equal(testLine, data);
					receivedCalled = true;
				});

			sendingPort.WriteLine(testLine);

			await Task.Delay(500);

			Assert.True(receivedCalled);
		}
#endif
	}
}