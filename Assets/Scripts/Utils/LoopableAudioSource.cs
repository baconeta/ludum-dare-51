using UnityEngine;

namespace Utils
{
    public class LoopableAudioSource : MonoBehaviour
    {
        private AudioSource[] _sources;
        private AudioClip _audioClip;
        private int _current = 1;

        private void Awake()
        {
            _sources = GetComponents<AudioSource>();
        }

        public void QueueClip(AudioClip audioClip)
        {
            Stop();
            foreach (AudioSource aSource in _sources)
            {
                aSource.clip = audioClip;
            }
        }

        private void Stop()
        {
            foreach (AudioSource aSource in _sources)
            {
                aSource.Stop();
            }
        }

        private void Switch()
        {
            _current = _current == 1 ? 0 : 1;
        }

        public void PlayScheduled(double startTime, double endTime, float time = 0)
        {
            Switch();
            _sources[_current].time = time;
            _sources[_current].PlayScheduled(startTime);
            _sources[_current].SetScheduledEndTime(endTime);
        }
    }
}