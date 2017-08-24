# Studio Ini Serial broker

Broker serial connections between Kinect, Xbee and Arduino.

## Sensors

* Adafruit 9 DOF IMU BNO055 [overview](https://learn.adafruit.com/adafruit-bno055-absolute-orientation-sensor/overview) and [github repository](https://github.com/adafruit/Adafruit_BNO055)  
* Xbee bluetooth [modules](http://docs.digi.com/display/XBeeArduinoCodingPlatform/XBee+Arduino+Compatible+Coding+Platform) and [github repository](https://github.com/digidotcom/XBeeArduinoCodingPlatform/releases) 
* Kinect v2 [gesture recognition example](http://pterneas.com/2014/01/27/implementing-kinect-gestures/)

## C# code

Run Kinect, Xbee and Arduino comms plus algorithms, reading from Kinect and Xbee, then passing position/intensity to Arduino. Note a [patch](https://www.microsoft.com/en-us/download/details.aspx?id=45105) may need applying if COM ports are not recognised by Windows OS.

## Arduino code

Receive and parse data, pass along to Arduino compatible PLC.

## TODO

* Add Kinect BN005 and Xbee separate example code sub-directories.
* Parse string at end of line character
* Setup 3 Arduinos to simulate all Serial ports.

