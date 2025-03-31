using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creditsButton : MonoBehaviour
{
    public void loadCredits()
    {
            SceneManager.LoadScene("Credits");
            SceneManager.UnloadSceneAsync("MainMenu");
    }
}
