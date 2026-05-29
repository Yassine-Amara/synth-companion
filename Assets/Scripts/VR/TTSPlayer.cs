using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

[System.Serializable]
public class TTSRequest { public string text; public string output_path; }

public class TTSPlayer : MonoBehaviour
{
    public static TTSPlayer Instance;
    public AudioSource audioSource;
    private string ttsUrl = "http://localhost:8000/tts";

    void Awake() => Instance = this;

    public IEnumerator Speak(string text)
    {
        string savePath = Application.persistentDataPath + "/output.mp3";
        var req = new TTSRequest { text = text, output_path = savePath };
        string json = JsonUtility.ToJson(req);
        using var request = new UnityWebRequest(ttsUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string mp3Path = "file://" + savePath;
            using var audioReq = UnityWebRequestMultimedia.GetAudioClip(mp3Path, AudioType.MPEG);
            yield return audioReq.SendWebRequest();
            if (audioReq.result == UnityWebRequest.Result.Success)
            {
                audioSource.clip = DownloadHandlerAudioClip.GetContent(audioReq);
                audioSource.Play();
            }
        }
    }
}