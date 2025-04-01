using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class RandallMovement : MonoBehaviour
{
    public GameObject Randall;
    public float speed;
    public float jumpSpeed;
    public bool onGround = true;
    public Rigidbody2D rb;
    public GameObject inventory;
    public GameObject noEye;
    public GameObject pinkEye;
    public GameObject purpleEye;
    public GameObject bothEye;
    public GameObject leftFoot;
    public GameObject rightFoot;
    public int eyeState; //0 is none, 1 is pink, 2 is purple, 3 is both
    public bool menuShown;
    public Animator leftFootanimator;
    public Animator rightFootanimator;
    public Animator bodyanimator;
    public float position;
    public TextMeshProUGUI VITtext;
    public TextMeshProUGUI MNAtext;
    public TextMeshProUGUI EGYtext;
    public TextMeshProUGUI STRtext;
    public TextMeshProUGUI AGItext;
    public TextMeshProUGUI INTtext;
    public TextMeshProUGUI DEFtext;
    public TextMeshProUGUI LUKtext;
    public TextMeshProUGUI CurrentHP;
    public TextMeshProUGUI leveltext;
    public GameObject CHP;
    public int currentVIT;
    public int level;
    public int VIT;
    public int MNA;
    public int EGY;
    public int STR;
    public int AGI;
    public int INT;
    public int DEF;
    public int LUK;
    public TheInventory TheInventory;
    public Button[] items;
    public Button[] spells;
    public TextMeshProUGUI equipToolTip;
    public GameObject FaerLeft;
    public GameObject FaerRight;
    public Transform[] CheckPoints;
    public int xp;
    public bool VIThidden;
    public int currentMNA;
    public TextMeshProUGUI CurrentMNA;
    public int equipSlotWait;
    public GameObject asha;
    public GameObject litha;
    public Transform bulletSpawn;
    public float firecooldown;
    public float cooldownreset;
    public float manarecover;
    public AudioManager AudioManager;
    public GameObject gameOverScreen;
    public AudioSource grassStep;
    public GameObject BekaStoreMenu;
    public Material damageShader;
    private bool damaged;
    private float damageTime = 0.3f;
    private float damageTimer;

    public string direction = "right";
    void Start()
    {
        leftFootanimator = GetComponent<Animator>();
        rightFootanimator = GetComponent<Animator>();
        bodyanimator = GetComponent<Animator>();
        inventory.SetActive(false);
        menuShown = false;
        eyeState = 0;
        speed = 5.0f;
        jumpSpeed = 10.0f;
        leftFootanimator.SetBool("motion", false);
        rightFootanimator.SetBool("motion", false);
        position = rb.position.x;
        TheInventory = GetComponent<TheInventory>();
        LoadSaveGame();
        Invoke("updateWeapon", 0.5f);
        Invoke("setStats", 1);
        equipSlotWait = -1;
        cooldownreset = 0;
        firecooldown = 2;
        Randall = this.gameObject;
        Randall.SetActive(true);
        damaged = false;
        damageShader.SetVector("_Fade", new Vector3(1.0f, 0.0f, 0.0f));
    }

    void setStats()
    {
        currentVIT = VIT;
        currentMNA = MNA;
        changeHP(0);
        changeMNA(0);
        Debug.Log("HPSET");
    }
    void LoadSaveGame()
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

        using (StreamReader saveFile = new StreamReader(filePath))
        {
            string[] stats = saveFile.ReadLine().Split(':');
            level = int.Parse(stats[0]);
            VIT = int.Parse(stats[1]);
            MNA = int.Parse(stats[2]);
            EGY = int.Parse(stats[3]);
            STR = int.Parse(stats[4]);
            AGI = int.Parse(stats[5]);
            INT = int.Parse(stats[6]);
            DEF = int.Parse(stats[7]);
            LUK = int.Parse(stats[8]);
            xp = int.Parse(stats[9]);

            int currentCheckpoint = int.Parse(saveFile.ReadLine());
            if (currentCheckpoint == 2 || currentCheckpoint == 3)
            {
                currentCheckpoint -= 2;
            }
            else if (currentCheckpoint == 4)
            {
                currentCheckpoint -= 4;
            }
            rb.transform.position = CheckPoints[currentCheckpoint].position;
            saveFile.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(damaged)
        {
            damageTimer += Time.deltaTime;
            if(damageTimer> damageTime)
            {
                damageShader.SetVector("_Fade", new Vector3(1.0f, 0.0f, 0.0f));
                damaged = false;
            }
        }
        cooldownreset += Time.deltaTime;
        manarecover += Time.deltaTime;
        if (manarecover > 5)
        {
            changeMNA(1);
            manarecover = 0;
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuShown)
            {
                changeHP(0);
                if (VIThidden == false)
                {
                    CHP.SetActive(true);
                }
                inventory.SetActive(false);
                menuShown = false;
            }
            else
            {
                setMenus();

                CHP.SetActive(false);
                inventory.SetActive(true);
                menuShown = true;
                if (eyeState == 0)
                {
                    noEye.SetActive(true);
                    pinkEye.SetActive(false);
                    purpleEye.SetActive(false);
                    bothEye.SetActive(false);
                }
                else if (eyeState == 1)
                {
                    pinkEye.SetActive(true);
                    noEye.SetActive(false);
                    purpleEye.SetActive(false);
                    bothEye.SetActive(false);
                }
                else if (eyeState == 2)
                {
                    purpleEye.SetActive(true);
                    noEye.SetActive(false);
                    pinkEye.SetActive(false);
                    bothEye.SetActive(false);
                }
                else if (eyeState == 3)
                {
                    bothEye.SetActive(true);
                    noEye.SetActive(false);
                    pinkEye.SetActive(false);
                    purpleEye.SetActive(false);
                }
            }
        }
    }

    public void changeHP(int change)
    {
        currentVIT += change;
        if (currentVIT > VIT)
        {
            currentVIT = VIT;
        }
        CurrentHP.text = "VIT: " + currentVIT + "/" + VIT;
    }
    public void changeMNA(int change)
    {
        currentMNA += change;
        if (currentMNA > MNA)
        {
            currentMNA = MNA;
        }
        CurrentMNA.text = "MNA: " + currentMNA + "/" + MNA;
    }
    public void setMenus()
    {
        leveltext.text = "Level " + level + " EXP: " + xp;
        VITtext.text = "VIT: " + VIT;
        MNAtext.text = "MNA: " + MNA;
        EGYtext.text = "EGY: " + EGY;
        STRtext.text = "STR: " + STR;
        AGItext.text = "AGI: " + AGI;
        INTtext.text = "INT: " + INT;
        DEFtext.text = "DEF: " + DEF;
        LUKtext.text = "LUK: " + LUK;

        item[] temp = TheInventory.getQuickItems();
        item equip = TheInventory.getCurrentWeapon();
        string[] slots = new string[3];
        for (int i = 0; i < 3; i++)
        {
            if (temp[i] != null)
            {
                slots[i] = temp[i].name;
            }
            else
            {
                slots[i] = "Empty";
            }
        }

        equipToolTip.text = "Weapon: " + equip.name + "\r\nSlot 1: \r\n" + slots[0] + "\r\nSlot 2: \r\n" + slots[1] + "\r\nSlot 3: \r\n" + slots[2];

        item[] deposit = TheInventory.deposit(TheInventory.inventory);
        for (int i = 0; i < 10; i++)
        {
            items[i].GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
            spells[i].GetComponentInChildren<TextMeshProUGUI>().text = "Empty";
        }
        int itemCount = 0;
        int spellCount = 0;

        for (int i = 0; i < deposit.Length; i++)
        {
            if (deposit[i].itemType == "spell")
            {
                spells[spellCount].GetComponentInChildren<TextMeshProUGUI>().text = deposit[i].name;
                spellCount++;
            }
            else
            {
                Debug.Log(items == null);
                Debug.Log(items[itemCount].GetComponentInChildren<TextMeshProUGUI>() == null);
                items[itemCount].GetComponentInChildren<TextMeshProUGUI>().text = deposit[i].name;
                itemCount++;
            }
        }
    }

    public void updateWeapon()
    {
        item equippedWeapon = TheInventory.getCurrentWeapon();
        VIT += equippedWeapon.hp;
        DEF += equippedWeapon.defense;
        STR += equippedWeapon.damage;
    }

    public void removeWeapon()
    {
        item equippedWeapon = TheInventory.getCurrentWeapon();
        VIT -= equippedWeapon.hp;
        DEF -= equippedWeapon.defense;
        STR -= equippedWeapon.damage;
    }

    public string getItemSlot(int slot)
    {
        item[] quickitems = TheInventory.getQuickItems();
        return quickitems[slot].name;
    }

    public void useItem(int slot)
    {
        if (getItemSlot(slot) == "Lesser Potion")
        {
            changeHP(4);
        }
        else if (getItemSlot(slot) == "Potion")
        {
            changeHP(8);
        }
        else if (getItemSlot(slot) == "Greater Potion")
        {
            changeHP(16);
        }
        else if (getItemSlot(slot) == "Asha")
        {
            if (cooldownreset >= firecooldown && currentMNA >= 3)
            {
                GameObject ashaTemp = Instantiate(asha, bulletSpawn.position, bulletSpawn.rotation);
                Rigidbody2D rb = ashaTemp.GetComponent<Rigidbody2D>();
                rb.AddForce(bulletSpawn.right * AGI, ForceMode2D.Impulse);
                Destroy(ashaTemp, 5);
                cooldownreset = 0;
                changeMNA(-3);
            }
        }
        else if (getItemSlot(slot) == "Litha")
        {
            if (cooldownreset >= firecooldown && currentMNA >= 3)
            {
                GameObject lithaTemp = Instantiate(litha, bulletSpawn.position, bulletSpawn.rotation);
                Rigidbody2D rb = lithaTemp.GetComponent<Rigidbody2D>();
                rb.AddForce(bulletSpawn.right * AGI, ForceMode2D.Impulse);
                Destroy(lithaTemp, 5);
                cooldownreset = 0;
                changeMNA(-3);
            }
        }

        item[] quickitems = TheInventory.getQuickItems();
        if (quickitems[slot].itemType == "potion")
        {
            TheInventory.deleteItem(ref TheInventory.inventory, quickitems[slot].name);
            for (int i = 0; i < 3; i++)
            {
                if (getItemSlot(slot) == getItemSlot(i))
                {
                    TheInventory.removeQuickItem(i);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        float movement;
        if (direction == "right")
        {movement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;}
        else
        {movement = Input.GetAxis("Horizontal") * speed * Time.deltaTime * -1;}
        transform.Translate(Vector2.right * movement);

        if (Input.GetAxis("Horizontal") > 0)
        {
            //animator.SetBool("facingRight", true);
            if (direction != "right")
            {
                Randall.transform.rotation =Quaternion.Euler(0f,0f,0f);
                direction = "right";
            }
            if (grassStep.isPlaying == false && onGround == true)
            {
                grassStep.Play();
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            //animator.SetBool("facingRight", false);
            if (direction == "right")
            {
                Randall.transform.rotation =Quaternion.Euler(0f,180f,0f);
                direction = "left";
            }
            if (grassStep.isPlaying == false && onGround == true)
            {
                grassStep.Play();
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            FaerRight.SetActive(true);
            Invoke("hideSword", .3f);
            if (TheInventory.getCurrentWeapon().name == "Fae'r")
            {
                /*if (bodyanimator.GetBool("facingRight") == true)
                {
                    FaerRight.SetActive(true);
                }
                else
                {
                    FaerLeft.SetActive(true);
                }
                Invoke("hideSword", .3f);
                */
            }
            bodyanimator.SetTrigger("attack");
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (TheInventory.getQuickItems()[0] != null)
            {
                useItem(0);
            }
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            if (TheInventory.getQuickItems()[1] != null)
            {
                useItem(1);
            }
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            if (TheInventory.getQuickItems()[2] != null)
            {
                useItem(2);
            }
        }

        if (onGround == true)
        {
            if (Input.GetButton("Jump"))
            {
                rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
                onGround = false;
            }
        }
        if (rb.position.x != position)
        {
            leftFootanimator.SetBool("motion", true);
            rightFootanimator.SetBool("motion", true);
        }
        else
        {
            leftFootanimator.SetBool("motion", false);
            rightFootanimator.SetBool("motion", false);
        }
        position = rb.position.x;
    }

    public void hideSword()
    {
        FaerRight.SetActive(false);
        FaerLeft.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spirit" || collision.gameObject.tag == "Meanie")
        {
            changeHP(-1);
            damageShader.SetVector("_Fade", new Vector3(20.0f, 0.0f, 0.0f));
            damaged = true;
            damageTimer = 0.0f;

            if (direction == "right")
            {
                transform.Translate(Vector2.right * -2);
            }
            if (direction != "right")
            {
                transform.Translate(Vector2.right * 2);
            }
            hpChecker();
        }
        else if (collision.gameObject.tag == "MeanieShot")
        {
            changeHP(-4);
            damageShader.SetVector("_Fade", new Vector3(20.0f, 0.0f, 0.0f));
            damaged = true;
            damageTimer = 0.0f;
            hpChecker();
        }
        else if (collision.gameObject.tag == "LoadVillageLeft")
        {
            saveGame(2);
            SceneManager.LoadScene("Village");
            SceneManager.UnloadSceneAsync("SeveringSword");
        }
        else if (collision.gameObject.tag == "Zone1Trigger")
        {
            saveGame(1);
            SceneManager.LoadScene("SeveringSword");
            SceneManager.UnloadSceneAsync("Village");
        }
        else if (collision.gameObject.tag == "LoadVillageRight")
        {
            saveGame(3);
            SceneManager.LoadScene("Village");
            SceneManager.UnloadSceneAsync("Zone2");
        }
        else if (collision.gameObject.tag == "Zone2TriggerLeft")
        {
            saveGame(4);
            SceneManager.LoadScene("Zone2");
            SceneManager.UnloadSceneAsync("Village");
        }
         else if (collision.gameObject.tag == "Zone3Trigger")
        {
            
            SceneManager.LoadScene("zone3");
            SceneManager.UnloadSceneAsync("Zone2");
        }
        /*else if (collision.gameObject.tag == "main menu trigger")
        {
            
            SceneManager.LoadScene("MainMenu");
            SceneManager.UnloadSceneAsync("zone3");
        }*/
    }

    public void hpChecker()
    {
        if (currentVIT <= 0)
        {
            gameOver();
        }
    }

    public void gameOver()
    {
        AudioManager.gameOver();
        Randall.SetActive(false);
        gameOverScreen.SetActive(true);
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BekaStoreDoor")
        {
            if (Input.GetKey(KeyCode.Q) && BekaStoreMenu.activeInHierarchy == false)
            {
                AudioManager.BekaStoreEnter();
                BekaStoreMenu.SetActive(true);
            }
        }
    }

    public void saveGame(int savespot)
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
        removeWeapon();
        using (StreamWriter saveFile = new StreamWriter(filePath))
        {
            saveFile.WriteLine(level+":"+VIT+":"+MNA+":"+EGY+":"+STR+":"+AGI+":"+INT+":"+DEF+":"+LUK+":"+xp);
            saveFile.WriteLine(savespot);
            string[] equipsRecord = new string[3];
            for (int i = 0; i < 3; i++)
            {
                if (TheInventory.equip[i] != null)
                {
                    equipsRecord[i] = TheInventory.equip[i].name;
                }
                else
                {
                    equipsRecord[i] = "Empty";
                }
            }
            saveFile.WriteLine(TheInventory.currentWeapon.name + ":" + equipsRecord[0] + ":" + equipsRecord[1] + ":" + equipsRecord[2]);
            item[] deposit = TheInventory.deposit(TheInventory.inventory);
            for (int i = 0; i < TheInventory.getSize(TheInventory.inventory); i++)
            {
                saveFile.WriteLine(deposit[i].itemType + ":" + deposit[i].name + ":" + deposit[i].description + ":" + deposit[i].damage + ":" + deposit[i].defense + ":" + deposit[i].hp + ":" + deposit[i].type + ":" + deposit[i].element + ":" + deposit[i].inscriptions.Length);
                if (deposit[i].inscriptions.Length != 0)
                {
                    for (int j = 0; j < deposit[i].inscriptions.Length; j++)
                    {
                        if (deposit[i].inscriptions[j] == null)
                        {
                            saveFile.WriteLine("Empty");
                        }
                        else
                        {
                            saveFile.WriteLine(deposit[i].inscriptions[j].name + ":" + deposit[i].inscriptions[j].description + ":" + deposit[i].inscriptions[j].damage + ":" + deposit[i].inscriptions[j].defense + ":" + deposit[i].inscriptions[j].hp + ":" + deposit[i].inscriptions[j].specialEffect + ":" + deposit[i].inscriptions[j].element);
                        }
                    }
                }
            }
            saveFile.Close();
        }
        updateWeapon();
    }

    public void equip(int which)
    {
        equipSlotWait = which;
        Invoke("resetEquip", 5);
    }

    public void resetEquip()
    {
        equipSlotWait = -1;
    }

    public void equipThis(Button clickedButton)
    {
        string text = clickedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        TheInventory.setQuickItem(TheInventory.inventory, text, equipSlotWait);
        setMenus();
        item[] tempquickslots = TheInventory.getQuickItems();
    }

    public void exitBekaStore()
    {
        AudioManager.BekaStoreExit();
        BekaStoreMenu.SetActive(false);
    }

    public void addThisItem(Button clickedButton)
    {
        string text = clickedButton.GetComponentInChildren<TextMeshProUGUI>().text;
        if (text == "Lesser Potion")
        {
            item itemToAdd = new item("potion", "Lesser Potion", "A potion that heals 4 VIT", 0, 0, 0, "None", "None", new inscription[0]);
            TheInventory.addNewItem(itemToAdd);
        }
        else if (text == "Potion")
        {
            item itemToAdd = new item("potion", "Potion", "A potion that heals 8 VIT", 0, 0, 0, "None", "None", new inscription[0]);
            TheInventory.addNewItem(itemToAdd);
        }
        else if (text == "Greater Potion")
        {
            item itemToAdd = new item("potion", "Greater Potion", "A potion that heals 16 VIT", 0, 0, 0, "None", "None", new inscription[0]);
            TheInventory.addNewItem(itemToAdd);
        }
    }
}
