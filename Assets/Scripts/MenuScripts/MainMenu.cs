using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider musicSlider;
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundPrefs = "BackgroundPref";
    private float backgroundFloat;
    private int firstPlayInt;
    public AudioSource backgroundAudio;

    private void Start()
    {

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if(firstPlayInt == 0)
        {
            backgroundFloat = .5f;
            musicSlider.value = backgroundFloat;
            PlayerPrefs.SetFloat(BackgroundPrefs, backgroundFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            backgroundFloat = PlayerPrefs.GetFloat(BackgroundPrefs);
            musicSlider.value = backgroundFloat;
        }
    }

    private void Update()
    {
        backgroundAudio.volume = musicSlider.value;
        saveVolumeSettings();
    }

    public void saveVolumeSettings()
    {
        PlayerPrefs.SetFloat(BackgroundPrefs, musicSlider.value);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            saveVolumeSettings();
    }

    public void updateSound()
    {
        backgroundAudio.volume = musicSlider.value;
    }


    public void PlayMainLvl()
    {
        SceneManager.LoadScene("Level Concept");
    }

    public void PlayDemoPortales()
    {
        SceneManager.LoadScene("PortalDemo");
    }

    public void PlayDemoPerspectiva()
    {
        SceneManager.LoadScene("ForcedPerspectiveDemo");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
