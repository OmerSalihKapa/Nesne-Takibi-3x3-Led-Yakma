#include<Servo.h>
Servo motorsag;
Servo motorsol;
int aci=0;
int aci1=90;
int aci2=180;
int nevsag;
int nevsol;
int pozsag=90;
int pozsol=90;
void setup() {
 motorsag.attach(3);

 motorsol.attach(4);
 Serial.begin(9600); 
}
char O;
void loop() {

 if (Serial.available())
 {
  O = Serial.read();

  if (O=='1')
  {
  nevsag=aci;
  pozsag=nevsag;
  motorsag.write(nevsag);
  
  }
  if (O=='2')
  {
  nevsag=aci1;
  pozsag=nevsag;
  motorsag.write(nevsag);
  }
  if (O=='3')
  {
  nevsag=aci2;
  pozsag=nevsag;
  motorsag.write(nevsag);
  }
  if (O=='6')
  {
  nevsol=aci1;
  pozsol=nevsol;
  motorsol.write(nevsol);
  }
  if (O=='5')
  {
  nevsol=aci2;
  pozsol=nevsol;
  motorsol.write(nevsol);
  }
  if (O=='4')
  {
  nevsol=aci;
  pozsol=nevsol;
  motorsol.write(nevsol);
  }
  
 }

}
