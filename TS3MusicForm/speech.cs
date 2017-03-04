using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace TS3MusicBot
{
    class speech
    {
        private bool canSpeak = true;

        public void textToSpeech(string say) // added text to speech, in case it's ever needed. Also useful for songs
        {
            if (canSpeak == false)
            {
                canSpeak = true;
                return;
            }
            canSpeak = false;
            using (SpeechSynthesizer synth = new SpeechSynthesizer()) // easy disposable objects, to limit resource consumption
            {
                synth.SetOutputToDefaultAudioDevice(); // set output to default audio output
                if (say.Length >= 50)
                {
                    synth.Speak("Please keep the speech under 50 characters."); // say whatever wants to be said.
                    canSpeak = true;
                    return;
                }
                synth.Speak(say); // say whatever wants to be said.
                canSpeak = true;
            }
        }
    }
}
