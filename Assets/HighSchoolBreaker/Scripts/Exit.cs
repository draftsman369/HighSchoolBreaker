using UnityEngine;

public class Exit : MonoBehaviour
{
    public enum ExitType
    {
        LevelExit,
        GameExit
    }

    public ExitType exitType;
    private void OnTriggerEnter(Collider other)
    {
        if(exitType == ExitType.LevelExit)
        {
            LevelLoader.Instance.LoadLevel(2, 0f);
            return;
        }
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetGameWon();
            Debug.Log("Player reached the exit! Game Won!");
            PlayerController.Instance.ChangeState(new PlayerWonState(PlayerController.Instance));
        }
    }
}
