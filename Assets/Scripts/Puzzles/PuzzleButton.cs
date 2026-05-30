using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleButton : MonoBehaviour, IPointerClickHandler
{
    public int buttonId;
    private Renderer rend;
    private Color originalColor;
    private Color clickedColor = Color.white;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    // Clic souris (Desktop)
    public void OnPointerClick(PointerEventData eventData)
    {
        Activate();
    }

    // Appele aussi depuis XR Simple Interactable via Unity Events dans l'Inspector
    public void Activate()
    {
        SequencePuzzle.Instance.PressButtonServerRpc(buttonId);
        StartCoroutine(FlashEffect());
    }

    public System.Collections.IEnumerator FlashEffect()
    {
        if (rend != null)
        {
            rend.material.color = clickedColor;
            yield return new WaitForSeconds(0.3f);
            rend.material.color = originalColor;
        }
    }
}