using UnityEngine;
public class PlayerWonState : State
{
    public PlayerWonState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        GameManager.Instance.SetGameWon();
        player.PlayGameWonAnimation();
        player.SwitchToWinCamera();
        Debug.Log("Player has won the game!");
        base.Enter();
    }
}