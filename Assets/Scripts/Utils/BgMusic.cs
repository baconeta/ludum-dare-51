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

        public void StartTrackManually()
        {
            StopAllTracks();
            StartCoroutine(PlayBGMLooped(track, true));
        }

        private IEnumerator PlayBGMLooped(MusicTrack musicTrack, bool loop)
        {
            _loopableAudioSource.QueueClip(musicTrack.audioClip);
            double startTime = AudioSettings.dspTime + audioSourceBufferTime;
            Debug.Log("start" + startTime);
            double endTime = startTime + musicTrack.loopStart + musicTrack.loopTime;
            Debug.Log("end" + endTime);
            _loopableAudioSource.PlayScheduled(startTime, endTime);
            Debug.Log("Start track loop from start");

            while (loop)
            {
                if (AudioSettings.dspTime > endTime - audioSourceBufferTime)
                {
                    Debug.Log("Set up next loop");
                    startTime = endTime;
                    endTime = startTime + track.loopTime;
                    _loopableAudioSource.PlayScheduled(startTime, endTime, track.loopStart);
                }

                yield return null;
            }
        }

        public void StopAllTracks()
        {
            _loopableAudioSource.Stop();
            StopAllCoroutines();
        }
    }
}