using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject endPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimeText;
    private float elapsed = 0;
    private bool running = true;

    void Awake() => Instance = this;

    void Update()
    {
        if (!running) return;
        elapsed += Time.deltaTime;
        int min = (int)(elapsed / 60);
        int sec = (int)(elapsed % 60);
        timerText.text = $"{min:00}:{sec:00}";
        if (PuzzleManager.Instance != null && PuzzleManager.Instance.AllComplete())
        {
            running = false;
            if (finalTimeText != null) finalTimeText.text = timerText.text;
            if (endPanel != null) endPanel.SetActive(true);
            SessionLogger.Instance?.Log("system", "game_complete");
        }
    }
}