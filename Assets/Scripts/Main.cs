using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private VisualsController visualsController;
    [SerializeField] private MusicController musicController;
    [SerializeField] private SfxController sfxController;
    
    [SerializeField] private Button buttonQuit;
    
    // Start is called before the first frame update
    private void Start()
    {
        buttonQuit.onClick.AddListener(OnClickQuit);
    }

    private void OnClickQuit()
    {
        Application.Quit();
    }
}
