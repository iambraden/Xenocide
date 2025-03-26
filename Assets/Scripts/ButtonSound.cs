using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour, 
    IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private float hoverVolume = 0.8f;
    [SerializeField] private float clickVolume = 1f;

    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (button.interactable) {
            SoundManager.PlaySound(SoundType.Hover, hoverVolume);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (button.interactable) {
            SoundManager.PlaySound(SoundType.Click, clickVolume);
        }
    }
}