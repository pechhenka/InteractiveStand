#include "Thread.h"

Thread::Thread(void (*callback)(void), unsigned long _interval) {
  onRun(callback);
  last_run = millis();
  setInterval(_interval);
};

void Thread::setInterval(unsigned long _interval) {
  interval = _interval;
}

bool Thread::shouldRun() {
  return (millis() - last_run) >= interval;
}

void Thread::onRun(void (*callback)(void)) {
  _onRun = callback;
}

void Thread::run() {
  if (_onRun != NULL)
    _onRun();

  last_run = millis();
}
