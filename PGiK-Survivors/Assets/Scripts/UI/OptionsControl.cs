using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsControl : MonoBehaviour {
    [Header( "General" )]
    [field: SerializeField] private GameObject optionsArea;

    [Header( "Audio Settings" )]
    [field: SerializeField] private Slider masterSlider;
    [field: SerializeField] private Slider musicSlider;
    [field: SerializeField] private Slider sfxSlider;

    [Header( "Buttons" )]
    [field: SerializeField] private Button backButton;

    private void Awake() {
        Deactivate();
    }

    private void Start() {
        SetupAudioSliders();
    }

    private void SetupAudioSliders() {
        masterSlider.value = AudioManager.Instance.GetMasterVolume();
        musicSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetEffectsVolume();
    }


    public void OnMasterChange() {
        AudioManager.Instance.SetMasterVolume( masterSlider.value );
        SaveChanges();
    }

    public void OnMusicChange() {
        AudioManager.Instance.SetMusicVolume( musicSlider.value );
        SaveChanges();
    }

    public void OnSFXChange() {
        AudioManager.Instance.SetSoundFXVolume( sfxSlider.value );
        SaveChanges();
    }

    public void SaveChanges() {
        if ( !SaveControl.Instance.HasSaveData() ) SaveControl.Instance.NewGame();

        SaveControl.Instance.SaveGame();
    }

    public void Activate() { SetupAudioSliders(); SelectFirstElement(); optionsArea.SetActive( true ); }
    public void Deactivate() { optionsArea.SetActive( false ); }

    private void SelectFirstElement() {
        masterSlider.Select();
    }
}
