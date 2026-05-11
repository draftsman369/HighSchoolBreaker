using UnityEngine;

public class Exit : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetGameWon();
            Debug.Log("Player reached the exit! Game Won!");
            PlayerController.Instance.ChangeState(new PlayerWonState(PlayerController.Instance));
        }
    }
}
