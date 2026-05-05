using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private Animator animator;

    public void Interact()
    {
        Debug.Log("Interacted with door!");
        animator.SetTrigger("Open");
        // Implement door opening logic here
    }

    public string GetInteractText()
    {
        return "Interacted with door!";
        // Implement door opening logic here
    }
}
