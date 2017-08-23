# Studio Ini Serial broker

Broker serial connections between Kinect, Xbee and Arduino.

## C# code

Apply [patch](https://www.microsoft.com/en-us/download/details.aspx?id=45105) to Windows OS if need be
Run all comms plus algorithms, pass position/intensity to Arduino.

## Arduino code

Receive and parse data, pass along to Arduino compatible PLC.

## TODO

* Parse string at end of line character
* Setup 3 Arduinos to simulate all Serial ports.
