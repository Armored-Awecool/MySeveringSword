using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonMovement : MonoBehaviour
{
    public float speed = 0.2f;
    public Rigidbody2D rb;
    public float position;
    // Start is called before the first frame update
    void Start()
    {
        speed = 0.2f;
        position = rb.position.x;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        if (rb.position.x != position)
        {
            float movement = -Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            transform.Translate(Vector2.right * movement);
        }
        position = rb.position.x;
    }
}
