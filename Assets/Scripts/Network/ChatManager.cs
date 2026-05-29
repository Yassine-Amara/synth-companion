using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    public static ChatManager Instance;
    public TMP_InputField inputField;
    public TextMeshProUGUI chatDisplay;
    public ScrollRect scrollRect;
    private List<string> messages = new List<string>();

    void Awake() => Instance = this;

    public void SendMessage()
    {
        if (inputField == null || string.IsNullOrEmpty(inputField.text)) return;
        SendMessageServerRpc(inputField.text, NetworkManager.Singleton.LocalClientId);
        inputField.text = "";
    }

    [Rpc(SendTo.Server)]
    void SendMessageServerRpc(string msg, ulong senderId)
    {
        BroadcastMessageClientRpc($"Joueur {senderId}: {msg}");
    }

    [Rpc(SendTo.Everyone)]
    void BroadcastMessageClientRpc(string msg)
    {
        messages.Add(msg);
        chatDisplay.text = string.Join("\n", messages);
        if (scrollRect != null)
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(
                scrollRect.content.GetComponent<RectTransform>());
        StartCoroutine(ScrollToBottom());
    }

    System.Collections.IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        if (scrollRect != null)
            scrollRect.verticalNormalizedPosition = 0f;
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