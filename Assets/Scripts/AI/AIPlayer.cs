using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class AIPlayer : NetworkBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 2f;
    private float minX = 1f, maxX = 4f;
    private float minZ = -8f, maxZ = -3f;
    private float fixedY = 1f;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        StartCoroutine(AILoop());
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        targetPosition = transform.position;
        while (true)
        {
            targetPosition = new Vector3(
                Random.Range(minX, maxX),
                fixedY,
                Random.Range(minZ, maxZ)
            );
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    void Update()
    {
        if (!IsServer) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        Vector3 direction = targetPosition - transform.position;
        if (direction.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, rotation, 5f * Time.deltaTime);
        }
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

        // Envoie au chat, ce qui déclenchera l'audio sur tous les clients connectés
        ChatManager.Instance?.DisplayAIMessage(msg);
    }
}