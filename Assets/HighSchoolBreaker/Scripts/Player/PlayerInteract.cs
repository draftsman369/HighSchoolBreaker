using UnityEngine;
using System;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance { get; private set; }

    public event EventHandler<OnSelectedInteractableEventArgs> OnSelectedInteractable;

    public class OnSelectedInteractableEventArgs : EventArgs
    {
        public IInteractable selectedInteractable;
    }

    [Header("Interaction")]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private float interactRadius = 1.2f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("References")]
    [SerializeField] private Transform orientation;

    private IInteractable selectedInteractable;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InputReader.Instance.OnInteractAction += Interact;
    }

    private void Update()
    {
        HandleInteraction();
    }

    private void Interact(object sender, EventArgs e)
    {
        if (PlayerController.Instance.IsHidden)
        {
            PlayerController.Instance.TryExitLocker();
            return;
        }

        selectedInteractable?.Interact();
    }

    private void HandleInteraction()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            interactRadius
        );

        IInteractable bestInteractable = null;
        float bestScore = -999f;

        Vector3 forward = orientation != null
            ? orientation.forward
            : transform.forward;

        forward.y = 0f;
        forward.Normalize();

        foreach (Collider hit in hits)
        {
            if (!hit.TryGetComponent(out IInteractable interactable))
                continue;

            Vector3 directionToObject = hit.transform.position - transform.position;
            directionToObject.y = 0f;

            float distance = directionToObject.magnitude;

            if (distance > interactDistance)
                continue;

            directionToObject.Normalize();

            float dot = Vector3.Dot(forward, directionToObject);

            if (dot < 0.25f)
                continue;

            float score = dot * 2f - distance;

            if (score > bestScore)
            {
                bestScore = score;
                bestInteractable = interactable;
            }
        }

        SetSelectedInteractable(bestInteractable);

        if (selectedInteractable != null)
        {
            UIManager.Instance.ShowInteractText(selectedInteractable.GetInteractText());
        }
        else
        {
            UIManager.Instance.HideInteractText();
        }
    }

    private void SetSelectedInteractable(IInteractable interactable)
    {
        if (selectedInteractable == interactable)
            return;

        selectedInteractable = interactable;

        OnSelectedInteractable?.Invoke(this, new OnSelectedInteractableEventArgs
        {
            selectedInteractable = selectedInteractable
        });
    }

    private void OnDestroy()
    {
        if (InputReader.Instance != null)
        {
            InputReader.Instance.OnInteractAction -= Interact;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);

        Vector3 forward = orientation != null
            ? orientation.forward
            : transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, forward.normalized * interactDistance);
    }
}