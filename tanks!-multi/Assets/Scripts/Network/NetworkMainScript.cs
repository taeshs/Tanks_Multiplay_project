using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Threading;

public class NetworkMainScript : MonoBehaviour
{
    public GameObject IPText;
    public GameObject IPPanel;
    public GameObject ChatPanel;
    public GameObject ChatText;


    private static UINewChat newChatScript;

    private static object lockObject = new object();

    private static Queue<string> outstrs = new Queue<string>();

    private static TcpClient tc;
    private static NetworkStream stream;

    private static bool isConnected = false;
    // Start is called before the first frame update
    public void GetIpText()
    {
        TMP_Text tmp = IPText.GetComponent<TMP_Text>();
        if(tmp.text.Length > 6) // 0.0.0.0
        {
            string str_iptext;
            str_iptext = tmp.text;
            str_iptext = str_iptext.Substring(0, str_iptext.Length - 1);
            ConnectedToServer(str_iptext);
        }
    }

    private void Update()
    {
        string str;
        if (isConnected) {
            lock (lockObject)
            {
                while (outstrs.Count > 0)
                {
                    Debug.Log("dequeued.");
                    str = outstrs.Dequeue();
                    Debug.Log("from : " + str);
                    newChatScript.NewChat(str);
                }
            }
        }
    }

    private void ConnectedToServer(string ipstr)
    {
        
        tc = new TcpClient(ipstr, 8888);
        Debug.Log("connected!");
        
        stream = tc.GetStream();
        newChatScript = ChatPanel.GetComponent<UINewChat>();
        Thread thread = new Thread(RecvThreadFunc);
        thread.Start();
        isConnected = true;

        IPPanel.SetActive(false);
        ChatPanel.SetActive(true);
    }

    public void ChatToServer()
    {
        TMP_Text tmp = ChatText.GetComponent<TMP_Text>();
        string chatstr = tmp.text;
        try
        {
            byte[] buf = Encoding.Default.GetBytes(chatstr.Substring(0, chatstr.Length - 1));
            Debug.Log(chatstr);
            stream.Write(buf, 0, buf.Length);
            tmp.text = "";
        }
        catch
        {
            Debug.Log("chat send error");
        }
    }

    private static void RecvThreadFunc()
    {
        
        byte[] buf = new byte[1024];
        int recvBytes;
        while(true)
        {
            recvBytes = stream.Read(buf);
            if(recvBytes < 1 )
            {
                break;
            }
            string outstr = Encoding.Default.GetString(buf,0,recvBytes);            
            lock (lockObject)
            {
                outstrs.Enqueue(outstr);
                Debug.Log("Enqueued.");
            }
        }
    }
}
