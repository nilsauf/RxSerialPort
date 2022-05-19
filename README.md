# RxSerialPort
An extension to the classic `System.IO.Ports.SerialPort` class, to be able to use reactive extensions. 

It enables developers to create observable streams from received data of a serial port or to write data from an observable stream to a serial port in an reactive and functional way of programming.

## Create observable stream from `SerialPort`

### Internally managed `SerialPort`

Use one of the static connect methods from `RxSerialPort`. There is a connect method for all `SerialPort` constructors. If there is a need to do additional stuff, a port factory kann be provided. 
```csharp
var eventObservable = RxSerialPort.Connect("COM1");
```

Do NOT use the serialport from the read function anywhere else! This serialport will be created and opened by the static function upon subscribing to the stream und will also be disposed if the stream is not longer needed.

### Externally managed `SerialPort`

If there is already a serial port available, just use on of the extension methods to connect to the port. 
```csharp
SerialPort serialPort = ...;

var eventObservable = serialPort.Connect()
```

In this case the serialPort needs to be opened, closed and disposed by the using code! If the port is not opend, no data events will be fired.

## Read data from `SerialPort`

After connecting to the serial port a `AndRead` extension can be used. For every read method of `SerialPort` there is a extension function. Plus a specific read funtion can be provided (eg for testing). 

```csharp
var eventsWithData = eventObservable.AndReadLine();
var eventsWithData = eventObservable.AndReadExisting();
var eventsWithData = eventObservable.AndReadTo(string valueToReadTo);
var eventsWithData = eventObservable.AndReadByte();
var eventsWithData = eventObservable.AndReadChar();

var eventsWithData = eventObservable.AndRead<TResult>(serialPort => { /* Do the reading */ });
```

## Write observable stream data to `SerialPort`

There are different extension methods to write data form an observable stream to a serialport. The `SerialPort` can be managed internally or externally. A completed and error Action can be provided optionally. Plus a specific write funtion can be provided (eg for testing). 

### Internally managed `SerialPort`

```csharp
IObservable<bytes[]> bytesToWrite = ...
IDisposable subscription = bytesToWrite.WriteTo("COM1");

IObservable<char[]> charsToWrite = ...
IDisposable subscription = charsToWrite.WriteTo("COM1");

IObservable<string> stringsToWrite = ...
IDisposable subscription = stringsToWrite.WriteTo("COM1");
IDisposable subscription = stringsToWrite.WriteLineTo("COM1");

IDisposable subscription = stringsToWrite.WriteLineTo("COM1", (serialPort, data) => { /* Do the writing */});
```

### Externally managed `SerialPort`

```csharp
SerialPort serialPort = ...

IObservable<bytes[]> bytesToWrite = ...
IDisposable subscription = bytesToWrite.WriteTo(serialPort);

IObservable<char[]> charsToWrite = ...
IDisposable subscription = charsToWrite.WriteTo(serialPort);

IObservable<string> stringsToWrite = ...
IDisposable subscription = stringsToWrite.WriteTo(serialPort);
IDisposable subscription = stringsToWrite.WriteLineTo(serialPort);

IDisposable subscription = stringsToWrite.WriteLineTo(serialPort, (serialPort, data) => { /* Do the writing */});
```

## Testing

The test project consists mainly of tests to test the correct creation of the observbales and observers. To test with real serial ports the tag `TEST_WITH_REAL_PORTS` needs to be set to true.

Virtual COM Ports are very easy to setup with [NetBurner VirtualComPort](https://www.netburner.com/virtual-com-port/). One port needs to send its data to the other port.