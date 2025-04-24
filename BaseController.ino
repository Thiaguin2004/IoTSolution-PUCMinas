#include <WiFi.h>
#include <HTTPClient.h>
#include <OneWire.h>
#include <DallasTemperature.h>

// Configuração WiFi
const char* ssid = "2G_NETVIRTUA100";
const char* password = "25252525Aa";

// DS18B20
#define ONE_WIRE_BUS 17
OneWire oneWire(ONE_WIRE_BUS);
DallasTemperature sensors(&oneWire);

// Configurações da API
const char* apiBaseUrl = "https://central-cute-lynx.ngrok-free.app/api/leituras";
const int idDispositivo = 1;  // Substitua com o ID real
const int idSensor = 1;       // Substitua com o ID real

// Timer
unsigned long lastTime = 0;
unsigned long timerDelay = 10000;

void setup() {
  Serial.begin(115200);
  sensors.begin();
  connectToWiFi();
}

void loop() {
  // Verifica WiFi e reconecta se necessário
  if (WiFi.status() != WL_CONNECTED) {
    connectToWiFi();
  }

  // Envia a cada 10 segundos
  if ((millis() - lastTime) > timerDelay) {
    sensors.requestTemperatures();
    float temperatura = sensors.getTempCByIndex(0);

  if (temperatura != DEVICE_DISCONNECTED_C) {
    Serial.print("Temperatura: ");
    Serial.println(temperatura);

    enviarPost(String(temperatura, 2));  // converte float para String com 2 casas decimais
  } else {
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
    Serial.println("\nWiFi conectado!");
    Serial.print("IP: ");
    Serial.println(WiFi.localIP());
  } else {
    Serial.println("\nFalha na conexão WiFi.");
  }
}

void enviarPost(String temperatura) {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;

    // Monta URL com parâmetros
    String url = String(apiBaseUrl) + "?text=" + temperatura +
                 "&dispositivo=" + idDispositivo +
                 "&sensor=" + idSensor;

    Serial.println("Enviando para: " + url);

    http.begin(url);
    int httpResponseCode = http.POST("");  // corpo vazio

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
