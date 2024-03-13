const hexDigits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'];
/**
 * Converts "rgb(0, 0, 255)" to "#0000FF"
 * @param {'rgb(r,g,b)'} rgb ->  "rgb\(255,255,255\)"
 * @returns {'HexColourValueCode'}
 */
function rgbStringToHexByRegEx(rgb) {
    // Extract the integer values of the RGB components
    let [r, g, b] = rgb.match(/\d+/g).map(Number);
    // Convert each component to a hex string and pad with zeros if necessary
    return "#" + ((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1).toUpperCase();
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
    if (decimal > hexDigits.length - 1 || !parseInt(decimal) || decimal < 0) {
        return '0'
    }
    return hexDigits[decimal];
};
function colourWeightsToHex(weight) {
    if (weight > 255 || weight < 0 || !parseInt(weight)) {
        return '00';
    }
    return decimalToHex(weight);
};

/**
* Converts [255,122,0] to '#FF7A00'
* @param {Array<number>[]} \[r,g,b\] --- ex: \[255,255,255\]
* @returns {'HexColourString'}
*/
function fromRGBtoHexColourByWeight(value = []) {
    if (value) {
        let red = value[0] ? value[0] : 0;
        let green = value[1] ? value[1] : 0;
        let blue = value[2] ? value[2] : 0;
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
};
function fromRGBAtoHexColourByWeight(value = []) {
    // Convert a decimal value to hex, clamping between 0 and 255
    function decimalToHex(decimalValue) {
        const clampedValue = Math.max(0, Math.min(decimalValue, 255));
        let hex = clampedValue.toString(16);
        return hex.length === 1 ? '0' + hex : hex;
    }

    // Convert an alpha value (0-1) to hex
    function alphaToHex(alphaValue) {
        return decimalToHex(Math.round(alphaValue * 255));
    }

    // Check if RGB values are provided, default to 0 if not
    let red = value[0] !== undefined ? value[0] : 0;
    let green = value[1] !== undefined ? value[1] : 0;
    let blue = value[2] !== undefined ? value[2] : 0;
    // Handle the alpha value, defaulting to 1 (fully opaque) if not provided
    let alpha = value.length === 4 ? alphaToHex(value[3]) : 'FF';

    // Convert each component to hex and construct the RGBA hex string
    const redHex = decimalToHex(red);
    const greenHex = decimalToHex(green);
    const blueHex = decimalToHex(blue);

    return `#${redHex}${greenHex}${blueHex}${alpha}`;
}


// Converts a hex color to its RGB components
function hexToRgb(hex) {
    let r = 0, g = 0, b = 0;
    // 3 digits
    if (hex.length === 4) {
        r = parseInt(hex[1] + hex[1], 16);
        g = parseInt(hex[2] + hex[2], 16);
        b = parseInt(hex[3] + hex[3], 16);
    }
    // 6 digits
    else if (hex.length === 7) {
        r = parseInt(hex[1] + hex[2], 16);
        g = parseInt(hex[3] + hex[4], 16);
        b = parseInt(hex[5] + hex[6], 16);
    }
    return [r, g, b];
}

function inverseHexColour(hexColour = "") {
    // Remove the '#' if it's there
    hexColour = hexColour.replace('#', '');
    // Convert hex to RGB
    let r = parseInt(hexColour.substr(0, 2), 16);
    let g = parseInt(hexColour.substr(2, 2), 16);
    let b = parseInt(hexColour.substr(4, 2), 16);
    // Calculate the inverse for each component
    r = 255 - r;
    g = 255 - g;
    b = 255 - b;
    // Convert back to hex and return
    return fromRGBtoHexColourByWeight([r, g, b]);
}

// Linearly interpolates between two values
function lerp(start, end, t) {
    return start + (end - start) * t;
}

function colourRadialGradient(shape, colourCount, startColour = [], endColour = [], intermediates = []) {
    let colours = [];

    // Ensure start and end colours are set; otherwise, assign random colours
    if (startColour.length === 0 || endColour.length === 0) {
        startColour = [Math.floor(Math.random() * 256), Math.floor(Math.random() * 256), Math.floor(Math.random() * 256)];
        endColour = [Math.floor(Math.random() * 256), Math.floor(Math.random() * 256), Math.floor(Math.random() * 256)];
    }

    // Convert and add the start colour
    colours.push(fromRGBtoHexColourByWeight(startColour));

    // Check if there are predefined intermediate colours
    if (intermediates.length > 0) {
        intermediates.forEach(intermediate => {
            colours.push(fromRGBtoHexColourByWeight(intermediate));
        });
    }

    // Generate and add intermediate colours if needed
    let totalDefinedColours = 2 + intermediates.length; // Start + End + Intermediates
    let remainingColoursToGenerate = colourCount - totalDefinedColours;
    for (let i = 1; i <= remainingColoursToGenerate; i++) {
        let r = startColour[0] + (endColour[0] - startColour[0]) * (i / (colourCount - 1));
        let g = startColour[1] + (endColour[1] - startColour[1]) * (i / (colourCount - 1));
        let b = startColour[2] + (endColour[2] - startColour[2]) * (i / (colourCount - 1));
        colours.push(fromRGBtoHexColourByWeight([Math.round(r), Math.round(g), Math.round(b)]));
    }

    // Convert and add the end colour
    colours.push(fromRGBtoHexColourByWeight(endColour));

    // Return the radial-gradient CSS string
    return `radial-gradient(${shape}, ${colours.join(', ')})`;
};


function colourRadialGradientInverse(shape, colourCount, startColour = [], endColour = []) {
    if (startColour.length === 0 || endColour.length === 0) {
        startColour = [Math.floor(Math.random() * 256), Math.floor(Math.random() * 256), Math.floor(Math.random() * 256)];
        endColour = [Math.floor(Math.random() * 256), Math.floor(Math.random() * 256), Math.floor(Math.random() * 256)];
    }
    let start = [];
    let end = [];
    start.push(255 - startColour[0]);
    start.push(255 - startColour[1]);
    start.push(255 - startColour[2]);
    end.push(255 - endColour[0]);
    end.push(255 - endColour[1]);
    end.push(255 - endColour[2]);
    return colourRadialGradient(shape, colourCount, start, end);
};


function colourLinearGradient(degAngle, colourCount, startColour, endColour) {
    let colours = [];
    if (!startColour || !endColour) {
        startColour = startColour ? startColour : Math.floor(Math.random() * 255);
        endColour = endColour ? endColour : Math.floor(Math.random() * 255);
    }
    // Always start with the startColour
    colours.push(startColour);
    if (colourCount > 2) {
        // Generate intermediate colours
        for (let i = 0; i < colourCount - 2; i++) { // Adjusted loop condition
            let r = Math.floor(Math.random() * 255);
            let g = Math.floor(Math.random() * 255);
            let b = Math.floor(Math.random() * 255);
            colours.push(fromRGBtoHexColourByWeight([r, g, b]));
        }
    }
    // Always end with the endColour
    colours.push(endColour);
    let angle = Math.abs(180 - degAngle); // Gradient angle
    // Use join method to combine all colours
    return `linear-gradient(${angle}deg, ${colours.join(', ')})`
};

// Animate the transition between two radial gradients
function animateGradientTransition(elementId, startGradient, endGradient, duration) {
    const element = document.getElementById(elementId);
    const startTime = Date.now();

    // Extract gradient shape and color stops (assuming the shape is the first match)
    const shapeMatch = /radial-gradient\(([^,]+),/;
    const startShape = startGradient.match(shapeMatch)[1];
    const endShape = endGradient.match(shapeMatch)[1];
    // Default to startShape if they differ; consider additional logic if shapes need to blend

    const startColors = startGradient.match(/\#[0-9a-fA-F]{6}/g).map(hexToRgb);
    const endColors = endGradient.match(/\#[0-9a-fA-F]{6}/g).map(hexToRgb);
    let dv = 1;
    function update() {
        const currentTime = Date.now();
        const timeElapsed = currentTime - startTime;
        const t = Math.min(timeElapsed / duration, 1);
        element.style.transform = 'rotate(45deg)';
        const interpolatedColors = startColors.map((startColor, i) => {
            // Ensure we have a corresponding end color; default to startColor if not
            const endColor = endColors[i] || startColor;
            // Interpolate each color channel
            const ir = Math.round(lerp(startColor[0], endColor[0], t));
            const ig = Math.round(lerp(startColor[1], endColor[1], t));
            const ib = Math.round(lerp(startColor[2], endColor[2], t));
            let degs = duration * (255 - ir);
            element.style.rotate = `${-degs}deg`;

            return `rgb(${ir},${ig},${ib})`;
        });

        // Reconstruct the gradient string with interpolated colors
        element.style.backgroundImage = `radial-gradient(${startShape}, ${interpolatedColors.join(', ')})`;
        if (t < 1) {
            
            requestAnimationFrame(update);
        }
    }

    requestAnimationFrame(update);
};