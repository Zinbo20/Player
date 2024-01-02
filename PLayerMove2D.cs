using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    public float speed;
    public Rigidbody2D rig;

    float direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          direction = Input.GetAxis("Horizontal");
    }

    //Update is called by physics
    void FixedUpdate()
    {
      rig.velocity = new Vector2(direction * speed, rig.velocity.y);
    }
