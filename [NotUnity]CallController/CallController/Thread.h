/*
  Based on https://github.com/ivanseidel/ArduinoThread
  Created by Ivan Seidel Gomes, March, 2013.
  Released into the public domain.
*/

#ifndef Thread_h
#define Thread_h

#include <Arduino.h>

class Thread{
protected:
  unsigned long interval;
  unsigned long last_run;
  unsigned long _cached_next_run;

  void runned();

  void (*_onRun)(void);   

public:
  Thread(void (*callback)(void) = NULL, unsigned long _interval = 0);
  void setInterval(unsigned long _interval);
  bool shouldRun();
  void onRun(void (*callback)(void));
  void run();
};

#endif
