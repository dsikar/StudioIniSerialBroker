/* NodeMCU */
#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266HTTPClient.h>
#define USE_SERIAL Serial
ESP8266WiFiMulti WiFiMulti;

/* BNO055 */
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BNO055.h>
#include <utility/imumaths.h>

/* Identifiers - Sensor number and string delimiter */
char chId = '2';
char chDelim = '|';
String strPayload;

//
/* This driver reads raw data from the BNO055

   Connections
   ===========
   Connect SCL to analog 5
   Connect SDA to analog 4
   Connect VDD to 3.3V DC
   Connect GROUND to common ground

   History
   =======
   2015/MAR/03  - First release (KTOWN)
*/

/* Set the delay between fresh samples */
#define BNO055_SAMPLERATE_DELAY_MS (1000)

Adafruit_BNO055 bno = Adafruit_BNO055(55);

/**************************************************************************/
/*
    Arduino setup function (automatically called at startup)
*/
/**************************************************************************/
void setup(void)
{
  /* NodeMCU */
  USE_SERIAL.begin(115200);  
  USE_SERIAL.println();
  USE_SERIAL.println();
  USE_SERIAL.println();  
  
  // Serial.begin(9600);
  USE_SERIAL.println("Orientation Sensor Raw Data Test"); USE_SERIAL.println("");

  /* Initialise the sensor */
  if(!bno.begin())
  {
    /* There was a problem detecting the BNO055 ... check your connections */
    USE_SERIAL.print("Ooops, no BNO055 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }

  /* Display the current temperature */
  int8_t temp = bno.getTemp();
  USE_SERIAL.print("Current Temperature: ");
  USE_SERIAL.print(temp);
  USE_SERIAL.println(" C");
  USE_SERIAL.println("");

  bno.setExtCrystalUse(true);

  USE_SERIAL.println("Calibration status values: 0=uncalibrated, 3=fully calibrated");
  
  /* Wait a bit more for good measure */
  for(uint8_t t = 4; t > 0; t--) {
    USE_SERIAL.printf("[SETUP] WAIT %d...\n", t);
    USE_SERIAL.flush();
    delay(1000);
  }

  /* TODO set network and password as defines */
  WiFiMulti.addAP("Makerversity_2G", "mak3rv3rs1ty");
}

/**************************************************************************/
/*
    Arduino loop function, called once 'setup' is complete (your own code
    should go here)
*/
/**************************************************************************/
void loop(void)
{
  /* Get a new sensor event */
  sensors_event_t event;
  bno.getEvent(&event);
  
  // init
  strPayload = chId;
  strPayload += chDelim;

  strPayload += "OX:";
  strPayload += event.orientation.x;
  strPayload += chDelim;  
  strPayload += "OY:";
  strPayload += event.orientation.y;
  strPayload += chDelim;
  strPayload += "OZ:";
  strPayload += event.orientation.z;
  
  USE_SERIAL.println(strPayload);
  myHttpReq(strPayload);

  delay(BNO055_SAMPLERATE_DELAY_MS);
}

void myHttpReq(String strPayload)
{
  // wait for WiFi connection
  if((WiFiMulti.run() == WL_CONNECTED)) {
  
      HTTPClient http;
  
      USE_SERIAL.print("[HTTP] begin...\n");
      // configure target server and url

      /* TODO server ip as define */
      /* Hardcoded sensor id as code has forked */
      String httpReq = "http://54.76.187.224/studio-ini.php?action=w&sensor=2&payload="+ strPayload;

      USE_SERIAL.println("*** Sending payload ***\n");
      USE_SERIAL.println(httpReq);
      http.begin(httpReq);
      
      USE_SERIAL.print("[HTTP] GET...\n");
      // start connection and send HTTP header
      int httpCode = http.GET();
  
      // httpCode will be negative on error
      if(httpCode > 0) {
          // HTTP header has been send and Server response header has been handled
          USE_SERIAL.printf("[HTTP] GET... code: %d\n", httpCode);
  
          // file found at server
          if(httpCode == HTTP_CODE_OK) {
              String payload = http.getString();
              USE_SERIAL.println(payload);
          }
      } else {
          USE_SERIAL.printf("[HTTP] GET... failed, error: %s\n", http.errorToString(httpCode).c_str());
      }
  
      http.end();
  }  
}

