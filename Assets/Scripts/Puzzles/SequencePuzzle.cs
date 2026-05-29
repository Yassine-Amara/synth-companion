using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SequencePuzzle : NetworkBehaviour
{
    public static SequencePuzzle Instance;
    public int[] correctOrder = { 0, 2, 1, 3 };
    private List<int> playerOrder = new List<int>();
    public TextMeshProUGUI progressText;

    string[] colorNames = { "Rouge", "Bleu", "Vert", "Jaune" };

    void Awake() => Instance = this;

    [Rpc(SendTo.Server)]
    public void PressButtonServerRpc(int buttonId)
    {
        playerOrder.Add(buttonId);
        UpdateProgressClientRpc(playerOrder.Count, buttonId);

        if (playerOrder.Count == 4)
        {
            bool correct = true;
            for (int i = 0; i < 4; i++)
                if (playerOrder[i] != correctOrder[i]) { correct = false; break; }

            if (correct)
            {
                PuzzleManager.Instance.puzzle3Complete.Value = true;
                ShowSuccessClientRpc();
            }
            else
            {
                playerOrder.Clear();
                ShowErrorClientRpc();
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    void UpdateProgressClientRpc(int count, int buttonId)
    {
        if (progressText == null) return;
        progressText.text = $"Sequence : {count}/4\nDernier : {colorNames[buttonId]}";
        progressText.color = Color.yellow;
    }

    [Rpc(SendTo.Everyone)]
    void ShowErrorClientRpc()
    {
        if (progressText == null) return;
        progressText.text = "Mauvaise sequence !\nRecommencez...";
        progressText.color = Color.red;
    }

    [Rpc(SendTo.Everyone)]
    void ShowSuccessClientRpc()
    {
        if (progressText == null) return;
        progressText.text = "Sequence OK !";
        progressText.color = Color.green;
    }
}