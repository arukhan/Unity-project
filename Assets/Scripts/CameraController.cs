using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Target transform to follow (usually the player).")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [Tooltip("Offset distance from the target.")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 3.5f, -6f);

    [Tooltip("Smooth follow lag factor.")]
    [SerializeField] private float smoothSpeed = 10f;

    [Tooltip("Camera pitch look angle offset.")]
    [SerializeField] private float lookAtHeightOffset = 1.5f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        Vector3 lookTarget = target.position + Vector3.up * lookAtHeightOffset;
        transform.LookAt(lookTarget);
    }
}
