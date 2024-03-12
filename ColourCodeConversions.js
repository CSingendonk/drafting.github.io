
function celsiusToFahrenheit(degreesC) {
    return (degreesC * (9 / 5)) + 32;
};

function decimalToHex(decimalValue) {
    const clampedValue = Math.max(0, Math.min(decimalValue, 255));
    const quotient = Math.floor(clampedValue / 16);
    const remainder = clampedValue % 16;
    const hexQuotient = toHexDigit(quotient);
    const hexRemainder = toHexDigit(remainder);
    return hexQuotient + hexRemainder;
};

function toHexDigit(decimal) {
    // Define an array of hexadecimal digits
    const hexDigits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
    // Return the corresponding hexadecimal digit for the decimal value
    return hexDigits[decimal];
};

function colourWeightsToHex(weight) {
    return decimalToHex(weight);
};

/**
 * 
 * @param {Array};
 * @summary 'RGB() values between 0 & 255 [red,green,blue]' ;
 * @returns HexColourValueCode (example: #123ABC);
 */
function fromRGBtoHexColour(value = []) {
    if (value) {
        let red = value[0];
        let green = value[1];
        let blue = value[2];
        let r = red > 0 ? true : false, g = green > 0 ? true : false, b = blue > 0 ? true : false;
        if (r) { red = colourWeightsToHex(value[0]); } else { red = colourWeightsToHex(0); }
        if (g) { green = colourWeightsToHex(value[1]); } else { green = colourWeightsToHex(0); }
        if (b) { blue = colourWeightsToHex(value[2]); } else { blue = colourWeightsToHex(0); }
        // Construct RGBA hex color code
        const rgbaHex = `#${red}${green}${blue}`;
        return rgbaHex;
    }
    else {
        return '#f00ff0';
    }
    return '#123456'
}
