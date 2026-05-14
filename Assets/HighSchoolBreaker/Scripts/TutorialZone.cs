using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    [TextArea]
    public string instruction;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialManager.Instance.StartCoroutine(TutorialManager.Instance.ShowTutorial(instruction));
            //AudioManager.Instance.PlaySFX("TutorialStart");
            Destroy(gameObject); // Destroy the tutorial zone after triggering
        }
    }
}
