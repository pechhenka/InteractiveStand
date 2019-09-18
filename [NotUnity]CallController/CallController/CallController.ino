#include <ESP8266WebServer.h>
#include <queue>

//--НАСТРАИВАЕМЫЕ ПАРАМЕТРЫ--//
#define Display //раскомментировать для включения дисплея

const uint8_t pinRelay = 15;

const char* ssid = "ASUS-0360";// Имя WI-FI
const char* password = "12345678";// Пароль от этого WI-FI

IPAddress IPplate(192, 168, 1, 100); //IP адресс платы, не каждый адресс работает из-за чего принимает динамический адресс
IPAddress IPsubnet(255, 255, 255, 0); //маска подсети
IPAddress IPgateway(192, 168, 1,1); //Основной шлюз
IPAddress IPDNS(192,168,1,1); //DNS-сервер

IPAddress WhiteListIP[]={//IP адресса от которых можно получать запросы
  IPAddress(192,168,5,210),
  IPAddress(192,168,1,44),
  IPAddress(192,168,1,131),
  IPAddress(0,0,0,0) //доступ со всех адрессов (поможет для отладки)
  };
//---------------------------//

ESP8266WebServer server(80);//порт сервера (80 - HTTP) 
#ifdef Display
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#include <Thread.h>
Adafruit_SSD1306 display(128, 64, &Wire);// ширина, высота, I2C: дисплея
Thread ThreadWorkIndicator = Thread(); // поток для индикатора работы
#endif

const uint8_t pinLED_BUILTIN = 2;
const uint8_t pinTX = 1;
const uint8_t pinRX = 3;
const uint8_t pinD0   = 16;
const uint8_t pinD1   = 5;
const uint8_t pinD2   = 4;
const uint8_t pinD3   = 0;
const uint8_t pinD4   = 10;
const uint8_t pinD5   = 14;
const uint8_t pinD6   = 12;
const uint8_t pinD7   = 13;
const uint8_t pinD8   = 15;

unsigned long TaskStart = 0 ; //время принятия текущего задания
bool TasksCompleted = true; // отработаны ли уже задания

unsigned long CurrentMillis = 0; //текущее время
unsigned long LastMillis = 0;// предыдущее время

#ifdef Display
byte NumberRect = 0; // индикатор работы платы, для дисплея
#endif


struct CallTask_t
{
  char t;// p - pause, c - call
  unsigned long duration;//длитенльность действия
};
std::queue<CallTask_t> Tasks;

String cmd = "";

void setup(){
  Serial.begin(74880);
  delay(1000);
  Serial.println("Started");
  pinMode(pinLED_BUILTIN, OUTPUT);
  Call(false);

  WiFi.mode(WIFI_STA);
  WiFi.config(IPplate, IPsubnet, IPgateway, IPDNS);
  WiFi.begin(ssid, password);
  Serial.println("Configured controller completed!");

  #ifdef Display
  Wire.begin(pinD1,pinD2);
  display.begin(SSD1306_SWITCHCAPVCC, 0x3C);
  display.cp437(true);
  display.clearDisplay();
  display.setTextSize(1);
  display.setTextColor(WHITE);
  display.setCursor(6,3);
  display.print("Connecting to:");
  display.setCursor(6,14);
  display.print(ssid);
  display.setCursor(6,25);
  display.println("Requested IP:");
  display.setCursor(6,36);
  display.println(IPplate.toString());
  display.setTextSize(1);
  display.setTextColor(BLACK, WHITE);
  display.setCursor(0,56);
  display.println(" made by pechhenka.ru  ");
  display.display();
  ThreadWorkIndicator.onRun(WorkIndicator);
  ThreadWorkIndicator.setInterval(500);
  #endif

  Serial.println();
  // Wait for connection
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
     #ifdef Display
      if (ThreadWorkIndicator.shouldRun())
        ThreadWorkIndicator.run();
     #endif
  }
  
  Serial.println('\n');
  Serial.println("WI-FI connected!");
  Serial.println("Connected to: " + (String)ssid);
  Serial.println("IP adress: " + WiFi.localIP().toString());

  #ifdef Display
  display.clearDisplay();
  display.setTextSize(2);
  display.setTextColor(WHITE);
  display.setCursor(6,3);
  display.println("Call");
  display.setCursor(6,21);
  display.println("Controller");
  display.setTextSize(1);
  display.setTextColor(BLACK, WHITE);
  display.setCursor(0,56);
  display.println(" made by pechhenka.ru  ");
  display.setTextSize(1);
  display.setTextColor(WHITE); 
  display.setCursor(6,43);
  display.println(WiFi.localIP().toString());
  display.display();
  #endif

  server.on("/", handleRoot);
  server.on("/Call", handleCall);
  server.on("/call", handleCall);
  server.on("/Stop", handleStopCall);
  server.on("/Stop", handleStopCall);
  
  server.begin();
  Serial.println("Call controller started");
}

void loop(){
  CurrentMillis = millis();
    
  server.handleClient();

  if ((!TasksCompleted) && (!Tasks.empty()))
  {
    if (CurrentMillis - TaskStart>= Tasks.front().duration)
    {
      Tasks.pop();
      if (!Tasks.empty())
      {
        TaskStart = CurrentMillis;
        switch(Tasks.front().t)
        {
          case 'c':
            Call(true);
            break;
          case 'p':
            Call(false);
            break;
          default:
            break;
        }
      }
      else
      {
        Call(false);
        TasksCompleted = true;
      }
    }
  }

  #ifdef Display
  if (ThreadWorkIndicator.shouldRun())
        ThreadWorkIndicator.run();
  #endif

  LastMillis = CurrentMillis;
}

void handleStopCall()
{
  if(CheckAccess(server.client().remoteIP())) return;
  
  Call(false);
    TaskStart = 0;
    TasksCompleted = true;
    ClearTasks();
    server.send(200, "text/plain", "The current task is canceled");
}

#ifdef Display
void WorkIndicator()
{
  display.fillRect(120, 43, 6, 6, BLACK);
  if (NumberRect == 0)
    display.fillRect(120, 43, 3, 3, WHITE);
  else if (NumberRect == 1)
    display.fillRect(123, 43, 3, 3, WHITE);
  else if (NumberRect == 2)
    display.fillRect(123, 46, 3, 3, WHITE);
  else if (NumberRect == 3)
    display.fillRect(120, 46, 3, 3, WHITE);
  display.display();
  NumberRect++;
  NumberRect %= 4;  
}
#endif

void ClearTasks() // очищает очередь звонков
{
  std::queue<CallTask_t> empty;
  std::swap(Tasks,empty);
}

bool CheckAccess(IPAddress ipa)//проверка доступа к контроллеру у этого ip
{
  bool flag = true;
  for (auto x : WhiteListIP) 
    if (server.client().remoteIP() == x || x == IPAddress(0,0,0,0))
      flag = false;
      
  if (flag)
    server.send(403, "text/plain", "403\nmade by pechhenka.ru");  
    
  return flag;
}

void Call(bool state)// true - включить звонок, false - выключить звонок
{
  if (state)//On
  {
    digitalWrite(pinRelay,HIGH);
    digitalWrite(pinLED_BUILTIN, LOW); 
  }
  else//Off
  {
    digitalWrite(pinRelay,LOW);
    digitalWrite(pinLED_BUILTIN, HIGH); 
  }
}

void handleCall()//принимает запрос на создание задания
  {
  if(CheckAccess(server.client().remoteIP())) return;
    
  cmd = "";
  cmd +=("milliseconds current cycle = " + (String)CurrentMillis + "\n");
  cmd += "Number of args received:";
  cmd += server.args();    
  cmd += "\n"; 
  if (!TasksCompleted)
  {
    server.send(503, "text/plain", "503 Previous request not completed (You can cancel task, follow the link '/TaskOff')");
    return;
  }
  for (int i = 0; i < server.args(); i++) 
    {
    cmd += "argument[" + (String)i + "]";
    if(server.argName(i) == "c")
      {
      if (TasksCompleted)
        Call(true);
      TasksCompleted = false;
      
      CallTask_t ct;
      ct.t = 'c';
      ct.duration = atol(server.arg(i).c_str());
      cmd += " (processed) -> ";
      Tasks.push(ct);
      }
    else if (server.argName(i) == "p")
      {
      if (TasksCompleted)
        Call(false);
      TasksCompleted = false;
      
      CallTask_t ct;
      ct.t = 'p';
      ct.duration = atol(server.arg(i).c_str());
      cmd += " (processed) -> ";
      Tasks.push(ct);
      }
    else
      {
      cmd += " (not found) -> ";
      }

     cmd += server.argName(i) + "=";   
     cmd += server.arg(i);  
     cmd += "\n";
    }

  if(!TasksCompleted)
    TaskStart = CurrentMillis;
  
  server.send(200, "text/plain", cmd);
  }

void handleRoot() //главная страница
{
  if(CheckAccess(server.client().remoteIP())) return;
  
  cmd = "";     
  String TimeCycle = "";
  TimeCycle += (String)(CurrentMillis/86400000) + "d ";
  TimeCycle += ((String)((CurrentMillis%86400000)/3600000)) + "h ";
  TimeCycle += ((String)((CurrentMillis%3600000)/60000)) + "m ";
  TimeCycle += ((String)((CurrentMillis%60000)/1000)) + "s ";
  TimeCycle += (String)(CurrentMillis%1000) + "mil ";
  cmd += "Time current cycle = " + TimeCycle + "   It's = " + (String)CurrentMillis + " millis\n";
  cmd += "Your IP: " + server.client().remoteIP().toString() + "\n";
  cmd += "Sitemap:\n";
  cmd += "  /Call - gives the task to the controller; Arguments(can alternate): c=(call duration),p=(pause duration)\n";
  cmd += "  /Stop Disables the current task\n";

  cmd += "\n\n\nmade by pechhenka.ru";
  server.send(200, "text/plain", cmd);
}
