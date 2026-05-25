using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class AIPlayer : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        StartCoroutine(AILoop());
    }

    IEnumerator AILoop()
    {
        yield return new WaitForSeconds(15f);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20f, 45f));
            var pm = PuzzleManager.Instance;
            if (pm == null) continue;
            string context = $"P1:{pm.puzzle1State.Value} P2:{pm.puzzle2State.Value} P3:{pm.puzzle3Complete.Value}";
            string history = ChatManager.Instance != null ? ChatManager.Instance.GetLastMessages() : "aucun";
            yield return LLMConnector.Instance.AskLLM(context, history, (response) =>
            {
                StartCoroutine(TypingEffect(response));
            });
        }
    }

    IEnumerator TypingEffect(string msg)
    {
        yield return new WaitForSeconds(Random.Range(0.8f, 2.5f));
        SessionLogger.Instance?.Log("AI", "spoke", msg);
        ChatManager.Instance?.DisplayAIMessage(msg);
        if (TTSPlayer.Instance != null)
            yield return TTSPlayer.Instance.Speak(msg);
    }
}