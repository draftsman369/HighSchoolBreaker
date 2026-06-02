using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    public Animator animator;


private bool isLoadingNextLevel = false;


    private void Awake()
    {
        Instance = this;
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isLoadingNextLevel && SceneManager.GetActiveScene().buildIndex == 2)
        {
            isLoadingNextLevel = true;
            LoadLevel(3, 5f);
        }
    }

    public void ReloadLevel()
    {
        StartCoroutine(LoadLevel());
    }

    public void StartGame()
    {
        StartCoroutine(StartLevel());
    }

    public void LoadElevator()
    {
        StartCoroutine(StartElevator());
    }

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(3f);

        animator.SetTrigger("Transition");
        Debug.Log("Starting level reload...");

        yield return new WaitForSeconds(2f);
        GameManager.Instance.PlayAudio();

        Timer.Instance.ResetTimer();
        Timer.Instance.StartTimer();
        SceneManager.LoadScene(1);
        isLoadingNextLevel = false;
    }

    public void LoadMainMenu()
    {
        //GameManager.Instance.ResetGame();
        StartCoroutine(LoadMainMenuCoroutine());
    }

    private IEnumerator LoadMainMenuCoroutine()
    {
        yield return new WaitForSeconds(5f);

        animator.SetTrigger("Transition");
        Debug.Log("Starting main menu load...");

        yield return new WaitForSeconds(2f);

        //GameManager.Instance.StopAudio();
        //GameManager.Instance.ResetGame();

        SceneManager.LoadScene(0);
    }

    private IEnumerator StartLevel()
    {
        //yield return new WaitForSeconds(1f);

        animator.SetTrigger("Transition");
        Debug.Log("Starting level reload...");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(1);
    }

    private IEnumerator StartElevator()
    {
        //yield return new WaitForSeconds(1f);

        animator.SetTrigger("Transition");
        Debug.Log("Starting elevator level...");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(2);
    }

    private void QuitEleveator(int sceneIndex)
    {
        if(sceneIndex == 2)
        {
            StartCoroutine(LoadMainMenuCoroutine());
        }
    }

    public void LoadLevel(int levelIndex, float delay)
    {
        StartCoroutine(LoadLevelCoroutine(levelIndex, delay));
    }
    public IEnumerator LoadLevelCoroutine(int levelIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.SetTrigger("Transition");
        Debug.Log($"Starting level {levelIndex} load...");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(levelIndex);
    }




    public void QuitGame()
    {
        Application.Quit();
    }
}