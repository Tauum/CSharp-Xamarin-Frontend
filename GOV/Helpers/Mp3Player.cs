using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace GOV.Helpers
{
    public class Mp3Player
    {
        public ISimpleAudioPlayer Player { get; set; }
        public void PlaySound(string mp3) //mp3 fucntions reference this 2 line reduce
        {
            Player.Load($"{mp3}.mp3");
            Player.Play();
        }
        public Mp3Player()
        {
            Player = CrossSimpleAudioPlayer.Current;
        }
    }
}
