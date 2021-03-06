# Studio Ini Serial Broker

Broker serial communication between IMU BNO055 and PLC.

```
                          
                                                       XXXXX
                                                       XXXX XX   XX XXXXX
                                                    XXXXX X X     XXX  X
                                                    X     XXX          XXXXX
                                 +----------->   XXXX                      X
                                 |              XX      studio-ini.php     X
                                 |              XX X XX                XXXX
                                 |                   X     XX+XXX      XX
                                 |            +--->  XXXXXX  |  X    XXX
                                 |            |              |   XXX
                                 |            |              |
                                 |            |              v
                             +---+---+    +---+---+    +-----+-----+
                             |NodeMCU|    |NodeMCU|    |PROCESSING |
                             +---+---+    +---+---+    |  SCRIPT   |
                                 ^            ^        +-----+-----+
                                 |            |              |
                                 |            |              v
                          +------+----+ +-----+-----+      +-+-+
                          |BNO055 HAND| |BNO055 BLOW|      |PLC|
                          +-----------+ +-----------+      +---+
```
## Directory Structure

* Arduino - Sketches
* DotNet/StudioIniSerialBroker - Serial broker desktop app (**not used**) - Processing sketch used instead
* KinectConsole - Command line Kinect interface (**not used**)
* cloud - PHP read/write broker and service user rights configuration script
* docs - job spec

## Software

* Arduino 1.8.1 with "NodeMCU 1.0 (ESP-12E Module)" board - added as per this [instructable](http://www.instructables.com/id/Quick-Start-to-Nodemcu-ESP8266-on-Arduino-IDE/).
* Processing (maintained by Ken, not currently versioned)

## Sensor

* Adafruit 9 DOF IMU BNO055 [overview](https://learn.adafruit.com/adafruit-bno055-absolute-orientation-sensor/overview) and [github repository](https://github.com/adafruit/Adafruit_BNO055)  

## Wifi radio

* [NodeMCU](http://www.ebay.co.uk/itm/NodeMcu-Lua-WIFI-Internet-Things-development-board-based-ESP8266-CP2102-module-/272041591472) - Arduino compatible ESP8266 wifi radio

## Arduino code

Two BNO055 and NodeMCU setups are used, one running sketch  
[**Arduino/imu_arduino_blow**](https://github.com/dsikar/StudioIniSerialBroker/blob/master/Arduino/imu_arduino_blow/imu_arduino_blow.ino),  
the other  
[**Arduino/imu_arduino_hand**](https://github.com/dsikar/StudioIniSerialBroker/blob/master/Arduino/imu_arduino_blow/imu_arduino_blow.ino).

Note server ip address and wifi network and password details must be edited in the above sketches as required.

## PHP code

Read (Processing script relaying to PLC) and write (NodeMCU sketches) are serviced by  
[**cloud/studio-ini.php**](https://github.com/dsikar/StudioIniSerialBroker/blob/master/cloud/studio-ini.php).  
For testing (29 and 30.08.2017) php code ran on Ubuntu 16.04 running Apache, the server itself running as a virtual machine on Amazon Web Services.

## Wiring

```
                                NodeMCU                                                  BNO055

                           +--------------+                                            +--------+
                           |        D1 (5)|  +-------+  SCL        3V      +--------+  | VIN    |
                           |              |                                            |        |
                           |        D2 (4)|  +-------+  SDA        GND     +--------+  | GND    |
                           |              |                                            |        |
+-------------+  +------+  | GND    GND   |  +-------+  GND        D2 (4)  +--------+  | SDA    |
|3.7V Battery |            |              |                                            |        |
+-------------+  +------+  | VIN    3V    |  +-------+  VIN        D1 (5)  +--------+  | SCL    |
                           +--------------+                                            +--------+
```

## Server configuration

read/write web user accesss is set by running  
[**cloud/access_log.sh**](https://github.com/dsikar/StudioIniSerialBroker/blob/master/cloud/access_log.sh)  
when web server is set up.

