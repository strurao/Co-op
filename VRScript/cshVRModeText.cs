using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cshVRModeText : MonoBehaviour
{
    public Text text;
    public GameObject CaptureCamera;
    string m_sAction;
    // Start is called before the first frame update
    void Start()
    {
        m_sAction = CaptureCamera.GetComponent<cshInceptionv3ImageClassifierApplyCNN>().m_sAction;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = m_sAction + " Mode";
    }
}
