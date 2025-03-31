using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other) //Deletes projectiles if they hit an enemy... I hope
    {
        if (other.gameObject.CompareTag("Spirit"))
        {
            Destroy(projectile);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Destroy(projectile);
        }
        else if (other.gameObject.CompareTag("Meanie"))
        {
            Destroy(projectile);
        }
    }
}
