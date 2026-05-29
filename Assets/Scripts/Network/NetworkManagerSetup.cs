using Unity.Netcode;
using UnityEngine;

public class NetworkManagerSetup : MonoBehaviour
{
    public GameObject aiPlayerPrefab;
    public GameObject ballPrefab;
    public GameObject lobbyPanel;
    public GameObject chatPanel;
    public GameObject puzzleStatus;
    public GameObject timerText;
    public GameObject sequenceProgressText;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        StartGame();
        if (aiPlayerPrefab != null)
        {
            var ai = Instantiate(aiPlayerPrefab);
            ai.GetComponent<NetworkObject>().Spawn();
        }
        if (ballPrefab != null)
        {
            // La balle spawn à sa position initiale
            var ball = Instantiate(ballPrefab, new Vector3(-2f, 1f, -8f), Quaternion.identity);
            ball.GetComponent<NetworkObject>().Spawn();

            var pp = FindFirstObjectByType<PlacementPuzzle>();
            if (pp != null) pp.ball = ball.transform;
        }
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        StartGame();
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