# Studio Ini Serial broker

Broker serial connections between IMU BNO055 and PLC.

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

## Software

* Arduino 1.8.1
* PHP
* Server configuration
* Processing (maintained by Ken, not currently versioned)

## Sensor

* Adafruit 9 DOF IMU BNO055 [overview](https://learn.adafruit.com/adafruit-bno055-absolute-orientation-sensor/overview) and [github repository](https://github.com/adafruit/Adafruit_BNO055)  

## Wifi radio

* NodeMCU - Arduino compatible ESP8266 wifi radio

## Arduino code

Two BNO055 and NodeMCU setups are used, one running **Arduino/imu_arduino_blow**, the other **Arduino/imu_arduino_hand** sketch.

# PHP code

Read (Processing script relaying to PLC) and write (NodeMCU sketches) are serviced by **cloud/studio-ini.php**. For testing (29 and 30.08.2017) php code ran on Ubuntu 16.04 running Apache, the server itself running as a virtual machine on Amazon Web Services.

# Server configuration

Access to read/write rights to web user is done by running **cloud/access_log.sh** when webserver is set up.


