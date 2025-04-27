using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordHitboxManager : MonoBehaviour
{
    public GameObject sword;
    public GameObject player;

    void Start()
    {
        hideSword();
    }

     public void activateSword()
    {
        sword.SetActive(true);
    }

    public void hideSword()
    {
        sword.SetActive(false);
    }

    void useItem(string item)
    {
        player.SendMessage("useItem");
    }
}
