using UnityEngine;
using TMPro;

public class PuzzleHUD : MonoBehaviour
{
    public TextMeshProUGUI statusText;

    void Update()
    {
        var pm = PuzzleManager.Instance;
        if (pm == null) return;
        statusText.text =
            $"Code: {(pm.puzzle1State.Value == 1 ? "OK" : "??")}   " +
            $"Balle: {(pm.puzzle2State.Value == 1 ? "OK" : "??")}   " +
            $"Sequence: {(pm.puzzle3Complete.Value ? "OK" : "??")}";
    }
}