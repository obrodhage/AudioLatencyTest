using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdownAudioFormat;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonPause;
    [SerializeField] private Button buttonStop;
    [SerializeField] private Toggle toggleLayer0;
    [SerializeField] private Toggle toggleLayer1;
    [SerializeField] private Toggle toggleLayer2;
    [SerializeField] private Toggle toggleLayer3;
    [SerializeField] private Toggle toggleLayer4;
    [SerializeField] private TMP_Text textLayer0;
    [SerializeField] private TMP_Text textLayer1;
    [SerializeField] private TMP_Text textLayer2;
    [SerializeField] private TMP_Text textLayer3;
    [SerializeField] private TMP_Text textLayer4;
    [SerializeField] private Toggle toggleRandom;
    
    [SerializeField] private List<AudioSource> audioSourcesOgg;
    [SerializeField] private List<AudioSource> audioSourcesWav;
    [SerializeField] private List<AudioSource> audioSourcesMp3;

    private Main mainController;
    
    private int selectedAudioFormat;

    private List<AudioSource> activeAudioSources;
    private List<bool> audioLayerActive;
    
    private const int AudioLayers = 5;

    private bool isPlaying;
    private bool isPaused;
    private bool randomLayers;
    
    private float sampleDisplayUpdate;
    private float sampleDisplayUpdateInterval;
    
    private float nextRandomLayerUpdate;
    private float randomLayerUpdateInterval;
    
    public void OnInit(Main main)
    {
        mainController = main;
        
        selectedAudioFormat = dropdownAudioFormat.value;

        activeAudioSources = new List<AudioSource>();
        audioLayerActive = new List<bool>();
        for (var i = 0; i < AudioLayers; ++i) {
            activeAudioSources.Add(new AudioSource());
            audioLayerActive.Add(true);
        }

        sampleDisplayUpdateInterval = 1f;
        randomLayerUpdateInterval = 2f;
            
        dropdownAudioFormat.onValueChanged.AddListener(OnAudioFormatChanged);
        buttonPlay.onClick.AddListener(OnClickPlay);
        buttonPause.onClick.AddListener(OnClickPause);
        buttonStop.onClick.AddListener(OnClickStop);
        toggleLayer0.onValueChanged.AddListener(OnToggleLayer0Changed);
        toggleLayer1.onValueChanged.AddListener(OnToggleLayer1Changed);
        toggleLayer2.onValueChanged.AddListener(OnToggleLayer2Changed);
        toggleLayer3.onValueChanged.AddListener(OnToggleLayer3Changed);
        toggleLayer4.onValueChanged.AddListener(OnToggleLayer4Changed);
        toggleRandom.onValueChanged.AddListener(OnToggleRandomChanged);

        UpdateControlButtons();
        ResetSampleTextFields();
    }

    public void OnUpdate(float timer)
    {
        if (!isPlaying) return;
        
        if (timer >= sampleDisplayUpdate)
        {
            sampleDisplayUpdate = timer + sampleDisplayUpdateInterval;
            DisplayAudioSourceSamples();
        }
        
        if (!randomLayers) return;
        
        if (timer >= nextRandomLayerUpdate)
        {
            nextRandomLayerUpdate = timer + randomLayerUpdateInterval;
            var randLayer = Random.Range(1, AudioLayers+1);

            if (randLayer == 1) toggleLayer1.isOn = !toggleLayer1.isOn;
            else if (randLayer == 2) toggleLayer2.isOn = !toggleLayer2.isOn;
            else if (randLayer == 3) toggleLayer3.isOn = !toggleLayer3.isOn;
            else if (randLayer == 4) toggleLayer4.isOn = !toggleLayer4.isOn;
            
            DisplayAudioSourceSamples();
        }
    }
    
    //
    // Dropdown Action
    //
    
    private void OnAudioFormatChanged(int value)
    {
        selectedAudioFormat = value;
        
        if (isPlaying || isPaused)
        {
            StopAllAudioSources();
            
            if (isPlaying) PlayAllAudioSources(true);
        }
        
        ResetSampleTextFields();
    }
    
    //
    // Play Controls
    //
    
    private void OnClickPlay()
    {
        PlayAllAudioSources();
        
        isPlaying = true;
        isPaused = false;
        UpdateControlButtons();
        
        ResetSampleTextFields();
    }

    private void OnClickPause()
    {
        if (isPaused) return;
        
        PauseAllAudioSources();
        
        isPlaying = false;
        isPaused = true;
        UpdateControlButtons();
        
        DisplayAudioSourceSamples();
    }

    private void OnClickStop()
    {
        StopAllAudioSources();
        
        isPlaying = false;
        isPaused = false;
        UpdateControlButtons();
        
        ResetSampleTextFields();
    }

    private void PlayAllAudioSources(bool resetSource = false)
    {
        if (selectedAudioFormat == 0) PlayAudio(audioSourcesOgg, resetSource);
        else if (selectedAudioFormat == 1) PlayAudio(audioSourcesWav, resetSource);
        else PlayAudio(audioSourcesMp3, resetSource);
    }
    
    private void PlayAudio(List<AudioSource> sources, bool resetSource)
    {
        for (var i = 0; i < AudioLayers; ++i)
        //for (var i = AudioLayers-1; i >= 0; --i)
        {
            if (resetSource) {
                activeAudioSources[i] = null;
            }

            if (!activeAudioSources[i]) {
                activeAudioSources[i] = sources[i];
                activeAudioSources[i].mute = false;
                activeAudioSources[i].spatialize = false;
                activeAudioSources[i].spatializePostEffects = false;
                activeAudioSources[i].bypassEffects = true;
                activeAudioSources[i].bypassListenerEffects = true;
                activeAudioSources[i].bypassReverbZones = true;
                activeAudioSources[i].playOnAwake = false;
                activeAudioSources[i].loop = true;
                //activeAudioSources[i].volume = 0.25f;
                activeAudioSources[i].spatialBlend = 0f;
            }

            if (audioLayerActive[i]) {
                activeAudioSources[i].Play();
            }
        }
    }

    private void PauseAllAudioSources()
    {
        for (var i = 0; i < AudioLayers; ++i)
        {
            if (activeAudioSources[i]) {
                activeAudioSources[i].Pause();
            }
        }
    }
    
    private void StopAllAudioSources()
    {
        for (var i = 0; i < AudioLayers; ++i)
        {
            if (activeAudioSources[i]) {
                activeAudioSources[i].Stop();
            }
        }
    }

    private void UpdateControlButtons()
    {
        buttonPlay.interactable = isPaused || !isPlaying;
        buttonPause.interactable = isPlaying;
        buttonStop.interactable = isPlaying;
    }

    private void ResetSampleTextFields()
    {
        textLayer0.text = "";
        textLayer1.text = "";
        textLayer2.text = "";
        textLayer3.text = "";
        textLayer4.text = "";
    }

    private void DisplayAudioSourceSamples()
    {
        var baseSample = !activeAudioSources[0] ? 0 : activeAudioSources[0].timeSamples;
        textLayer0.text = "Current timeSample: " + baseSample;
        
        textLayer1.text = GetAudioTimeSample(baseSample, 1);
        textLayer2.text = GetAudioTimeSample(baseSample, 2);
        textLayer3.text = GetAudioTimeSample(baseSample, 3);
        textLayer4.text = GetAudioTimeSample(baseSample, 4);
    }

    private string GetAudioTimeSample(int baseSample, int index)
    {
        var s = "";
        
        if (!activeAudioSources[index]) return s;

        if (!activeAudioSources[index].isPlaying) return s;
        
        var sample = activeAudioSources[index].timeSamples;
        s = "Current timeSample: " + sample;
        s += (sample - baseSample != 0) ? "  :(" : "";

        return s;
    }
    
    //
    // Toggle it!
    //
    
    private void ToggleLayer(int layer, bool isOn)
    {
        var audioSource = activeAudioSources[layer];

        if (isOn)
        {
            audioSource.timeSamples = activeAudioSources[0].timeSamples;
            if (isPlaying) {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

        audioLayerActive[layer] = isOn;
    }
    
    private void OnToggleLayer0Changed(bool isOn) {
        // ToggleLayer(0, isOn);
    }
    private void OnToggleLayer1Changed(bool isOn) {
        ToggleLayer(1, isOn);
    }
    private void OnToggleLayer2Changed(bool isOn) {
        ToggleLayer(2, isOn);
    }
    private void OnToggleLayer3Changed(bool isOn) {
        ToggleLayer(3, isOn);
    }
    private void OnToggleLayer4Changed(bool isOn) {
        ToggleLayer(4, isOn);
    }
    
    private void OnToggleRandomChanged(bool isOn)
    {
        randomLayers = isOn;
    }
}
