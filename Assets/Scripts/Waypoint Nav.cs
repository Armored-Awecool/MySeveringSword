using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNav : MonoBehaviour
{
    public List<Transform> waypoints;
    public Vector3 goTo;
    public float speed;
    public int waypointSpot = 0;
    public Vector3 playerDistance;
    public Transform player;
    public bool inChase;
    public bool inShoot;
    public bool isShootingEnemy;
    public string enemyType;
    public int chaseDis;
    public int shootDis;
    public GameObject shot;
    public float cooldownreset;
    public float firecooldown;
    public Transform bulletSpawn;
    public float projectileLength;
    public float hp;
    public RandallMovement Randall;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[waypointSpot].position;
        goToNext();
        cooldownreset = firecooldown;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, goTo, speed * Time.deltaTime);
        if (transform.position == goTo)
        {
            goToNext();
        }

        playerDistance = player.position - transform.position;
        if (playerDistance.x < shootDis && playerDistance.x > -shootDis && playerDistance.y < shootDis && playerDistance.y > -shootDis)
        {
            if (isShootingEnemy)
            {
                inShoot = true;
            }
        }
        else if (playerDistance.x < chaseDis && playerDistance.x > -chaseDis && playerDistance.y < chaseDis && playerDistance.y > -chaseDis)
        {
            inShoot = false;
            inChase = true;
            goTo = player.position;
        }
        else
        {
            inChase = false;
            goTo = waypoints[waypointSpot].position;
        }

        cooldownreset += Time.deltaTime;
        if (inShoot)
        {
            if (cooldownreset >= firecooldown)
            {
                GameObject fireTemp = Instantiate(shot, bulletSpawn.position, bulletSpawn.rotation);
                Rigidbody2D rb = fireTemp.GetComponent<Rigidbody2D>();
                rb.AddForce(-bulletSpawn.right * 5, ForceMode2D.Impulse);
                //fireTemp.transform.position = Vector2.MoveTowards(transform.position, player.position, .5f * Time.deltaTime);
                Destroy(fireTemp, projectileLength);
                cooldownreset = 0;
            }
        }
    }

    public void hpChecker()
    {
        if (hp <= 0)
        {
            Randall.xp += 3;
            Destroy(gameObject);
        }
    }

    public void goToNext()
    {
        waypointSpot += 1;
        if (waypointSpot == waypoints.Count)
        {
            waypointSpot = 0;
        }
        goTo = waypoints[waypointSpot].position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            hp -= Randall.STR;
        }
        else if (other.gameObject.CompareTag("Asha"))
        {
            hp -= Randall.INT;
        }
        else if (other.gameObject.CompareTag("Litha"))
        {
            hp -= Randall.INT;
        }

        hpChecker();
    }
}
