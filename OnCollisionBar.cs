using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectMap : MonoBehaviour
{

    public class_name name;

    private void OnTriggerEnter2D(Collider2D collison)
    {
          if(collision.CompareTag("Player"))
          {
          name.class_function();
          }


    
    }


}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{

    public Image Bar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Bar.fillAmount -= Input.GetAxis("Horizontal") /1000f ; // Diminui a Barra ao clicar/mover
    }

    public void addBar()
    {
        Bar.fillAmout += 0.3f;
    }










}
