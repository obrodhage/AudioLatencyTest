using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SfxController : MonoBehaviour
{
    [SerializeField] private Toggle toggleFires;
    [SerializeField] private Toggle toggleFootsteps;
    
    [SerializeField] private Transform campfiresParent;
    [SerializeField] private GameObject prefabCampfire;
       
    [SerializeField] private AudioSource audioSourcesFootstep;
    
    private Main mainController;
    
    private bool firesActive;
    
    private List<GameObject> fireObjects;
    private int numberOfFires;
    private List<Campfire> campfires;
    
    private float campfiresUpdate;
    private float campfiresUpdateInterval;

    private bool footstepsActive;
    private float footstepsUpdate;
    private float footstepsUpdateInterval;
    
    public void OnInit(Main main)
    {
        mainController = main;
        
        numberOfFires = 100;

        campfiresUpdateInterval = .5f;
        footstepsUpdateInterval = .3f;
        
        toggleFires.onValueChanged.AddListener(OnToggleFiresChanged);
        toggleFootsteps.onValueChanged.AddListener(OnToggleFootstepsChanged);
    }

    public void OnUpdate(float timer)
    {
        if (firesActive)
        {
            fireObjects ??= new List<GameObject>();
            campfires ??= new List<Campfire>();

            if (fireObjects.Count < numberOfFires)
            {
                CreateFire();
            }
            else
            {
                if (timer >= campfiresUpdate)
                {
                    campfiresUpdate = timer + campfiresUpdateInterval;

                    var randFire = Random.Range(0, campfires.Count);
                    if (campfires[randFire]) campfires[randFire].PlayAudioSourceTwo();
                }
            }
        }
        
        if (!footstepsActive) return;
        
        if (timer >= footstepsUpdate)
        {
            var randInterval = Random.Range(10, 16);
            footstepsUpdate = timer + footstepsUpdateInterval + (randInterval * .01f);
                
            var randPitch = Random.Range(9, 12);
            audioSourcesFootstep.pitch = randPitch * .1f;
            var randVol = Random.Range(12, 16);
            audioSourcesFootstep.volume = randVol * .1f;
            
            audioSourcesFootstep.Play();
        }
    }

    public void SetFootsteps(bool active)
    {
        toggleFootsteps.interactable = active;
        if (!active) {
            toggleFootsteps.isOn = false;
            footstepsActive = false;
        }
    }
    
    private void CreateFire()
    {
        var randomPoint = Vector2.zero + Random.insideUnitCircle * (750 * 0.5f);
        var v3Pos = new Vector3(randomPoint.x, 120, randomPoint.y);
        var fire = Instantiate(prefabCampfire, v3Pos, Quaternion.Euler(0, 0, 0), campfiresParent);
        fireObjects.Add(fire);

        var mono = fire.GetComponent<Campfire>();
        if (mono) campfires.Add(mono);
    }

    private void DestroyFires()
    {
        if (fireObjects == null) return;

        foreach (var fire in fireObjects) {
            Destroy(fire);
        }
        
        fireObjects = new List<GameObject>();
    }
    
    //
    // Toggle it!
    //
    
    private void OnToggleFiresChanged(bool isOn) {
        firesActive = isOn;
        if (!firesActive) DestroyFires();
    }

    private void OnToggleFootstepsChanged(bool isOn) {
        footstepsActive = isOn;
    }
}
