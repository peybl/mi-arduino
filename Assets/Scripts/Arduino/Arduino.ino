//Schematics for the display: http://www.instructables.com/id/Temperature-Displayed-on-4-Digit-7-segment-common/
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
unsigned long starttime = 0;//for measureing the score
unsigned long endtime = 0;//for measureing the score
int score = 0;//for measureing the score

//init the 4 digits on the diplay
int digitBuffer[4] = {
  0
};
//the 10 possabilities, which each digit can display
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



void setup()
{
  Serial.begin(9600);
  //init all pins
  pinMode(photoresistorPin, INPUT);
  pinMode(buzzerPin, OUTPUT);
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
    if ('a' == receivedChar)//unity sends a, to make a crashing sound
    {
      playCrashSound();
    }
    else if ('b' == receivedChar && !gameover)//unity sends b, when sound is over and score needs to be displayed
    {
      endtime = millis();
      //-3 because of startsequence
      score = ((endtime / 1000 - starttime / 1000)) - 6;
      gameover = true;
    }
    else if ('c' == receivedChar)//unity sends c, when game is started
    {
      starttime = millis();
      gameover = false;
      resetDisplay();
    }
  }

  if (!gameover) {
    //calculate light intensity on photoresistor
    long light = analogRead(photoresistorPin); //read from photoresistor

    //calculate the distance between hand and ultrasonic sensor
    long distance = measureDistance();

    //send values to unity
    Serial.println(String(light) + String(" ") + String(distance));

    //delay for better performance
    delay(100);

  } else {
    //display score in loop - each digit per iteration
    int tempscore = score;
    digitBuffer[3] = tempscore % 10;
    tempscore = tempscore / 10;
    digitBuffer[2] = tempscore % 10;
    tempscore = tempscore / 10;
    digitBuffer[1] = tempscore % 10;
    tempscore = tempscore / 10;
    digitBuffer[0] = tempscore % 10;
    updateDisplay();
  }

}

//makes a small buzzering sound, when game is over
void playCrashSound()
{
  digitalWrite(buzzerPin, HIGH);
  delay(1);//wait for 1ms
  digitalWrite(buzzerPin, LOW);
  delay(1);//wait for 1ms
}

//writes the score on display
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

//resetting display when game starts
void resetDisplay() {
  digitScan = 3;
  digitBuffer[0] = 0;
  updateDisplay();
}

//measures distances between hand and ultrasonic sensor
long measureDistance() {
  long duration = 0, cm = 0;

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

  // The speed of sound is 340 m/s or 29 microseconds per centimeter.
  // The ping travels out and back, so to find the distance of the
  // object we take half of the distance travelled.
  return duration / 29 / 2;
}


