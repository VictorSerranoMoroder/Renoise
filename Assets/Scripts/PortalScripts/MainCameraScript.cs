using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCameraScript : MonoBehaviour
{
    PortalScript[] portals;
    ApparentWindowScript[] windows;
    AudioSource audioSource;

    private static readonly string BackgroundPrefs = "BackgroundPref";
    private float backgroundFloat;

    private void Awake()
    {
        portals = FindObjectsOfType<PortalScript>();
        windows = FindObjectsOfType<ApparentWindowScript>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Cursor.visible = false;
        backgroundFloat = PlayerPrefs.GetFloat(BackgroundPrefs);
        audioSource.volume = backgroundFloat;
        audioSource.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    private void OnPreCull()
    {
        for(int i=0;i<portals.Length;i++){
            if(portals.Length>0)
                portals[i].Render();
        }

        
        for(int i=0;i<windows.Length;i++){
            if (windows.Length > 0)
                windows[i].Render();
        }
        
        Debug.Log("Precull!");
    }
}
