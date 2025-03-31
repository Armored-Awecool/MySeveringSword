using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SettingsMenu : MonoBehaviour
{
    public float volume = .5f;
    public GameObject menu;
    public AudioManager AudioManager;
    public bool fs = true;
    public int size = 1;
    public RandallMovement RandallMovement;
    public bool VIThidden = false;
    // Start is called before the first frame update
    void Start()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "MSSsettingssave.txt");

        if (File.Exists(filePath))
        {
            using (StreamReader saveFile = new StreamReader(filePath))
            {
                string[] values = saveFile.ReadLine().Split(':');
                volume = float.Parse(values[0]);
                fs = bool.Parse(values[1]);
                size = int.Parse(values[2]);
                VIThidden = bool.Parse(values[3]);
                RandallMovement.VIThidden = VIThidden;
            }
        }
        AudioManager.SetVolume(volume);
        if (fs != true)
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        if (size == 1)
        {
            setResBig();
        }
        if (size == 2)
        {
            setResSmall();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            menu.SetActive(false);
        }
    }

    public void setVolume(float input)
    {
        volume = input;
        AudioManager.SetVolume(volume);
        saveSettings();
    }

    public void openSettings()
    {
        menu.SetActive(true);
    }

    public void fullscreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
        fs = !fs;
        saveSettings();
    }

    public void setResBig()
    {
        Screen.SetResolution(1920, 1080, true);
        size = 1;
        saveSettings();
    }

    public void setResSmall()
    {
        Screen.SetResolution(640, 480, true);
        size = 2;
        saveSettings();
    }

    public void setVITUI()
    {
        RandallMovement.VIThidden = !RandallMovement.VIThidden;
        VIThidden = !VIThidden;
    }

    public void saveSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "MSSsettingssave.txt");

        using (StreamWriter saveFile = new StreamWriter(filePath))
        {
            saveFile.WriteLine(volume + ":" + fs + ":" + size + ":" + VIThidden);
        }
    }
}
