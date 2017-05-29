  //ultrasonic code from: https://www.tautvidas.com/blog/2012/08/distance-sensing-with-ultrasonic-sensor-and-arduino/
//pins
const int photoresistorPin = 0; //photoresistor
const int echoPin = 2; //ultrasonic echo
const int trigPin = 3; //ultrasonic trigger
const int digitPins[4] = {7, 6, 5, 4}; //datapins for segment
const int buzzerPin = 8; //passive buzzer
const int clockPin = 11;    //74HC595 Pin 11
const int latchPin = 12;    //74HC595 Pin 12
const int dataPin = 13;     //74HC595 Pin 14

//variables for game
int digitScan = 0; //for the current digit
bool sound = false; //playing no sound
bool gameover = false; //to check wenn to activate the segment display
const byte digit[10] =      //seven segment digits in bits
{
  B00111111, //0
  B00000110, //1
  B01011011, //2
  B01001111, //3
  B01100110, //4
  B01101101, //5
  B01111101, //6
  B00000111, //7
  B01111111, //8
  B01101111  //9
};

int digitBuffer[4] = {
  0
};

String score = "";

void setup()
{
  Serial.begin(9600);
  pinMode(photoresistorPin, INPUT); //photoresistor
  pinMode(buzzerPin, OUTPUT); //initialize the buzzer pin as an output
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  for (int i = 0; i < 4; i++)
  {
    pinMode(digitPins[i], OUTPUT);
  }
  pinMode(latchPin, OUTPUT);
  pinMode(clockPin, OUTPUT);
  pinMode(dataPin, OUTPUT);

  resetDisplay();
}

void loop()
{

  //receiving data from unity
  if (Serial.available() > 0) {
    // read the incoming byte:
    char receivedChar = Serial.read();
    if ('a' == receivedChar)
    {
      buzz_crash();
    }
    else if ('b' == receivedChar)
    {
      gameover = true;
    }
      else if ('c' == receivedChar)
    {
      gameover = false;
      resetDisplay();
    }
  }
  //photoresistor
  long photoresistor = analogRead(photoresistorPin); //read from photoresistor

  long duration = 0, cm = 0;

  if (gameover) {
    digitBuffer[3] = 3;
    digitBuffer[2] = 2;
    digitBuffer[1] = 1;
    digitBuffer[0] = 0;
    updateDisplay();
  } else {
    //ultrasonic sensor
    // establish variables for duration of the ping,
    // and the distance result in inches and centimeters:
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
  }

  Serial.println(String(photoresistor) + String(" ") + String(cm));

  if(!gameover){ //just for photosonic sensor
    delay(100);
  }

}

void buzz_crash()
{
  
    digitalWrite(buzzerPin, HIGH);
    delay(1);//wait for 1ms
    digitalWrite(buzzerPin, LOW);
    delay(1);//wait for 1ms

}

long microsecondsToCentimeters(long microseconds)
{
  // The speed of sound is 340 m/s or 29 microseconds per centimeter.
  // The ping travels out and back, so to find the distance of the
  // object we take half of the distance travelled.
  return microseconds / 29 / 2;
}

//writes the temperature on display
void updateDisplay() {
  for (byte j = 0; j < 4; j++) {
    digitalWrite(digitPins[j], HIGH); // Turns the display off. Changed to HIGH
  }
  digitalWrite(latchPin, LOW);
  shiftOut(dataPin, clockPin, MSBFIRST, B00000000);
  digitalWrite(latchPin, HIGH);

  delayMicroseconds(2);

  digitalWrite(digitPins[digitScan], LOW); //Changed to LOW for turning the leds on.

  digitalWrite(latchPin, LOW);
  shiftOut(dataPin, clockPin, MSBFIRST, digit[digitBuffer[digitScan]]);

  digitalWrite(latchPin, HIGH);

  digitScan++;
  if (digitScan > 3) digitScan = 0;
}

void resetDisplay(){
    digitBuffer[3] = 0;
    updateDisplay();
    digitBuffer[2] = 0;
    updateDisplay();
    digitBuffer[1] = 0;
    updateDisplay();
    digitBuffer[0] = 0;
    updateDisplay();
}


