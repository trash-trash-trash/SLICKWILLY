using UnityEngine;

public class Timer : MonoBehaviour
{
    //note: model + view together
    
    public PlayerInputHandler playerInputs;
    
    [SerializeField] private float currentTime = 0f;
    
    public float CurrentTime
    {
        get => currentTime;
        private set => currentTime = value;
    }

    public bool TimeIsRunning { get; private set; } = false;

    public float bestTime = 10000f;
    public bool aNewRecord = false;
    
    public GameObject pauseCanvasObj;
    public GameObject skinPickerObject;

    void Start()
    {
        playerInputs.AnnounceEscape += PlayerHitEscape;
        
        aNewRecord = false;
        StartTimer();
    }

    private void PlayerHitEscape(bool obj)
    {
        if(obj)
        {
            if (TimeIsRunning)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }


    void Update()
    {
        if (!TimeIsRunning) return;
        CurrentTime += Time.deltaTime;
    }

    public void StartTimer()
    {
        aNewRecord = false;
        CurrentTime = 0f;
        TimeIsRunning = true;
    }

    public void Stop()
    {
        TimeIsRunning = false;
        if (CurrentTime < bestTime)
        {
            bestTime = CurrentTime;
            aNewRecord = true;
        }
    }

    public void Resume()
    {
        pauseCanvasObj.SetActive(false);
        skinPickerObject.SetActive(false);
        TimeIsRunning = true;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        pauseCanvasObj.SetActive(true);
        skinPickerObject.SetActive(true);
        TimeIsRunning = false;
        Time.timeScale = 0f; 
    }

    public void ResetTimer()
    {
        CurrentTime = 0f;
    }

}