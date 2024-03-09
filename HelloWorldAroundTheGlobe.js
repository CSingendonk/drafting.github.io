let isTalkingShit = false;

function speakWithAllVoices() {
    if (!document.getElementById("hellofrom")){
    let hey = document.createElement('h1');
    hey.id = "hellofrom";
    hey.textContent = "Hey There!";
    document.getElementsByTagName("body")[0].appendChild(hey);
  }
  if(!isTalkingShit) {
    const utteranceText = 'Hello, world!'; // Text to be spoken
    const voices = window.speechSynthesis.getVoices();

    voices.forEach(voice => {
      const ssu = new SpeechSynthesisUtterance(utteranceText);
      ssu.voice = voice; // Set the voice to the current voice in the loop
      ssu.lang = voice.lang; // Set the language code
      ssu.onend = function(event) {
        document.title = voice.name;
        console.log('Speech finished for voice:', voice.name);
        document.getElementById("hellofrom").textContent = ('Speaking with voice:', voice.name);
        isTalkingShit = false;
      };
      document.getElementById("hellofrom").textContent = ('Speaking with voice:', voice.name);
      console.log('Speaking in voice:', voice.name);
      window.speechSynthesis.speak(ssu);
      isTalkingShit = true;
    });
  }
}

// It's important to call this function after the 'voiceschanged' event fires
// to ensure all voices are loaded, especially on some browsers.
window.speechSynthesis.onvoiceschanged = function() {
  speakWithAllVoices();
};

