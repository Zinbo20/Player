using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interação : MonoBehaviour
{
    public bool EstaInteragindo { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interage"))
        {
            EstaInteragindo = true;
        }
        else
        {
            EstaInteragindo = false;
        }
        
    }
}
