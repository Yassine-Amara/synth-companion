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
    private string enteredCode = "";

    void Update()
    {
        if (solved) return;
        if (player == null)
        {
            var p = FindObjectOfType<DesktopPlayer>();
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
            enteredCode = "";
            UpdateDisplay();
        }
    }

    // Appele par chaque bouton chiffre
    public void PressDigit(string digit)
    {
        if (enteredCode.Length < 4)
        {
            enteredCode += digit;
            UpdateDisplay();
        }
    }

    public void DeleteDigit()
    {
        if (enteredCode.Length > 0)
        {
            enteredCode = enteredCode.Substring(0, enteredCode.Length - 1);
            UpdateDisplay();
        }
    }

    void UpdateDisplay()
    {
        if (codeInput != null)
            codeInput.text = enteredCode;
    }

    public void TryCode()
    {
        TryCodeServerRpc(enteredCode);
    }

    [ServerRpc(RequireOwnership = false)]
    void TryCodeServerRpc(string entered)
    {
        if (entered == secretCode)
        {
            PuzzleManager.Instance.puzzle1State.Value = 1;
            HidePanelClientRpc();
        }
        else
        {
            ResetCodeClientRpc();
        }
    }

    [ClientRpc]
    void HidePanelClientRpc()
    {
        solved = true;
        if (codePanel != null) codePanel.SetActive(false);
    }

    [ClientRpc]
    void ResetCodeClientRpc()
    {
        enteredCode = "";
        UpdateDisplay();
    }
}