using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshSoundmanager : MonoBehaviour
{
    Button[] myButtons;
    public AudioSource buttonclick;
    // Start is called before the first frame update
    void Start()
    {
        myButtons = GetComponentsInChildren<Button>();

        foreach (Button myButton in myButtons)
        {
            myButton.onClick.AddListener(()=>buttonclick.Play());
        }
    }
}
