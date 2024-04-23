using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISceneChange : MonoBehaviour
{
    public Button btn_locl, btn_mult;


    // Start is called before the first frame update
    void Start()
    {
        btn_mult.onClick.AddListener(multiplay);
        btn_locl.onClick.AddListener(localplay);
    }

    void multiplay()
    {
        Debug.Log("multi");

        //SceneManager.LoadScene("Main");
    }

    void localplay()
    {
        Debug.Log("local");
        SceneManager.LoadScene("Main");
    }
}
