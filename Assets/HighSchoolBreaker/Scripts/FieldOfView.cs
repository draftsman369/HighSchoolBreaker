using UnityEngine;
using System;
public class FieldOfView : MonoBehaviour
{
    
    

    public Transform player;

    public float viewDistance = 10f;
    public float viewAngle = 90f;

    public LayerMask obstacleMask;

    [SerializeField] private bool playerSpotted = false;

    public bool canSeePlayer{get; private set;}

    void Update()
    {
        if(PlayerController.Instance.IsHidden) return;
        canSeePlayer = CanSeePlayer();
        if(canSeePlayer)
        {
            playerSpotted = true;
        }else
        {
            playerSpotted = false;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 eyesPosition = this.transform.position + Vector3.up; // Adjust for eye height
        Vector3 directionToPlayer = player.position - eyesPosition; // Adjust for eye height
        float distanceToPlayer = directionToPlayer.magnitude;

        // Too far
        if (distanceToPlayer > viewDistance)
            return false;

        // Outside vision angle
        float angle = Vector3.Angle(this.transform.forward, directionToPlayer);

        if (angle > viewAngle / 2f)
            return false;

        // Blocked by wall/object
        if (Physics.Raycast(eyesPosition, directionToPlayer.normalized, distanceToPlayer, obstacleMask))
            return false;

        return true;
    }
}
