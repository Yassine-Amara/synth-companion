using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class SequencePuzzle : NetworkBehaviour
{
    public static SequencePuzzle Instance;
    public int[] correctOrder = { 0, 2, 1, 3 };
    private List<int> playerOrder = new List<int>();

    void Awake() => Instance = this;

    [ServerRpc(RequireOwnership = false)]
    public void PressButtonServerRpc(int buttonId)
    {
        playerOrder.Add(buttonId);
        if (playerOrder.Count == 4)
        {
            bool correct = true;
            for (int i = 0; i < 4; i++)
                if (playerOrder[i] != correctOrder[i]) { correct = false; break; }
            if (correct)
                PuzzleManager.Instance.puzzle3Complete.Value = true;
            else
                playerOrder.Clear();
        }
    }
}