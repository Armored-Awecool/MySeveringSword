using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource BGMLoop;
    public AudioSource TearsCannotMendScars;
    public AudioSource BekaStore;
    // Start is called before the first frame update
    void Start()
    {
        BGM.Play(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (BGM.isPlaying == false)
        {
            if (BGMLoop.isPlaying == false && TearsCannotMendScars.isPlaying == false && BekaStore.isPlaying == false)
            {
                BGMLoop.Play(0);
            }
        }
        else
        {
            BGMLoop.Stop();
        }
    }

    public void BekaStoreEnter()
    {
        BGM.Stop();
        BGMLoop.Stop();
        BekaStore.Play(0);
    }

    public void BekaStoreExit()
    {
        BekaStore.Stop();
        BGM.Play();
    }
    public void SetVolume(float set)
    {
        BGM.volume = set;
        BGMLoop.volume = set;
        BekaStore.volume = set;
        TearsCannotMendScars.volume = set;
    }

    public void gameOver()
    {
        BGM.Stop();
        BGMLoop.Stop();
        TearsCannotMendScars.Play(0);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SceneManager.UnloadSceneAsync("SeveringSword");
    }
}
