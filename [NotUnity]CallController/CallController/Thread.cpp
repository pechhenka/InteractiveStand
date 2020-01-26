#include "Thread.h"

Thread::Thread(void (*callback)(void), unsigned long _interval){
  onRun(callback);
  last_run = micros();
  setInterval(_interval);
};

void Thread::runned(){
  last_run = micros();

  _cached_next_run = last_run + interval;
}

void Thread::setInterval(unsigned long _interval){
  interval = _interval;
  _cached_next_run = last_run + interval;
}

bool Thread::shouldRun(){
  return (micros() - last_run) >= interval;
}

void Thread::onRun(void (*callback)(void)){
  _onRun = callback;
}

void Thread::run(){
  if(_onRun != NULL)
    _onRun();

  runned();
}
