using Unity.Netcode;
using UnityEngine;

public class NetworkManagerSetup : MonoBehaviour
{
    public GameObject aiPlayerPrefab;
    public GameObject ballPrefab;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        if (aiPlayerPrefab != null)
        {
            var ai = Instantiate(aiPlayerPrefab);
            ai.GetComponent<NetworkObject>().Spawn();
        }
        if (ballPrefab != null)
        {
            var ball = Instantiate(ballPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            ball.GetComponent<NetworkObject>().Spawn();
            var pp = FindObjectOfType<PlacementPuzzle>();
            if (pp != null) pp.ball = ball.transform;
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton est null !");
            return;
        }
        NetworkManager.Singleton.StartClient();
    }
}