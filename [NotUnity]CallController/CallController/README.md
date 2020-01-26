## Call Controller
Для того что бы скомпилировать данный продукт необходимо установить некоторые библиотеки и инструменты:
* ESP8266WebServer.h
* Adafruit_SSD1306.h
* Adafruit_GFX.h

### Начало работы
Для начала работы с платой необходимо выполнить следующие действия:

Запустить среду разработки (Arduino) и пройти в Файл->Настройки:
![Вид окна](https://habrastorage.org/files/040/c7e/70d/040c7e70d101439685fb5169f3bc1aa5.png)

Вставить ссылку в поле «Дополнительные ссылки для Менеджера плат:» и нажать «OK».
`https://arduino.esp8266.com/stable/package_esp8266com_index.json`
(Эту ссылку также можно получить на странице проекта [Arduino core for ESP8266 WiFi chip](https://github.com/esp8266/Arduino#available-versions))

Потом пройти в Инструменты->Плата:->Менеджер плат...:
![Вид окна](https://habrastorage.org/files/a12/730/7f9/a127307f9f274a87ab864fdc069f2041.png)

Появится окно менеджера плат, его надо пролистать до самого низа,, и если всё сделано правильно будет что-то подобно этому:
![Вид окна](https://habrastorage.org/files/253/7f8/af8/2537f8af8a2d4dd8a9fea7c0ee1ed34f.png)

Наводим курсор на  "esp8266 by ESP8266 Community" после этого, появляется кнопка «Установка», выбираете нужную версию, на сегодняшний день последняя это 2.1.0. Среда разработки закачает нужные ей файлы (~150 мегабайт) и напротив надписи "esp8266 by ESP8266 Community" появится «INSTALLED» то есть установлено:
![Вид окна](https://habrastorage.org/files/809/183/b65/809183b65ceb4933885263fd6e0eee58.png)

Далее надо пройти в «Инструменты» и выбрать нужный COM порт, потом ставим Upload Speed:«921600» (остальные настройки можно оставить по умолчанию, тем самым у меня были следующие настройки):

![Вид меню](https://raw.githubusercontent.com/pechhenka/InteractiveStand/Extras/PlateSettings.png)

Если вам будет необходима отладка ПО в мониторе порта выставьте скорость 74880 бод и «NL & CR»:
![Вид окна](https://habrastorage.org/files/83b/d9d/297/83bd9d2977d948158c56f675c5c8be1b.png)

Поздравляю! На этом этапе плата должна работать и выполнять все свои функции, но без дисплея и без доступа к роутеру невозможно узнать IP-адрес платы. Хотя если у вас нет в этом нужды, то всё готово.

### Дисплей
Для "оживления" дисплея остаётся только установить `Adafruit_SSD1306` и `Adafruit_GFX`

Предпочтительным способом установки является использование Arduino IDE Library Manager.
![Вид окна](https://raw.githubusercontent.com/pechhenka/InteractiveStand/Extras/EnterArduinoIDELibraryManager.png)

В поиске открывшегося окна вводим `Adafruit_SSD1306` и жмём установить, аналогично делаем для `Adafruit_GFX`:
![Вид меню](https://raw.githubusercontent.com/pechhenka/InteractiveStand/Extras/Adafruit_SSD1306.png)
![Вид меню](https://raw.githubusercontent.com/pechhenka/InteractiveStand/Extras/Adafruit_GFX.png)

Поздравляю x2!! Остаётся только раскомментировать следующую строку в программе:
```C++
//#define Display //раскомментировать для включения дисплея
```
И после загрузки программы, Call Controller покажет на дисплее свой IP-адрес.

### Использованные источники

* https://habr.com/ru/post/371853/
* https://github.com/adafruit/Adafruit_SSD1306