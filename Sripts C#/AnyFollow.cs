using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyFollow : MonoBehaviour
{
    public float velocidadeEnemy;
    private Transform posaliado;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        posaliado = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if(posaliado.gameObject != null){
            transform.position = Vector2.MoveTowards(transform.position,posaliado.position,
                velocidadeEnemy*Time.deltaTime); 
        }
     
    }
}
