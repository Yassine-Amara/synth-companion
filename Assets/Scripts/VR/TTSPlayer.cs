using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class TTSRequest { public string text; }

public class TTSPlayer : MonoBehaviour
{
    public static TTSPlayer Instance;
    public AudioSource audioSource;
    private string ttsUrl = "http://localhost:8000/tts";
    private string audioFileUrl = "http://localhost:8000/static/output.mp3";

    void Awake() => Instance = this;

    public IEnumerator Speak(string text)
    {
        var req = new TTSRequest { text = text };
        string json = JsonUtility.ToJson(req);
        using var request = new UnityWebRequest(ttsUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Forcer le rechargement du fichier en évitant la mise en cache d'Unity
            string finalAudioUrl = audioFileUrl + "?t=" + System.DateTime.Now.Ticks;

            using var audioReq = UnityWebRequestMultimedia.GetAudioClip(finalAudioUrl, AudioType.MPEG);
            yield return audioReq.SendWebRequest();

            if (audioReq.result == UnityWebRequest.Result.Success)
            {
                if (audioSource != null)
                {
                    audioSource.clip = DownloadHandlerAudioClip.GetContent(audioReq);
                    audioSource.Play();
                }
            }
            else
            {
                Debug.LogWarning("Audio non charge : " + audioReq.error);
            }
        }
    }
}