using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Portal: MonoBehaviour
{
    //private int dificuldade = 0;
    public string SceneName;


    public void Transição()
    {
        SceneManager.LoadScene(SceneName);
    }



}
