using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public AudioSource titleMusic;
    public Button newGame;
    public GameObject theStory;
    public GameObject storyReady;
    public bool storyActive;
    public float endScroll;
    // Start is called before the first frame update
    void Start()
    {
        titleMusic.Play();
        storyActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (titleMusic.time == 96)
        {
            titleMusic.Play();
            titleMusic.time = 14;
        }

        if (storyActive)
        {
            if (theStory.transform.position.y < endScroll)
            {
                theStory.transform.Translate(Vector2.up * 10f);
            }
            if (Input.GetButton("Cancel"))
            {
                loadSaveGame();
            }
        }
    }

    public void loadNewGame()
    {
        /*
        SAVE FILE ORGANIZATION:
        FIRST LINE IS THE STATS: LVL/VIT/MNA/EGY/STR/AGI/INT/DEF/LUK/xp
        SECOND LINE IS THE CURRENT CHECKPOINT
        THIRD LINE IS THE NAME OF CURRENT WEAPON AND QUICK SLOTS HERE! WEAPON/SLOT1/SLOT2/SLOT3
        THE NEXT LINES BELOW ARE INVENTORY SLOTS, SAVED AS: itemtype/name/description/damage/defense/hp/type/element/inscriptionamount
        IF INSCRIPTION AMOUNT IS MORE THAN 0, THE LINES BELOW WILL SAVE INSCRIPTIONS WITH FORMAT OF name/description/damage/defense/hp/specialEffect/element
        */

        string filePath = Path.Combine(Application.persistentDataPath, "MSSsave.txt");

        using (StreamWriter saveFile = new StreamWriter(filePath))
        {
            saveFile.WriteLine("1:12:12:5:7:7:7:7:5:0");
            saveFile.WriteLine("0");
            saveFile.WriteLine("Fae'r:Asha:Litha:Empty");
            saveFile.WriteLine("sword:Fae'r:A sword forged by Randall himself. It's spirit has been awakened by him as well.:3:3:4:Beautiful:Light:2");
            saveFile.WriteLine("Empty");
            saveFile.WriteLine("Empty");
            saveFile.WriteLine("spell:Asha:The Lowest Dark Magic:0:0:0:None:Dark:0");
            saveFile.WriteLine("spell:Litha:The Lowest Light Magic:0:0:0:None:Light:0");
            saveFile.Close();
        }
        theIntro();
    }

    public void loadSaveGame()
    {
        titleMusic.Stop();
        string filePath = Path.Combine(Application.persistentDataPath, "MSSsave.txt");

        int checkpoint = 0;
        using (StreamReader saveFile = new StreamReader(filePath))
        {
            saveFile.ReadLine();
            checkpoint = int.Parse(saveFile.ReadLine());
        }

        if (checkpoint == 2 || checkpoint == 3)
        {
            SceneManager.LoadScene("Village");
            SceneManager.UnloadSceneAsync("MainMenu");
        }
        else if (checkpoint == 4)
        {
            SceneManager.LoadScene("Zone2");
            SceneManager.UnloadSceneAsync("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("SeveringSword");
            SceneManager.UnloadSceneAsync("MainMenu");
        }
    }

    public void theIntro()
    {
        storyReady.SetActive(true);
        storyActive = true;
    }
}
