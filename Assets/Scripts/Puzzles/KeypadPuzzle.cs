using Unity.Netcode;
using UnityEngine;
using TMPro;

public class KeypadPuzzle : NetworkBehaviour
{
    public string secretCode = "4729";
    public TMP_InputField codeInput;
    public GameObject codePanel;
    public float interactDistance = 2f;
    private Transform player;
    private bool solved = false;

    void Update()
    {
        if (solved) return;
        if (player == null)
        {
            var p = FindFirstObjectByType<DesktopPlayer>();
            if (p != null) player = p.transform;
            return;
        }
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < interactDistance)
        {
            if (codePanel != null) codePanel.SetActive(true);
        }
        else
        {
            if (codePanel != null) codePanel.SetActive(false);
        }
    }

    public void TryCode()
    {
        TryCodeServerRpc(codeInput.text);
    }

    [Rpc(SendTo.Server)]
    void TryCodeServerRpc(string entered)
    {
        if (entered == secretCode)
        {
            PuzzleManager.Instance.puzzle1State.Value = 1;
            HidePanelClientRpc();
        }
    }

    [Rpc(SendTo.Everyone)]
    void HidePanelClientRpc()
    {
        solved = true;
        if (codePanel != null) codePanel.SetActive(false);
    }
}