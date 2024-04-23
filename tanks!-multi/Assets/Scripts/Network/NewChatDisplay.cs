using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINewChat : MonoBehaviour
{
    public GameObject chatPrefab;
    public Transform chatParent;
    // Start is called before the first frame update
    public void NewChat(string str)
    {
        Debug.Log("newchat : " + str);
        GameObject newchat = Instantiate(chatPrefab, chatParent);
        Text comptext = newchat.GetComponentInChildren<Text>();
        comptext.text = str;
    }
}
