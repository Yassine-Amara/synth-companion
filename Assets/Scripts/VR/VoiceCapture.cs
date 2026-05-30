using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Networking;

public class VoiceCapture : MonoBehaviour
{
    private AudioClip clip;
    private bool isRecording = false;
    private string sttUrl = "http://localhost:8000/stt";

    void Update()
    {
        // Desktop — touche T
        if (Keyboard.current != null)
        {
            if (Keyboard.current.tKey.wasPressedThisFrame && !isRecording)
                StartRecording();
            if (Keyboard.current.tKey.wasReleasedThisFrame && isRecording)
                StopAndSend();
        }

        // VR — bouton X (main gauche Quest 2)
        if (UnityEngine.XR.XRSettings.isDeviceActive)
        {
            var leftHand = InputSystem.GetDevice<UnityEngine.InputSystem.XR.XRController>(
                CommonUsages.LeftHand.ToString());
            if (leftHand != null)
            {
                var primaryButton = leftHand.GetChildControl<ButtonControl>("primaryButton");
                if (primaryButton != null)
                {
                    if (primaryButton.wasPressedThisFrame && !isRecording)
                        StartRecording();
                    if (primaryButton.wasReleasedThisFrame && isRecording)
                        StopAndSend();
                }
            }
        }
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
        string json = "{\"audio_path\": \"" + path.Replace("\\", "\\\\") + "\"}";
        using var request = new UnityWebRequest(sttUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            STTResponse response = JsonUtility.FromJson<STTResponse>(responseText);
            string transcribed = response.text.Trim();
            Debug.Log($"Transcrit : {transcribed}");
            ChatManager.Instance?.SendVoiceMessage(transcribed);
        }
    }

    [System.Serializable]
    class STTResponse { public string text; }
}