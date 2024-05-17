using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualsController : MonoBehaviour
{
    [SerializeField] private Toggle toggleTerrain;
    [SerializeField] private Toggle toggleCamMovement;
    [SerializeField] private Toggle toggleMonsters;
    
    [SerializeField] private GameObject terrain;
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject monsters;
    [SerializeField] private GameObject animatedGoblins;
    
    [SerializeField] private List<RuntimeAnimatorController> goblinAnimControllers;

    private Main mainController;
    
    private bool camIsMoving;
    private bool monstersActive;
    
    private List<Animator> goblinAnimators;
    private int curGoblinIndex;
    private float nextGoblinUpdate;
    private float goblinUpdateInterval;
    
    public void OnInit(Main main)
    {
        mainController = main;
        
        goblinUpdateInterval = .5f;
        
        goblinAnimators = new List<Animator>();
        foreach(Transform child in animatedGoblins.transform)
        {
            var animator = child.GetComponent<Animator>();
            if (animator != null) {
                goblinAnimators.Add(animator);
            }
        }

        toggleTerrain.onValueChanged.AddListener(OnToggleTerrainChanged);
        toggleCamMovement.onValueChanged.AddListener(OnToggleCamMovementChanged);
        toggleMonsters.onValueChanged.AddListener(OnToggleMonstersChanged);
    }

    public void OnUpdate(float timer)
    {
        if (camIsMoving)
        {
            //mainCam.transform.Rotate(Vector3.up, 10.0f * Time.deltaTime);
            mainCam.transform.RotateAround(new Vector3(50,128, 50), Vector3.up, 15 * Time.deltaTime);
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

        if (mainController)
        {
            var sfxController = mainController.GetSfxController();
            if (sfxController) sfxController.SetFootsteps(camIsMoving);
        }
    }
    private void OnToggleMonstersChanged(bool isOn) {
        monsters.SetActive(isOn);
        monstersActive = isOn;
    }
}
