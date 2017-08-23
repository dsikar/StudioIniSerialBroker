/*
 * Studio Ini Arduino code
 * for Serial Broker
 */

String SBString;

void setup() {
  Serial.begin(9600);      // open the serial port at 9600 bps: 
  Serial.println("Starting up...");   
}

void loop() {  
  CheckSerial();
  delay(1000);
}

void CheckSerial() {
  while (Serial.available()) {
    delay(3);  //delay to allow buffer to fill 
    if (Serial.available() >0) {
      char c = Serial.read();  //gets one byte from serial buffer
      SBString += c; //makes the string readString
    } 
  }

  if(SBString.length() > 0) {
    Serial.print("Received data length() = ");
    Serial.println(SBString.length());
    SBString = "";
  }
}
