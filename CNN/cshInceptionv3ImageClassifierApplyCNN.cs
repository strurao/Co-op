using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using UnityEditor;
using UnityEngine.UI;

public class cshInceptionv3ImageClassifierApplyCNN : MonoBehaviour
{

    private cshInceptionv3ImageClassifierCNN classifier = new cshInceptionv3ImageClassifierCNN();
    public static bool MyBack = false;
    public string m_sAction = null;
    public int m_iAction = -1; // Default: -1
    /* 0:   "BackB",
     * 1:   "CommunicationC",
       2:   "GatherG",
       3:   "ImplementI",
       4:   "MoveM",
       5:   "PictureP",
       6:   "SettingS",
    */
    GameObject vrUser;
   
    public Text text;
    public Image img;
    // Use this for initialization
    void Start()
    {
      
        Debug.Log(TensorFlow.TFCore.Version);
        classifier.LoadModel("tf_models/optimized_inception_graph");
        vrUser = GameObject.FindWithTag("VRUser");
        //classifier.LoadModel("tf_models/optimized_inceptionDH_graph");

        //StartCoroutine("Predicted");
    }

    public void InitTensorflow()
    {
        Debug.Log(TensorFlow.TFCore.Version);
        classifier.LoadModel("tf_models/optimized_inception_graph");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PredictedHands(bool start)
    {
        m_sAction = classifier.PredictLabel(start);
        string textString = "";
        switch (m_sAction)
        {
            case "BackB":
                m_iAction = 0;
                textString = "Back";
                MyInit();
                Debug.Log("B 입력");
                break;

            case "CommunicationC":
                m_iAction = 1;
                textString = "Communication";
                MyInit();
                Debug.Log("C 입력");
                vrUser.GetComponent<cshVRCommunication>().isCcalled = true;
                break;

            case "GatherG": // 수집G
                m_iAction = 2;
                textString = "Gathering";
                MyInit();
                vrUser.GetComponent<cshVRGather>().isGcalled = true;
                Debug.Log("G 입력");
                break;

            case "ImplementI": 
                m_iAction = 3;
                textString = "Implement";
                MyInit();
                vrUser.GetComponent<cshVRGather>().isIcalled = true;
                Debug.Log("I 입력");
                break;

            case "MoveM":
                m_iAction = 4;
                textString = "Movement";
                vrUser.GetComponent<cshVRMoveV2>().VRMoveON();
                Debug.Log("M 입력");
                break;

            case "PictureP":
                m_iAction = 5;
                textString = "Picture";
                MyInit();
                vrUser.GetComponent<cshVRShareCamera>().isScalled = true;
                Debug.Log("P 입력");
                break;

            case "SettingS":
                m_iAction = 6;
                textString = "Settings";
                MyInit();
                vrUser.GetComponent<cshVRSetting>().isScalled = true;
                Debug.Log("S 입력");
                break;

            default:
                m_iAction = -1;
                break;
        }

        Debug.Log("Predicted: " + m_sAction);
        img.gameObject.SetActive(true);
        text.text = textString + " Mode";
        Invoke("mydelay", 2.0f);
    }

    //VR사용자의 시점에서 보이는 text를 꺼준다. Invoke로 지연호출함.
    void mydelay()
    {
        text.text = null;
        img.gameObject.SetActive(false);
    }

    //초기화함수.
    void MyInit()
    {
        //vrUser.GetComponent<cshVRMove>().isMcalled = false;
        vrUser.GetComponent<cshVRMoveV2>().isMcalled = false;
        vrUser.GetComponent<cshVRCommunication>().isCcalled = false;
        vrUser.GetComponent<cshVRShareCamera>().isScalled = false;
        vrUser.GetComponent<cshVRSetting>().isScalled = false;
        vrUser.GetComponent<cshVRGather>().isGcalled = false;
        vrUser.GetComponent<cshVRGather>().isIcalled = false;

    }
}
