using Unity.Netcode;
using UnityEngine;
using TMPro;

public class KeypadPuzzle : NetworkBehaviour
{
    public string secretCode = "4729";
    public TMP_InputField codeInput;

    public void TryCode()
    {
        TryCodeServerRpc(codeInput.text);
    }

    [ServerRpc(RequireOwnership = false)]
    void TryCodeServerRpc(string entered)
    {
        if (entered == secretCode)
            PuzzleManager.Instance.puzzle1State.Value = 1;
    }
}