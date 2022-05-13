namespace System.IO.Ports.Tests
{
	using System;
	using System.Threading.Tasks;
	using Xunit;

	public class RxSerialPort_Extensiosn_Watch_Tests
	{
#if TEST_WITH_REAL_PORTS
		[Fact]
		public async Task Connect_Extension_WatchDataReceived()
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
		public void Connect_Extension_WatchDisposing()
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
		public async Task Connect_WatchDataReceived()
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
		public async Task ConnectAndRead_Extension_WatchData()
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
		public async Task ConnectAndRead_WatchData()
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
