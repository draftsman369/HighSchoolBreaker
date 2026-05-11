using UnityEngine;
using TMPro;
using System.Collections;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance{get; private set;}

    [SerializeField] private GameObject interactGameObject;
    [SerializeField] private TextMeshProUGUI interactText;


    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI timerText;


    [SerializeField] private TextMeshProUGUI temporaryText;

    private Coroutine temporaryTextCoroutine;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void ShowInteractText(string text)
    {
        interactGameObject.SetActive(true);
        interactText.text = text;
    }

    public void HideInteractText()
    {
        interactGameObject.SetActive(false);
        interactText.text = "";
    }


    public void ShowTemporaryText(string message)
    {
        if (temporaryTextCoroutine != null)
            StopCoroutine(temporaryTextCoroutine);

        temporaryTextCoroutine = StartCoroutine(
            ShowTemporaryTextRoutine(message)
        );
    }

    private IEnumerator ShowTemporaryTextRoutine(string message)
    {
        temporaryText.text = message;
        temporaryText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        temporaryText.gameObject.SetActive(false);
    }
}
