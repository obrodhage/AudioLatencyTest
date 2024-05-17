using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private VisualsController visualsController;
    [SerializeField] private MusicController musicController;
    [SerializeField] private SfxController sfxController;
    
    [SerializeField] private Button buttonQuit;
    
    [SerializeField] private AudioListener audioListener;
    
    private float timer;

    public SfxController GetSfxController()
    {
        return sfxController;
    }

    private void Start()
    {
        visualsController.OnInit(this);
        musicController.OnInit(this);
        sfxController.OnInit(this);
        
        buttonQuit.onClick.AddListener(OnClickQuit);
    }

    private void Update()
    {
        timer = Time.realtimeSinceStartup;

        visualsController.OnUpdate(timer);
        musicController.OnUpdate(timer);
        sfxController.OnUpdate(timer);
    }
    
    private void OnClickQuit()
    {
        Application.Quit();
    }
}