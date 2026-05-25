using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Instance;
    public TMP_InputField inputField;
    public TextMeshProUGUI chatDisplay;
    private List<string> messages = new List<string>();

    void Awake() => Instance = this;

    public void SendMessage()
    {
        if (inputField == null || string.IsNullOrEmpty(inputField.text)) return;
        SendMessageServerRpc(inputField.text, NetworkManager.Singleton.LocalClientId);
        inputField.text = "";
    }

    [ServerRpc(RequireOwnership = false)]
    void SendMessageServerRpc(string msg, ulong senderId)
    {
        BroadcastMessageClientRpc($"Joueur {senderId}: {msg}");
    }

    [ClientRpc]
    void BroadcastMessageClientRpc(string msg)
    {
        messages.Add(msg);
        if (messages.Count > 12) messages.RemoveAt(0);
        chatDisplay.text = string.Join("\n", messages);
    }

    public void DisplayAIMessage(string msg)
    {
        if (IsServer) BroadcastMessageClientRpc($"Alex: {msg}");
    }

    public void SendVoiceMessage(string text)
    {
        SendMessageServerRpc(text, NetworkManager.Singleton.LocalClientId);
    }

    public string GetLastMessages()
    {
        int count = Mathf.Min(messages.Count, 5);
        if (count == 0) return "aucun message encore";
        return string.Join(" | ", messages.GetRange(messages.Count - count, count));
    }
}