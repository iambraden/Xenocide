using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsController : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValueText;
    
    private void Start()
    {
        // hide options panel
        optionsPanel.SetActive(false);
        
        // initialize slider with current volume
        volumeSlider.value = SoundManager.MasterVolume;
        UpdateVolumeText(volumeSlider.value);
        
        // listener for slider value changes
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }
    
    public void ToggleOptionsPanel()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
        
        // refresh slider value when opening panel
        if (optionsPanel.activeSelf)
        {
            volumeSlider.value = SoundManager.MasterVolume;
            UpdateVolumeText(volumeSlider.value);
        }
    }
    
    public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }
    
    private void OnVolumeChanged(float value)
    {
        // update the volume in SoundManager
        SoundManager.MasterVolume = value;
        
        // update text display
        UpdateVolumeText(value);
        
        // play a sound to demonstrate volume change
        SoundManager.PlaySound(SoundType.Hover, 0.5f);
    }
    
    private void UpdateVolumeText(float value)
    {
        if (volumeValueText != null)
        {
            volumeValueText.text = $"{Mathf.RoundToInt(value * 100)}%";
        }
    }
}
