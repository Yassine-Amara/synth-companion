using UnityEngine;

public class PuzzleButton : MonoBehaviour
{
    public int buttonId;

    void OnMouseDown()
    {
        SequencePuzzle.Instance.PressButtonServerRpc(buttonId);
    }
}