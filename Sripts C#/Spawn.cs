using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject _object;

    public int numb;

    public Vector3Int tmapSize;

    int Count = 0;
    int xPos;
    int yPos;


    void Start()
    {
    StartCoroutine(Drops2()); 


    }

    IEnumerator Drops2()
    {
        while (Count < numb)
        {

            //    terrainMap = new int[width, height];

            xPos = Random.Range(-tmapSize.x/2, tmapSize.x/2);
            yPos = Random.Range(-tmapSize.y/2, tmapSize.y/2);

            Instantiate(_object, new Vector3(xPos, yPos, 0), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            Count += 1;
        }
    }

}
