using UnityEngine;

public class RemoteCarController : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Rigidbody carRigidbody;
    [SerializeField] private Transform player;

    [Header("Deploy")]
    [SerializeField] private float deployDistance = 1.5f;
    [SerializeField] private bool rechargeOnRetrieve = true;

    [Header("Deploy Check")]
    [SerializeField] private float deployCheckRadius = 0.5f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private string cannotDeployText = "I can't deploy here";

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 120f;

    [Header("Signal")]
    [SerializeField] private float maxSignalRadius = 15f;

    [Header("Energy")]
    [SerializeField] private float maxEnergy = 10f;
    [SerializeField] private float energyDrainPerSecond = 1f;

    private float currentEnergy;
    private bool isControlled;
    private bool isDeployed;
    public bool CanBeDeployed {get; private set;}

    public bool IsDeployed => isDeployed;
    public bool HasEnergy => currentEnergy > 0f;
    public bool HasSignal => Vector3.Distance(transform.position, player.position) <= maxSignalRadius;
    public bool CanControl => isControlled && isDeployed && HasEnergy && HasSignal;

    private void Awake()
    {
        if (carRigidbody == null)
            carRigidbody = GetComponent<Rigidbody>();

        currentEnergy = maxEnergy;
        gameObject.SetActive(false);
    }

    public void Deploy()
    {
        Vector3 deployPosition = player.position + player.forward * deployDistance;
        deployPosition.y = transform.position.y;

        if (!CanDeployAtPosition(deployPosition))
        {
            CanBeDeployed = false;
            return;
        }

        CanBeDeployed = true;
        gameObject.SetActive(true);

        isDeployed = true;
        isControlled = false;

        transform.position = deployPosition;
        transform.rotation = Quaternion.LookRotation(player.forward);

        carRigidbody.linearVelocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
    }

    private bool CanDeployAtPosition(Vector3 deployPosition)
    {
        Vector3 lineStart = player.position + Vector3.up * 0.5f;
        Vector3 lineEnd = deployPosition + Vector3.up * 0.5f;

        bool wallBetweenPlayerAndCar = Physics.Linecast(
            lineStart,
            lineEnd,
            obstacleLayer
        );

        bool deployPositionBlocked = Physics.CheckSphere(
            deployPosition,
            deployCheckRadius,
            obstacleLayer
        );

        if (wallBetweenPlayerAndCar || deployPositionBlocked)
        {
            Debug.Log(cannotDeployText);

            // Uncomment this when your UIManager has ShowTemporaryText()
            UIManager.Instance.ShowTemporaryText(cannotDeployText);

            return false;
        }

        return true;
    }

    public void Retrieve()
    {
        isControlled = false;
        isDeployed = false;

        carRigidbody.linearVelocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;

        if (rechargeOnRetrieve)
            currentEnergy = maxEnergy;

        gameObject.SetActive(false);
    }

    public void StartControl()
    {
        if (!isDeployed)
            return;

        isControlled = true;
    }

    public void StopControl()
    {
        isControlled = false;

        carRigidbody.linearVelocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
    }

    public void Move(Vector2 input)
    {
        if (!CanControl)
            return;

        if (input.sqrMagnitude > 0.01f)
        {
            currentEnergy -= energyDrainPerSecond * Time.fixedDeltaTime;
            currentEnergy = Mathf.Max(currentEnergy, 0f);
        }

        Vector3 movement = transform.forward * input.y * moveSpeed * Time.fixedDeltaTime;
        carRigidbody.MovePosition(carRigidbody.position + movement);

        Quaternion rotation = Quaternion.Euler(
            0f,
            input.x * turnSpeed * Time.fixedDeltaTime,
            0f
        );

        carRigidbody.MoveRotation(carRigidbody.rotation * rotation);
    }

    public void Interact()
    {
        Retrieve();
    }

    public string GetInteractText()
    {
        return "Retrieve car";
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null)
            return;

        Vector3 deployPosition = player.position + player.forward * deployDistance;
        deployPosition.y = transform.position.y;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(deployPosition, deployCheckRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            player.position + Vector3.up * 0.5f,
            deployPosition + Vector3.up * 0.5f
        );
    }
}