using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.InputSystem;

public class VoiceCapture : MonoBehaviour
{
    private AudioClip clip;
    private bool isRecording = false;
    private string sttUrl = "http://localhost:8000/stt";

    void Update()
    {
        // Maintiens T pour parler, relache pour envoyer
        if (Keyboard.current.tKey.wasPressedThisFrame && !isRecording)
            StartRecording();
        if (Keyboard.current.tKey.wasReleasedThisFrame && isRecording)
            StopAndSend();
    }

    void StartRecording()
    {
        isRecording = true;
        clip = Microphone.Start(null, false, 10, 16000);
        Debug.Log("Enregistrement vocal...");
    }

    void StopAndSend()
    {
        isRecording = false;
        Microphone.End(null);
        string path = Application.persistentDataPath + "/voice_input.wav";
        SavWav.Save(path, clip);
        StartCoroutine(SendToWhisper(path));
    }

    IEnumerator SendToWhisper(string path)
    {
        string json = "{\"audio_path\": \"" + path + "\"}";
        using var request = new UnityWebRequest(sttUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            string result = request.downloadHandler.text;
            string text = result.Replace("{\"text\": \"", "").Replace("\"}", "");
            ChatManager.Instance?.SendVoiceMessage(text);
            Debug.Log($"Transcrit : {text}");
        }
    }
}