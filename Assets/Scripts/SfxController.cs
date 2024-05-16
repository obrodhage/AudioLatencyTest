using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SfxController : MonoBehaviour
{
    [SerializeField] private Toggle toggleFires;

    [SerializeField] private Transform campfiresParent;
    [SerializeField] private GameObject prefabCampfire;
       
    private bool firesActive;
    
    private List<GameObject> fireObjects;
    private int numberOfFires;
    
    // Start is called before the first frame update
    private void Start()
    {
        numberOfFires = 100;
            
        toggleFires.onValueChanged.AddListener(OnToggleFiresChanged);
    }

    private void Update()
    {
        if (!firesActive) return;
        
        fireObjects ??= new List<GameObject>();

        if (fireObjects.Count < numberOfFires)
        {
            CreateFires();
        }
    }
    
    private void CreateFires()
    {
        //for (var i = 0; i < numberOfFires; ++i)
        //{
            var randomPoint = Vector2.zero + Random.insideUnitCircle * (750 * 0.5f);
            var v3Pos = new Vector3(randomPoint.x, 120, randomPoint.y);
            var fire = Instantiate(prefabCampfire, v3Pos, Quaternion.Euler(0, 0, 0), campfiresParent);
            fireObjects.Add(fire);
        //}
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
}
