let isTalkingShit = false;

function loadedthepage(){
    let okay = document.createElement('button');
    okay.id = "hellofrombutton";
    okay.textContent = "Hey There! Click Me!";
    okay.onclick = speakWithAllVoices();
    okay.style.width = '250';
    okay.style.fontSize = '3rem';
    document.getElementsByTagName("body")[0].appendChild(okay);
  speakWithAllVoices();
};

if (!document.getElementById('hellofrombutton')){
    let okay = document.createElement('button');
    okay.id = "hellofrombutton";
    okay.textContent = "Hey There! Click Me!";
    okay.onclick = speakWithAllVoices();
    okay.style.width = '250';
    okay.style.fontSize = '3rem';
    document.getElementsByTagName("body")[0].appendChild(okay);
}

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
window.onload = (event) => {
  loadedthepage();
};

