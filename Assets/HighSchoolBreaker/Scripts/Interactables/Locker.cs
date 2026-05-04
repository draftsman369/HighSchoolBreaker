using UnityEngine;

public class Locker : MonoBehaviour, IInteractable
{
    private PlayerController currentPlayer;
    private Animator lockAnimator;

    [SerializeField] private Transform hidePoint; // where the player snaps inside

    private void Awake()
    {
        lockAnimator = this.GetComponent<Animator>();
    }

    public void Interact()
    {
        PlayerController player = PlayerController.Instance;
        lockAnimator.SetTrigger("Interact");

        // ENTER locker
        if (currentPlayer == null)
        {
            EnterLocker(player);
        }
        // EXIT locker
        else if (currentPlayer == player)
        {
            ExitLocker();
        }

    }

    private void EnterLocker(PlayerController player)
    {
        currentPlayer = player;

        player.EnterLocker(this, hidePoint);
    }

    public void ExitLocker()
    {
        if (currentPlayer == null) return;

        currentPlayer.ExitLocker();
        currentPlayer = null;
    }

    public string GetInteractText()
    {
        return "Hide";
    }
}