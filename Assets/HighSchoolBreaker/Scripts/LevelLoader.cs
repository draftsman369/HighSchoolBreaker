using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    public Animator animator;

    private void Awake()
    {
        Instance = this;
        //animator = GetComponent<Animator>();
    }

    public void ReloadLevel()
    {
        StartCoroutine(LoadLevel());
    }

    public void StartGame()
    {
        StartCoroutine(StartLevel());
    }

    public void StartLevel2_()
    {
        StartCoroutine(StartLevel2());
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

    private IEnumerator StartLevel2()
    {
        //yield return new WaitForSeconds(1f);

        animator.SetTrigger("Transition");
        Debug.Log("Starting level reload...");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}