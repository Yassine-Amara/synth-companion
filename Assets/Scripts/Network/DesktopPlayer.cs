using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopPlayer : NetworkBehaviour
{
    private CharacterController cc;
    private Camera cam;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private float rotationX = 0f;
    private float mouseSensitivity = 0.3f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        cc = GetComponent<CharacterController>();
        velocity = Vector3.zero;
        transform.position = new Vector3(4f, 5f, 6f);
        cam = Camera.main;
        if (cam != null)
        {
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0, 1.7f, 0);
            cam.transform.localRotation = Quaternion.identity;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Start()
    {
    }

    void Update()
    {
        if (!IsOwner) return;
        if (cc == null) return;

        // Rotation camera avec la souris
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
            float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity;
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -80f, 80f);
            if (cam != null)
                cam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // Gravite
        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        // Deplacement WASD
        Vector2 move = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) move.y += 1;
        if (Keyboard.current.sKey.isPressed) move.y -= 1;
        if (Keyboard.current.aKey.isPressed) move.x -= 1;
        if (Keyboard.current.dKey.isPressed) move.x += 1;
        Vector3 direction = transform.right * move.x + transform.forward * move.y;
        direction.y = 0;
        cc.Move(direction * 5f * Time.deltaTime);

        // Interaction avec E — regarde vers un cube de sequence et appuie E
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryInteract();
        }

        // Echap pour liberer/capturer la souris
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void TryInteract()
    {
        if (cam == null) return;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            PuzzleButton pb = hit.collider.GetComponent<PuzzleButton>();
            if (pb != null)
            {
                SequencePuzzle.Instance.PressButtonServerRpc(pb.buttonId);
                pb.StartCoroutine(pb.FlashEffect());
                return;
            }
        }
    }
}