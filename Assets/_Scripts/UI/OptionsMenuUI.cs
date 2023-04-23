using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Script for dealing with visual components of the UI and application customisation logic.
 * Contains functions for dealing with typicial operations found in Settings.
 * Used in both MainMenu and GameScene.
 */
public class OptionsMenuUI : AbstractMainMenu {

    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider volumeSlider;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private int currentResolutionIndex;

    private void Awake() {
        HideMenu();
    }

    //Initialise dropdown options for resolution.
    private void Start() {

        resolutions = Screen.resolutions;
        currentResolutionIndex = 0;

        filteredResolutions = new List<Resolution>();
        List<string> resolutionOptions = new List<string>();

        //Filter resolutions based on fresh rate so there are no duplicate resolutions of same width/height.
        for(int i = 0; i < resolutions.Length; i++) {
            if(resolutions[i].refreshRate == Screen.currentResolution.refreshRate) {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        //Create a list of valid resolution options for the dropdown from the filtered list.
        for(int i = 0; i < filteredResolutions.Count; i++) {
            string optionValue = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
            resolutionOptions.Add(optionValue);

            //Identify the Player's current resolution whilst looping through resolutions.
            if(filteredResolutions[i].width == Screen.width &&
                filteredResolutions[i].height == Screen.height) {

                currentResolutionIndex = i;

            }
        }

        //Initiate current Player resolution/s.
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    //Button functions.
    public void CloseClicked() {
        Debug.Log("Closing Options pop-up.");

        //Hide the pop-up.
        this.HideMenu();
    }

    public void ApplyClicked() {
        Debug.Log("Apply clicked.");

        //Hide the pop-up.
        this.HideMenu();
    }

    //Toggle between fullscreen and windowed mode.
    public void ToggleFullscreen(bool isFullscreen) {

        if(isFullscreen) {
            Debug.Log("Set to fullscreen.");
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        } else {
            Debug.Log("Set to windowed.");
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }

    }

    //Set the resolution when changed.
    public void SetResolution(int resolutionIndex) {
        Resolution newResolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen);

        //Save index for when opening OptionsMenu in another Scene.
        currentResolutionIndex = resolutionIndex;
    }

    //Set the volume based on what is currently designated on the slider.
    public void SetVolumeFromVolumeSlider() {
        SoundSystem.INSTANCE.SetVolume(volumeSlider.value);
    }

    //Ensure selected options are correctly displayed when opening Options Menu in another Scene.
    public void InitialiseOptions() {
        fullscreenToggle.isOn = Screen.fullScreen;
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        ShowMenu();
    }
    
}
