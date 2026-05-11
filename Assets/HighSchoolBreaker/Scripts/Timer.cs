using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{

    public static Timer Instance{get; private set;}
    public float timer;
    [SerializeField]
    private TextMeshProUGUI timerText;

    public bool IsTimerStopped{get; private set;}

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    } 

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (IsTimerStopped) return;

        timer += Time.deltaTime;
        float minutes = timer/60f;
        float seconds = timer%60f;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        IsTimerStopped = true;
    }

    public void StartTimer()
    {
        IsTimerStopped = false;
    }
}
