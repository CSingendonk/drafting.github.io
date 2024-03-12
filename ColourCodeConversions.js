/**
* @CSingendonk
* @param Array<number>\[0,0,0\]
* @returns {HexColourValueCode}
* @example fromRGBtoHexColour([255,122,0]): '#FF7A00'
*/
function fromRGBtoHexColour(value = []) {
    const hexDigits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
    function decimalToHex(decimalValue) {
        const clampedValue = Math.max(0, Math.min(decimalValue, 255));
        const quotient = Math.floor(clampedValue / 16);
        const remainder = clampedValue % 16;
        const hexQuotient = toHexDigit(quotient);
        const hexRemainder = toHexDigit(remainder);
        return hexQuotient + hexRemainder;
    };
    function toHexDigit(decimal) {
        if (decimal > hexDigits.length - 1 || !parseInt(decimal) || decimal < 0){
            return '0'
        }
        return hexDigits[decimal];
    };
    function colourWeightsToHex(weight) {
        if (weight > 255 || weight < 0 || !parseInt(weight)){
            return '00';
        }
        return decimalToHex(weight);
    };
    if (value) {
        let red = value[0];
        let green = value[1];
        let blue = value[2];
        let r = red > 0 ? true : false, g = green > 0 ? true : false, b = blue > 0 ? true : false;
        if (r) { red = colourWeightsToHex(value[0]); } else { red = '00'; }
        if (g) { green = colourWeightsToHex(value[1]); } else { green = '00'; }
        if (b) { blue = colourWeightsToHex(value[2]); } else { blue = '00'; }
        const rgbaHex = `#${red}${green}${blue}`;
        return rgbaHex;
    }
    else {
        return '#f00ff0';
    }
}
