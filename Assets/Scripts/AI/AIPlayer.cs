using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class AIPlayer : NetworkBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 2f;
    private float minX = -3f, maxX = 3f;
    private float minZ = -3f, maxZ = 3f;
    private float fixedY = 1f;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        StartCoroutine(AILoop());
        StartCoroutine(MoveLoop());
    }

    // Mouvement aleatoire
    IEnumerator MoveLoop()
    {
        targetPosition = transform.position;
        while (true)
        {
            // Choisit une nouvelle position aleatoire dans la salle
            targetPosition = new Vector3(
                Random.Range(minX, maxX),
                fixedY,
                Random.Range(minZ, maxZ)
            );
            // Attend entre 3 et 8 secondes avant de bouger encore
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    void Update()
    {
        if (!IsServer) return;
        // Deplace l'IA vers la cible doucement
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
        // Tourne vers la direction du mouvement
        Vector3 direction = targetPosition - transform.position;
        if (direction.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, rotation, 5f * Time.deltaTime);
        }
    }

    // Chat IA
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