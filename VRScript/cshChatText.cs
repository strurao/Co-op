using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshChatText : MonoBehaviour
{
    public Text text;
    Text mytext;

    // Start is called before the first frame update
    void Start()
    {
        mytext = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        mytext.text = text.text;  
    }
}
