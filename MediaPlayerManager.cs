using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace Final_Project_CIS_297
{

    //UMGPT: How to implement music to play in a universal windows platform app in visual studio using c#
    public class MediaPlayerManager
    {
        private static MediaPlayer _mediaPlayer = new MediaPlayer();

        public static MediaPlayer Instance
        {
            get { return _mediaPlayer ?? (_mediaPlayer = new MediaPlayer()); }
        }

        public static void PlayMedia(StorageFile file)
        {
            _mediaPlayer.IsLoopingEnabled = true;
            _mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);
            _mediaPlayer.Play();
        }

        public static void StopMedia()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Pause();    // Pause the playback
                _mediaPlayer.Source = null;
            }
        }
    }
}
