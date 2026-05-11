using UnityEngine;

public class PlayerNoiseEmitter : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float noiseRadius = 9f;
    [SerializeField] private LayerMask patrollerMask;
    [SerializeField] private float noiseCooldown = 0.5f;

    private float noiseTimer;

    private void Awake()
    {
        if (playerController == null)
            playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.IsGameOver)
            return;

        noiseTimer -= Time.deltaTime;

        if (!ShouldMakeNoise())
            return;

        if (noiseTimer <= 0f)
        {
            MakeNoise();
            noiseTimer = noiseCooldown;
        }
    }

    private bool ShouldMakeNoise()
    {
        return playerController.IsMoving && !playerController.IsSneaking;
    }

    private void MakeNoise()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            noiseRadius,
            patrollerMask
        );

        foreach (Collider hit in hits)
        {
            MisterController patroller = hit.GetComponentInParent<MisterController>();

            if (patroller != null)
            {
                patroller.HearNoise(transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }
}