using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallDragger : NetworkBehaviour
{
    private bool isDragging = false;
    private Camera cam;
    private Rigidbody rb;
    private float dragHeight = 1.5f;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (cam == null) cam = Camera.main;

        // Clic gauche souris — utilise le nouveau Input System
        bool mouseDown = Mouse.current.leftButton.wasPressedThisFrame;
        bool mouseUp = Mouse.current.leftButton.wasReleasedThisFrame;
        bool mouseHeld = Mouse.current.leftButton.isPressed;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (mouseDown)
        {
            // Vérifie si on clique sur la balle
            Ray ray = cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit, 20f))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.linearVelocity = Vector3.zero;
                    }
                }
            }
        }

        if (mouseUp)
        {
            isDragging = false;
            if (rb != null)
                rb.isKinematic = false;
        }

        if (isDragging && mouseHeld)
        {
            Ray ray = cam.ScreenPointToRay(mousePos);
            Plane plane = new Plane(Vector3.up, new Vector3(0, dragHeight, 0));
            if (plane.Raycast(ray, out float dist))
            {
                Vector3 targetPos = ray.GetPoint(dist);
                targetPos.y = dragHeight;
                if (IsServer)
                    transform.position = Vector3.Lerp(transform.position, targetPos, 0.3f);
                else
                    UpdatePositionServerRpc(targetPos);
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void UpdatePositionServerRpc(Vector3 newPos)
    {
        transform.position = Vector3.Lerp(transform.position, newPos, 0.3f);
    }
}