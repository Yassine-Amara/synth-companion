using Unity.Netcode;
using UnityEngine;

public class PlacementPuzzle : NetworkBehaviour
{
    public Transform target;
    public Transform ball;

    void Update()
    {
        if (!IsServer || ball == null) return;
        if (Vector3.Distance(ball.position, target.position) < 0.5f)
            PuzzleManager.Instance.puzzle2State.Value = 1;
    }
}