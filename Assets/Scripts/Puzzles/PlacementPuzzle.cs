using Unity.Netcode;
using UnityEngine;

public class PlacementPuzzle : NetworkBehaviour
{
    public Transform target;
    public Transform ball;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!IsServer) return;
        if (ball == null) return;
        if (Vector3.Distance(ball.position, target.position) < 0.5f)
            PuzzleManager.Instance.puzzle2State.Value = 1;
    }
}