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

    [ServerRpc(RequireOwnership = false)]
    void SendMessageServerRpc(string msg, ulong senderId)
    {
        BroadcastMessageClientRpc($"Joueur {senderId}: {msg}");
    }

    [ClientRpc]
    void BroadcastMessageClientRpc(string msg)
    {
        messages.Add(msg);
        chatDisplay.text = string.Join("\n", messages);

        StartCoroutine(ScrollToBottom());

        // Synchronisation réseau du TTS : Si le message vient d'Alex, tout le monde joue le son
        if (msg.StartsWith("Alex:") && TTSPlayer.Instance != null)
        {
            string cleanText = msg.Replace("Alex:", "").Trim();
            StartCoroutine(TTSPlayer.Instance.Speak(cleanText));
        }
    }

    System.Collections.IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        if (scrollRect != null && scrollRect.content != null)
        {
            // Force la mise à jour géométrique du UI Content
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
            scrollRect.verticalNormalizedPosition = 0f;
        }
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