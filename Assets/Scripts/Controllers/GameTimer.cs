using UnityEngine.Events;
using UnityEngine;

public enum EWorldPhase
{
    LIGHT,
    DARK,
    DEFAULT_LAST
};

[System.Serializable]
public class PhaseChangeEvent : UnityEvent<EWorldPhase>
{
}

[System.Serializable]
public class TimerStopEvent : UnityEvent<float>
{
}

public class GameTimer : MonoBehaviour
{
    public PhaseChangeEvent OnPhaseChange;
    public PhaseChangeEvent OnPhaseChangeToDark;
    public PhaseChangeEvent OnPhaseChangeToLight;
    public PhaseChangeEvent OnTimerStart;
    public TimerStopEvent OnTimerStop;
    public UnityEvent OnTimerResume;

    private float timer;
    private float phaseTime = 10f;
    private EWorldPhase worldPhase;
    private bool isTimerRunning = false;
    private float currentRoundGameTime;
    private float totalGameTime;
    private float totalExpectedTime;

    private void Awake()
    {
        if (OnPhaseChange == null) OnPhaseChange = new PhaseChangeEvent();
        if (OnTimerStart == null) OnTimerStart = new PhaseChangeEvent();
        if (OnTimerStop == null) OnTimerStop = new TimerStopEvent();
        if (OnTimerResume == null) OnTimerResume = new UnityEvent();
    }

    public void StartTimer()
    {
        timer = phaseTime;
        isTimerRunning = true;
        OnTimerStart.Invoke(worldPhase);
    }

    public void ResumeTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        OnTimerStop.Invoke(timer);
    }

    public bool Running()
    {
        return isTimerRunning;
    }

    public float GetTime()
    {
        return timer;
    }

    public EWorldPhase GetWorldPhase()
    {
        return worldPhase;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentRoundGameTime += Time.deltaTime;
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ChangePhase();
            }
        }
    }

    public void ChangePhase()
    {
        worldPhase++;
        if (worldPhase >= EWorldPhase.DEFAULT_LAST)
        {
            worldPhase = 0;
        }

        OnPhaseChange.Invoke(worldPhase);
        timer = phaseTime;

        if (Application.isEditor)
            Debug.Log("Phase: " + worldPhase);
    }

    public float GetTimer()
    {
        return timer;
    }

    public void JumpToLightPhase()
    {
        if (worldPhase is EWorldPhase.LIGHT) return;

        //Its dark, change phase
        ChangePhase();
    }

    public float GetPlayedTimeScore()
    {
        return Mathf.Clamp(totalExpectedTime - totalGameTime, 0, totalExpectedTime);
    }

    public void ExpectedRounds(float slowRoundTime)
    {
        totalExpectedTime += slowRoundTime;
    }
}