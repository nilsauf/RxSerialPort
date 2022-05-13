namespace System.IO.Ports.Tests
{
	using System;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
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