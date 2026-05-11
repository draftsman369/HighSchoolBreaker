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

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(3f);

        animator.SetTrigger("Transition");
        Debug.Log("Starting level reload...");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(1);
    }

    private IEnumerator StartLevel()
    {
        //yield return new WaitForSeconds(1f);

        animator.SetTrigger("Transition");
        Debug.Log("Starting level reload...");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}