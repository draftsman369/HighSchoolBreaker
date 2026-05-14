using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class TutorialStep
{
    public string instruction;
    public bool isCompleted;
}

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }



    //public TutorialStep[] tutorialSteps;


    [SerializeField] private CanvasGroup tutorialCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;
    private float fadeTimer = 0f;

    public TextMeshProUGUI instructionText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialCanvasGroup.alpha = 0f;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ShowTutorial(string instruction)
    {
        fadeTimer = 0f;
        instructionText.text = instruction;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            tutorialCanvasGroup.alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            yield return null;
        }
        tutorialCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(2f); // Show tutorial for 2 seconds
        StartCoroutine(HideTutorial());

    }

    public IEnumerator HideTutorial()
    {
        fadeTimer = 0f;
        while (fadeTimer < fadeDuration + 2f)
        {
            fadeTimer += Time.deltaTime;
            tutorialCanvasGroup.alpha = 1f - Mathf.Clamp01(fadeTimer / fadeDuration);
            yield return null;
        }
        tutorialCanvasGroup.alpha = 0f;
    }


}
