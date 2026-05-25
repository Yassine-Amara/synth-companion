using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class DialogueRequest { public string game_context; public string chat_history; }
[System.Serializable]
public class DialogueResponse { public string text; }

public class LLMConnector : MonoBehaviour
{
    public static LLMConnector Instance;
    private string dialogueUrl = "http://localhost:8000/dialogue";

    void Awake() => Instance = this;

    public IEnumerator AskLLM(string context, string history, System.Action<string> callback)
    {
        var req = new DialogueRequest { game_context = context, chat_history = history };
        string json = JsonUtility.ToJson(req);
        using var request = new UnityWebRequest(dialogueUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = JsonUtility.FromJson<DialogueResponse>(request.downloadHandler.text);
            callback(response.text);
        }
        else callback(GetFallback());
    }

    string GetFallback()
    {
        string[] f = { "ok je cherche", "j ai une idee", "aidez moi", "on manque de temps!!" };
        return f[Random.Range(0, f.Length)];
    }
}