using NUnit.Framework;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    private Animator animator;
    public bool isDoorOpen = false;
    public bool isLocked = false;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    public void Interact()
    {
        if (isLocked)
        {
            Debug.Log("Door is locked!");
            return;
        }
        Debug.Log("Interacted with door!");
        isDoorOpen = !isDoorOpen;
        animator.SetBool("IsDoorOpen", isDoorOpen);
        // Implement door opening logic here
    }

    public string GetInteractText()
    {
        if(isDoorOpen)
        {
            return "Close door";
        }
        return "Open door";
        // Implement door opening logic here
    }
}
