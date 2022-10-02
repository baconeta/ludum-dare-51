using System.Collections;
using UnityEngine;

namespace Utils
{
    public class BgMusic : MonoBehaviour
    {
        private LoopableAudioSource _loopableAudioSource;
        public float audioSourceBufferTime;
        public MusicTrack track;

        private void Start()
        {
            _loopableAudioSource = GetComponent<LoopableAudioSource>();

            StartCoroutine(PlayBGMLooped(track, true));
        }

        private IEnumerator PlayBGMLooped(MusicTrack musicTrack, bool loop)
        {
            _loopableAudioSource.QueueClip(musicTrack.audioClip);
            double startTime = AudioSettings.dspTime + audioSourceBufferTime;
            double endTime = startTime + musicTrack.loopStart + musicTrack.loopTime;
            _loopableAudioSource.PlayScheduled(startTime, endTime);

            while (loop)
            {
                if (AudioSettings.dspTime > endTime - audioSourceBufferTime)
                {
                    startTime = endTime;
                    endTime = startTime + track.loopTime;
                    _loopableAudioSource.PlayScheduled(startTime, endTime, track.loopStart);
                }

                yield return null;
            }
        }
    }
}