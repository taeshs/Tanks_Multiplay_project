using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerDataDontDestroy : MonoBehaviour
{
    private ServerDataDontDestroy instance = null;
    public string data = "";
    public int socket = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance == null)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
