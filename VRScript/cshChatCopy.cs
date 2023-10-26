using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cshChatCopy : MonoBehaviour
{
    Text chat;
    private void Start()
    {
        chat = GameObject.FindWithTag("ChatManager").GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Text>().text = chat.text;
    }
}
