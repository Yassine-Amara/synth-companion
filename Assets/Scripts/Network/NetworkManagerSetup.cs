using Unity.Netcode;
using UnityEngine;

public class NetworkManagerSetup : MonoBehaviour
{
    public GameObject desktopPlayerPrefab;
    public GameObject vrPlayerPrefab;
    public GameObject aiPlayerPrefab;
    public GameObject ballPrefab;
    public GameObject lobbyPanel;
    public GameObject chatPanel;
    public GameObject puzzleStatus;
    public GameObject timerText;
    public GameObject sequenceProgressText;

    public void StartHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.StartHost();
        StartGame();
        if (aiPlayerPrefab != null)
        {
            var ai = Instantiate(aiPlayerPrefab);
            ai.GetComponent<NetworkObject>().Spawn();
        }
        if (ballPrefab != null)
        {
            var ball = Instantiate(ballPrefab, new Vector3(-2f, 1f, -8f), Quaternion.identity);
            ball.GetComponent<NetworkObject>().Spawn();
            var pp = FindObjectOfType<PlacementPuzzle>();
            if (pp != null) pp.ball = ball.transform;
        }
    }

    public void StartClient()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.StartClient();
        StartGame();
    }

    void OnClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        bool isVR = UnityEngine.XR.XRSettings.isDeviceActive;
        GameObject prefab = isVR ? vrPlayerPrefab : desktopPlayerPrefab;
        if (prefab != null)
        {
            var player = Instantiate(prefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }

    void StartGame()
    {
        if (lobbyPanel != null) lobbyPanel.SetActive(false);
        if (chatPanel != null) chatPanel.SetActive(true);
        if (puzzleStatus != null) puzzleStatus.SetActive(true);
        if (timerText != null) timerText.SetActive(true);
        if (sequenceProgressText != null) sequenceProgressText.SetActive(true);
    }
}