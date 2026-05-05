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

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(3f);

        animator.SetTrigger("Transition");
        Debug.Log("Starting level reload...");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }
}