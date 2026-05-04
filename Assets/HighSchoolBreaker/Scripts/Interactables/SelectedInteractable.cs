using UnityEngine;

public class SelectedInteractable : MonoBehaviour
{
    [SerializeField] private IInteractable interactable;
    public GameObject selectedVisual;

    private void Awake()
    {
        interactable = this.GetComponent<IInteractable>();
    }

    private void Start()
    {
        PlayerInteract.Instance.OnSelectedInteractable += Player_OnSelectedInteractChange;
    }

    private void Player_OnSelectedInteractChange(object sender, PlayerInteract.OnSelectedInteractableEventArgs e)
    {
        if(e.selectedInteractable == interactable)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        selectedVisual.SetActive(true);
    }

    public void Hide()
    {
        selectedVisual.SetActive(false);
    }

}
