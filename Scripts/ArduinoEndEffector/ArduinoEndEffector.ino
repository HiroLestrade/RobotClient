// =============================================================================
// ArduinoEndEffector.ino
// Controlador de electroimán vía GCode por comunicación serie.
//
// Comandos GCode soportados:
//   M115          Handshake / identificación → responde "ok"
//   M42 P1 S255   Activa el electroimán  (GPIO 1 HIGH)
//   M42 P1 S0     Desactiva el electroimán (GPIO 1 LOW)
//
// ADVERTENCIA — pin 1 en Arduino Uno / Nano:
//   El pin 1 es el TX del puerto serie hardware. Si se programa la placa por
//   USB con esas variantes, conectar el electroimán al pin 1 provoca conflicto.
//   Opciones:
//     • Arduino Mega   → pin 1 es libre si se usa Serial (pines 0/1) o
//                        Serial1 (pines 18/19); verificar la variante.
//     • Arduino Leonardo / Micro → USB-CDC nativo; los pines 0/1 son libres.
//     • Cualquier placa → cambiar ELECTROMAGNET_PIN a otro pin digital libre
//                         (p.ej. 13) si hay conflicto.
// =============================================================================

const int  ELECTROMAGNET_PIN = 1;
const long BAUD_RATE         = 115200;

String inputBuffer;

// -----------------------------------------------------------------------------
void setup() {
    pinMode(ELECTROMAGNET_PIN, OUTPUT);
    digitalWrite(ELECTROMAGNET_PIN, LOW);   // apagado por defecto

    Serial.begin(BAUD_RATE);
    inputBuffer.reserve(64);
}

// -----------------------------------------------------------------------------
void loop() {
    while (Serial.available()) {
        char c = (char)Serial.read();
        if (c == '\n') {
            processCommand(inputBuffer);
            inputBuffer = "";
        } else if (c != '\r') {
            inputBuffer += c;
        }
    }
}

// -----------------------------------------------------------------------------
void processCommand(String cmd) {
    cmd.trim();
    cmd.toUpperCase();

    if (cmd == "M115") {
        // Handshake — identificación de firmware
        Serial.println("ok");

    } else if (cmd == "M42 P1 S255") {
        // Activar electroimán
        digitalWrite(ELECTROMAGNET_PIN, HIGH);
        Serial.println("ok");

    } else if (cmd == "M42 P1 S0") {
        // Desactivar electroimán
        digitalWrite(ELECTROMAGNET_PIN, LOW);
        Serial.println("ok");

    } else {
        // Comando desconocido: responde ok para mantener compatibilidad GCode
        Serial.println("ok");
    }
}
