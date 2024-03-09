let isTalkingShit = false;
let whoseVoice = 'Silence - Silent';

function speakWithAllVoices() {
  if (!isTalkingShit) {
    let utteranceText = 'Hello, world!'; // Text to be spoken
    let voices = window.speechSynthesis.getVoices();
    addHelloText();
    voices.forEach(voice => {
      whoseVoice = voices.indexOf(voice) == 0 ? voice.name : voices[voices.indexOf(voice) - 1].name;
      let ssu = new SpeechSynthesisUtterance(utteranceText);
      ssu.voice = voice; // Set the voice to the current voice in the loop
      ssu.lang = voice.lang; // Set the language code
      ssu.onend = function (event) {
        whoseVoice = voices.indexOf(voice) > 0 ? (voices.indexOf(voice) == voices.length - 1 ? voices[voices.indexOf(voice)].name : (whoseVoice == voice.name ? voices[voices.indexOf(voice) + 1].name : (voices.length - 1 == voices[voices.indexOf(voice)] ? voice.name : voices[voices.indexOf(voice)+1].name))) : voices[voices.indexOf(voice)+1].name;
        document.title = voices[voices.indexOf(voice)].name;
        console.log(whoseVoice);
        document.getElementById("hellofrom").textContent = ('Speaking with voice:' + whoseVoice);
        isTalkingShit = false;
      };
      document.getElementById("hellofrom").textContent = ('Speaking with voice:', voice.name);
      console.log('Speaking in voice:', voice.name);
      window.speechSynthesis.speak(ssu);
      isTalkingShit = true;
    });
    addHelloText();
  }
  addHelloText();
};

function addHelloText() {
  if (!document.getElementById("hellofrom")) {
    let hey = document.createElement('h1');
    hey.id = "hellofrom";
    hey.textContent = "Hey There!";
    document.getElementsByTagName("body")[0].appendChild(hey);
  }
  else {
    document.getElementById("hellofrom").textContent = "Ahoi!";
  }
};

// It's important to call this function after the 'voiceschanged' event fires
// to ensure all voices are loaded, especially on some browsers.
window.speechSynthesis.onvoiceschanged = function () {
  addHelloText();
  if (!isTalkingShit) {
    document.getElementById("hellofrom").textContent = ('Hello!');
    console.log(whoseVoice);
  } else { isTalkingShit = false; };
  document.getElementById("hellofrom").textContent = ('Hello!');
  speakWithAllVoices();
};

function helloButtonClick() {
  addHelloText();
  isTalkingShit = false;
  speakWithAllVoices();
};