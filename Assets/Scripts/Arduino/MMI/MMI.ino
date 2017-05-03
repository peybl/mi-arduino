//ultrasonic code from: https://www.tautvidas.com/blog/2012/08/distance-sensing-with-ultrasonic-sensor-and-arduino/
const int photoresistor_pin = 0; //photoresistor
const int trigPin = 2; //ultrasonic trigger
const int echoPin = 4; //ultrasonic echo
const int buzzer_pin = 7; //passive buzzer

void setup()
{
  Serial.begin(9600);
  pinMode(photoresistor_pin, INPUT); //photoresistor
  pinMode(buzzer_pin,OUTPUT); //initialize the buzzer pin as an output
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
}

void loop()
{
  
   //receiving data from unity
   if (Serial.available() > 0) {
         // read the incoming byte:
         char receivedChar = Serial.read();
       if('c' == receivedChar)
       {
          buzz_crash();
       }
     }
  //photoresistor
  long photoresistor = analogRead(photoresistor_pin); //read from photoresistor
  //Serial.println(photoresistor);
  //ultrasonic sensor
  // establish variables for duration of the ping, 
  // and the distance result in inches and centimeters:
  
  long duration, cm;

  // The sensor is triggered by a HIGH pulse of 10 or more microseconds.
  // Give a short LOW pulse beforehand to ensure a clean HIGH pulse:
 
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(10);
  digitalWrite(trigPin, LOW);


  // Read the signal from the sensor: a HIGH pulse whose
  // duration is the time (in microseconds) from the sending
  // of the ping to the reception of its echo off of an object.

  duration = pulseIn(echoPin, HIGH);

  // convert the time into a distance
  cm = microsecondsToCentimeters(duration);
  Serial.println(String(photoresistor) + String(" ") + String(cm));

  
  //delay
  delay(200);
}

void buzz_crash()
{
  unsigned char i;
  
  for(i=0;i<80;i++)
  {
  digitalWrite(buzzer_pin,HIGH);
  delay(1);//wait for 1ms
  digitalWrite(buzzer_pin,LOW);
  delay(1);//wait for 1ms
  }
}

long microsecondsToCentimeters(long microseconds)
{
  // The speed of sound is 340 m/s or 29 microseconds per centimeter.
  // The ping travels out and back, so to find the distance of the
  // object we take half of the distance travelled.
  return microseconds / 29 / 2;
}


