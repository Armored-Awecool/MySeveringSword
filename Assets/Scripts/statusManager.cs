using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statusManager : MonoBehaviour
{
    public GameObject[] bodyParts;
    public Material normalMaterial;
    public Material burnMaterial;
    public Material freezeMaterial;
    
    public bool burn;
    public bool normal;
    public bool freeze;
    
    public float burnTime = 3.0f;
    public float burnTimer;
    public float freezeTime = 4.0f;
    public float freezeTimer;


    // Start is called before the first frame update
    void Start()
    {
        setNormal();
    }

    // Update is called once per frame
    void Update()
    {
        if(burn)
        {
            gameObject.SendMessage("burnDamage");
            burnTimer+=Time.deltaTime;
            if(burnTimer>burnTime)
            {
                setNormal();
            }
        }
        if(freeze)
        {
            freezeTimer+=Time.deltaTime;
            if(freezeTimer>freezeTime)
            {
                setNormal();
            }
        }
    }

    void setNormal()
    {
        for(int i = 0;i<bodyParts.Length;i++)
        {
            bodyParts[i].GetComponent<Renderer>().material = normalMaterial;
        }
        gameObject.SendMessage("unfreeze");
        freeze = false;
        burn = false;
        normal = true;
        burnTimer = 0f;
        freezeTimer = 0f;
    }

    void setBurn()
    {
        for(int i = 0;i<bodyParts.Length;i++)
        {
            bodyParts[i].GetComponent<Renderer>().material = burnMaterial;
        }
        gameObject.SendMessage("unfreeze");
        freeze = false;
        burn = true;
        normal = false;
        freezeTimer = 0f;
    }
    

    void setFreeze()
    {
        for(int i = 0;i<bodyParts.Length;i++)
        {
            bodyParts[i].GetComponent<Renderer>().material = freezeMaterial;
        }
        gameObject.SendMessage("freeze");
         freeze = true;
        burn = false;
        normal = false;
        burnTimer = 0f;
    }
}
