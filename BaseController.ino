#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <HTTPClient.h>
#include <OneWire.h>
#include <DallasTemperature.h>
#include <ArduinoOTA.h>

// Configuração WiFi
const char* ssid = "2G_NETVIRTUA100";
const char* password = "25252525Aa";

// DS18B20
#define ONE_WIRE_BUS 17
OneWire oneWire(ONE_WIRE_BUS);
DallasTemperature sensors(&oneWire);

// Configurações da API
const char* apiBaseUrl = "https://central-cute-lynx.ngrok-free.app/api/leituras";
const int idDispositivo = 1;
const int idSensor = 1;

// Timer
unsigned long lastTime = 0;
unsigned long timerDelay = 10000;

void setup() {
  Serial.begin(115200);
  sensors.begin();
  connectToWiFi();

  // Inicializa OTA
  ArduinoOTA
    .onStart([]() {
      String type = (ArduinoOTA.getCommand() == U_FLASH) ? "sketch" : "filesystem";
      Serial.println("Iniciando atualização OTA: " + type);
    })
    .onEnd([]() {
      Serial.println("\nFinalizando OTA.");
      delay(2000);
      ESP.restart();
    })
    .onProgress([](unsigned int progress, unsigned int total) {
      Serial.printf("Progresso: %u%%\r", (progress / (total / 100)));
    })
    .onError([](ota_error_t error) {
      Serial.printf("Erro OTA[%u]: ", error);
      if (error == OTA_AUTH_ERROR) Serial.println("Falha de autenticação");
      else if (error == OTA_BEGIN_ERROR) Serial.println("Falha ao iniciar");
      else if (error == OTA_CONNECT_ERROR) Serial.println("Falha de conexão");
      else if (error == OTA_RECEIVE_ERROR) Serial.println("Falha ao receber");
      else if (error == OTA_END_ERROR) Serial.println("Falha ao finalizar");
    });

  ArduinoOTA.setHostname("meu_esp32");
  ArduinoOTA.begin();
}

void loop() {
  ArduinoOTA.handle();

  if (WiFi.status() != WL_CONNECTED) {
    connectToWiFi();
  }

  if ((millis() - lastTime) > timerDelay) {
    sensors.requestTemperatures();
    float temperatura = sensors.getTempCByIndex(0);

    if (temperatura != DEVICE_DISCONNECTED_C) {
      Serial.print("Temperatura: ");
      Serial.println(temperatura);
      enviarPost(String(temperatura, 2));
    } else {
      enviarLog("Erro: Sensor desconectado!");
      Serial.println("Erro: Sensor desconectado.");
    }

    lastTime = millis();
  }
}

void connectToWiFi() {
  Serial.print("Conectando ao WiFi");
  WiFi.begin(ssid, password);
  int tentativas = 0;

  while (WiFi.status() != WL_CONNECTED && tentativas < 20) {
    delay(500);
    Serial.print(".");
    tentativas++;
  }

  if (WiFi.status() == WL_CONNECTED) {
    enviarLog("Wifi conectado!");
    Serial.println("\nWiFi conectado!");
    Serial.print("IP: ");
    Serial.println(WiFi.localIP());
  } else {
    Serial.println("\nFalha na conexão WiFi.");
  }
}

void enviarPost(String temperatura) {
  if (WiFi.status() == WL_CONNECTED) {
    WiFiClientSecure client;
    client.setInsecure(); // Ignora certificado (ideal para testes com ngrok)

    HTTPClient http;
    String url = String(apiBaseUrl) + "?text=" + urlEncode(temperatura.c_str()) +
                 "&dispositivo=" + idDispositivo +
                 "&sensor=" + idSensor;

    Serial.println("Enviando para: " + url);

    http.begin(client, url);
    http.setTimeout(10000);
    http.addHeader("Content-Type", "application/x-www-form-urlencoded");

    int httpResponseCode = http.POST("teste=1");

    if (httpResponseCode > 0) {
      Serial.print("Código de resposta: ");
      Serial.println(httpResponseCode);
      String response = http.getString();
      Serial.println("Resposta: " + response);
    } else {
      Serial.print("Erro ao enviar POST. Código: ");
      Serial.println(httpResponseCode);
    }

    http.end();
  } else {
    Serial.println("WiFi desconectado. Tentando reconectar...");
    connectToWiFi();
  }
}

void enviarLog(String log) {
  if (WiFi.status() == WL_CONNECTED) {
    WiFiClientSecure client;
    client.setInsecure(); // Ignora certificado (ideal para testes com ngrok)

    HTTPClient http;
    String url = String(apiBaseUrl) + "/logs?log=" + urlEncode(log.c_str()) +
                 "&dispositivo=" + idDispositivo +
                 "&sensor=" + idSensor;

    Serial.println("Enviando para: " + url);

    http.begin(client, url);
    http.setTimeout(10000);
    http.addHeader("Content-Type", "application/x-www-form-urlencoded");

    int httpResponseCode = http.POST("teste=1");

    if (httpResponseCode > 0) {
      Serial.print("Código de resposta: ");
      Serial.println(httpResponseCode);
      String response = http.getString();
      Serial.println("Resposta: " + response);
    } else {
      Serial.print("Erro ao enviar POST. Código: ");
      Serial.println(httpResponseCode);
    }

    http.end();
  } else {
    Serial.println("WiFi desconectado. Tentando reconectar...");
    connectToWiFi();
  }
}

String urlEncode(const char *msg) {
  String encodedMsg = "";
  char c;
  char code0;
  char code1;
  for (int i = 0; i < strlen(msg); i++) {
    c = msg[i];
    if (isalnum(c)) {
      encodedMsg += c;
    } else {
      code1 = (c & 0xf) + '0';
      if ((c & 0xf) > 9) code1 = (c & 0xf) - 10 + 'A';
      code0 = ((c >> 4) & 0xf) + '0';
      if (((c >> 4) & 0xf) > 9) code0 = ((c >> 4) & 0xf) - 10 + 'A';
      encodedMsg += '%';
      encodedMsg += code0;
      encodedMsg += code1;
    }
  }
  return encodedMsg;
}
