using UnityEngine;
using System;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance { get; private set; }
    public GameObject player;

    public event EventHandler<OnSelectedInteractableEventArgs> OnSelectedInteractable;
    public class OnSelectedInteractableEventArgs : EventArgs
    {
        public IInteractable selectedInteractable;
    }

    [SerializeField] private float interactDistance;
    [SerializeField] LayerMask interactLayerMask;

    Vector3 lastInteractDirection;
    IInteractable selectedInteractable;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    private void Update()
    {
        HandleInteraction();
    }
    private void Start()
    {
        InputReader.Instance.OnInteractAction += Interact;
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
        Vector2 moveInput = InputReader.Instance.MoveInput;
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        if(moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }

        if(Physics.Raycast(this.transform.position, lastInteractDirection, out RaycastHit hit, interactDistance, interactLayerMask))
        {
            if(hit.transform.TryGetComponent(out IInteractable interactable))
            {
                SetSelectedInteractable(interactable);
            }
            else
            {
                SetSelectedInteractable(null);
            }
        }
        else
        {
            SetSelectedInteractable(null);
        }
    }

    private void SetSelectedInteractable(IInteractable interactable)
    {
        this.selectedInteractable = interactable;
        OnSelectedInteractable?.Invoke(this, new OnSelectedInteractableEventArgs {
            selectedInteractable = selectedInteractable
        });

    }

}
