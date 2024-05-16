using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private VisualsController visualsController;
    [SerializeField] private MusicController musicController;
    [SerializeField] private SfxController sfxController;
    
    [SerializeField] private Button buttonQuit;
    
    [SerializeField] private AudioListener audioListener;
    
    // Start is called before the first frame update
    private void Start()
    {
        buttonQuit.onClick.AddListener(OnClickQuit);

        //audioListener.velocityUpdateMode = AudioVelocityUpdateMode.Auto;
        //Debug.Log(audioListener);
    }

    private void OnClickQuit()
    {
        Application.Quit();
    }
}
