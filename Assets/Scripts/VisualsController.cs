using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualsController : MonoBehaviour
{
    [SerializeField] private Toggle toggleTerrain;
    [SerializeField] private Toggle toggleCamMovement;
    [SerializeField] private Toggle toggleMonsters;
    [SerializeField] private Toggle toggleFires;
    
    [SerializeField] private GameObject terrain;
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject monsters;
    [SerializeField] private GameObject animatedGoblins;
    
    [SerializeField] private List<RuntimeAnimatorController> goblinAnimControllers;
    
    [SerializeField] private Transform campfiresParent;
    [SerializeField] private GameObject prefabCampfire;
    
    private bool camIsMoving;
    private bool monstersActive;
    private bool firesActive;
    
    private List<Animator> goblinAnimators;
    private int curGoblinIndex;
    private float nextGoblinUpdate;
    private float goblinUpdateInterval;
    private float timer;
    
    private List<GameObject> fireObjects;
    private int numberOfFires;
    
    // Start is called before the first frame update
    private void Start()
    {
        goblinUpdateInterval = .5f;
        
        goblinAnimators = new List<Animator>();
        foreach(Transform child in animatedGoblins.transform)
        {
            var animator = child.GetComponent<Animator>();
            if (animator != null) {
                goblinAnimators.Add(animator);
            }
        }

        numberOfFires = 48;
            
        toggleTerrain.onValueChanged.AddListener(OnToggleTerrainChanged);
        toggleCamMovement.onValueChanged.AddListener(OnToggleCamMovementChanged);
        toggleMonsters.onValueChanged.AddListener(OnToggleMonstersChanged);
        toggleFires.onValueChanged.AddListener(OnToggleFiresChanged);
    }

    private void Update()
    {
        timer = Time.realtimeSinceStartup;
        
        if (camIsMoving)
        {
            mainCam.transform.Rotate(Vector3.up, 5.0f * Time.deltaTime);
        }

        if (!monstersActive) return;
        
        if (timer >= nextGoblinUpdate)
        {
            nextGoblinUpdate = timer + goblinUpdateInterval;
            var randAnim = Random.Range(0, goblinAnimControllers.Count);
            //Debug.Log("randAnim, goblinAnimControllers.Count: "+randAnim+", "+goblinAnimControllers.Count);
            goblinAnimators[curGoblinIndex].runtimeAnimatorController = goblinAnimControllers[randAnim];

            if (++curGoblinIndex >= goblinAnimators.Count) curGoblinIndex = 0;
        }
    }

    private void CreateFires()
    {
        fireObjects ??= new List<GameObject>();

        for (var i = 0; i < numberOfFires; ++i)
        {
            var randomPoint = Vector2.zero + Random.insideUnitCircle * 500 * 0.5f;
            var v3Pos = new Vector3(randomPoint.x, 120, randomPoint.y);
            var fire = Instantiate(prefabCampfire, v3Pos, Quaternion.Euler(0, 0, 0), campfiresParent);
            fireObjects.Add(fire);
        }
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
    
    private void OnToggleTerrainChanged(bool isOn)
    {
        terrain.SetActive(isOn);
    }
    private void OnToggleCamMovementChanged(bool isOn)
    {
        camIsMoving = isOn;
    }
    private void OnToggleMonstersChanged(bool isOn) {
        monsters.SetActive(isOn);
        monstersActive = isOn;
    }
    private void OnToggleFiresChanged(bool isOn) {
        firesActive = isOn;
        if (firesActive) CreateFires();
        else DestroyFires();
    }
}
