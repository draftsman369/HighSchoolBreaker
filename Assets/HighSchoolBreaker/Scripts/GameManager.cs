using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool GameWon {get; private set;}

    private AudioSource audioSource;
    public AudioClip victoryMusic;
    public AudioClip backgroundMusic;
    [SerializeField] private string mainMenuSceneName = "MainMenu";


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.Play();
    }

    private void OnEnable()
    {

    }

    public void SetGameWon()
    {
        Timer.Instance.StopTimer();
        audioSource.clip = victoryMusic;
        audioSource.PlayOneShot(victoryMusic);
        GameWon = true;
        LevelLoader.Instance.LoadMainMenu();
    }
    public void ResetGame()
    {
        //audioSource.Stop();
        GameWon = false;
        audioSource.clip = backgroundMusic;
        //audioSource.Play();
        Timer.Instance.ResetTimer();
        Destroy(this.gameObject);
    }    

    private void Update()
    {
        
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }



}
