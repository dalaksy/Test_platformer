using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float defaultDistance = 5.0f;
    public float minDistance = 1.5f;
    public float lookAtOffset = 1.5f;
    public float sensitivity = 3.0f;
    public float smoothSpeed = 10.0f;
    public LayerMask collisionLayers;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private float currentDistance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = defaultDistance;
        if (collisionLayers == 0) collisionLayers = LayerMask.GetMask("Default");
    }

    void LateUpdate()
    {
        if (!target) return;

        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity;

        rotationY = Mathf.Clamp(rotationY, -89f, 89f);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        Vector3 focusPoint = target.position + Vector3.up * lookAtOffset;
        Vector3 direction = rotation * Vector3.back;

        RaycastHit hit;
        float targetDistance = defaultDistance;

        if (Physics.Raycast(focusPoint, direction, out hit, defaultDistance, collisionLayers))
        {
            targetDistance = Mathf.Clamp(hit.distance - 0.3f, minDistance, defaultDistance);
        }

        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * smoothSpeed);
        transform.position = focusPoint + direction * currentDistance;
        transform.LookAt(focusPoint);
    }
}