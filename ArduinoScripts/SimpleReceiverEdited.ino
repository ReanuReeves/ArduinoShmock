#include "PinDefinitionsAndMore.h"
#include <IRremote.hpp> // Include the IRremote library

int analog_input = A0; // Analog output of the flame sensor
int digital_input = 3; // Digital output of the flame sensor

void setup() {
    Serial.begin(115200);
    while (!Serial)
        ; // Wait for Serial to become available. Is optimized away for some cores.

    // Initialize IR receiver
    Serial.println(F("START " __FILE__ " from " __DATE__ "\r\nUsing library version " VERSION_IRREMOTE));
    IrReceiver.begin(IR_RECEIVE_PIN, ENABLE_LED_FEEDBACK);

    Serial.print(F("Ready to receive IR signals of protocols: "));
    printActiveIRProtocols(&Serial);
    Serial.println(F("at pin " STR(IR_RECEIVE_PIN)));

    // Initialize flame sensor pins
    pinMode(analog_input, INPUT);
    pinMode(digital_input, INPUT);
    Serial.println("KY-026 Flame detection initialized");
}

void loop() {
    // Check for IR signals
    if (IrReceiver.decode()) {
        if (IrReceiver.decodedIRData.protocol == UNKNOWN) {
            IrReceiver.printIRResultRawFormatted(&Serial, true);
            IrReceiver.resume(); // Preserve raw data for printing
        } else {
            IrReceiver.resume(); // Enable receiving of the next IR frame
        }

        if (IrReceiver.decodedIRData.command != NULL) {
            Serial.print("IR Command Received: 0x");
            Serial.println(IrReceiver.decodedIRData.command, HEX);
        }
    }

    // Check flame sensor digital output
    int digital_value = digitalRead(digital_input);
    if (digital_value == HIGH) {
        Serial.println("FIRE_DETECTED");
    }

    delay(100); // Small delay to avoid flooding the serial output
}