using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System;


using Photon.Pun;
using Photon.Realtime;

public class cshInceptionv3ImageClassifierApply : MonoBehaviourPunCallbacks
{
    private cshInceptionv3ImageClassifier classifier = new cshInceptionv3ImageClassifier();

    public string m_sAction = null;
    public Text txtInfer;
    public Text CurrentState;

    public int m_iAction = -1;
    /* 0:   "BackB",
     * 1:   "CommunicationC",
       2:   "GatherG",
       3:   "ImplementI",
       4:   "MoveM",
       5:   "PictureP",
       6:   "SettingS",
    */
    public string[] msg;
    PhotonView ChatManagerPv;
    GameObject arUser;

    void Start()
    {
        TensorflowAndroidInit();
        Debug.Log(TensorFlow.TFCore.Version);
        classifier.LoadModel("tf_models/EMNIST_graph");
        arUser = GameObject.FindWithTag("ARUser");
        ChatManagerPv = GameObject.FindWithTag("ChatManager").GetComponent<PhotonView>();
        //cpm = GameObject.Find("PanelManager").GetComponent<cshPanelManager>();
        //cai = GameObject.Find("ARUser(Clone)").GetComponent<cshARInteraction>();
        //cmm = GameObject.Find("MissionManager").GetComponent<cshMissionManager>();
        //ManagerName = null;
        //classifier.LoadModel("tf_models/optimized_inceptionDH_graph");

        //StartCoroutine("Predicted");
    }

    public static void TensorflowAndroidInit()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        TensorFlowSharp.Android.NativeBinding.Init();
#endif
    }

    public void InitTensorflow()
    {
        Debug.Log(TensorFlow.TFCore.Version);
        classifier.LoadModel("tf_models/EMNIST_graph");
    }

    // Update is called once per frame
    void Update()
    {

    }


    static byte[] GetImageAsByteArray(string imageFilePath)
    {
        // Open a read-only file stream for the specified file.
        using (FileStream fileStream =
            new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
        {
            // Read the file's contents into a byte array.
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }


    public void PredictedText(bool start)
    {
        /*
        // Image Save Check
        //string pathRsc = Application.dataPath.ToString() + "/Resources/images/screen_28x28.jpg";
        // Android
        string pathRsc = Application.persistentDataPath + "/Resources/images/screen_256x256.jpg";
        byte[] byteTexture = GetImageAsByteArray(pathRsc);
        if (byteTexture.Length > 0)
        {
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
            testImg.texture = texture;
        }
        */

        m_sAction = classifier.PredictLabel(start); // 알파벳 인식받는 변수
        //string msg = null;
        switch (m_sAction)
        {
            case "B":  // 돌아가기
                MyInit();
                break;

            case "C": // 의사소통
                MyInit();
                arUser.GetComponent<cshARChat>().isCcalled = true;
                break;

            case "G": //도구
                MyInit();
                arUser.GetComponent<cshARgather>().isGCalled = true;
                break;

            case "I": //도구
                MyInit();
                arUser.GetComponent<cshARImplement>().isIcalled = true;
                arUser.GetComponent<cshARImplement>().ScareCrowImple();
                break;

            case "M": // 포탈이동
                MyInit();
                arUser.GetComponent<cshARCharactorCreate>().isMcalled = true;
                ChatManagerPv.RPC("isMcalled", RpcTarget.All, true);
                break;

            case "P": // 화면 공유
                MyInit();
                arUser.GetComponent<cshARShare>().isScalled = true;
                break;

            case "5":
            case "S": //세팅
                MyInit();
                arUser.GetComponent<cshARSetting>().isDcalled = true;
                break;

            // 채팅시 메세지 구분
            case "1":
            case "2":
            case "3":
                MySendMessage("ARUser", msg[Int32.Parse(m_sAction) - 1]);
                break;

            default:
                break;
        }

        Debug.Log("Predicted: " + m_sAction);
        if (m_sAction == "unknown") txtInfer.text = "XX";
        else if (m_sAction == "5") txtInfer.text = "S"; // "5" 입력시 "S" 로 처리
        else
        {
            txtInfer.text = m_sAction;
            CurrentState.text = m_sAction + " Mode";
            if (m_sAction == "B")
            {
                CurrentState.text = "Back";
                Invoke("DefaultMode", 1.0f);
            }
        }
    }
    void DefaultMode()
    {
        CurrentState.text = null;
    }
    private void MyInit()
    {
        arUser.GetComponent<cshARgather>().isGCalled = false;
        arUser.GetComponent<cshARCharactorCreate>().isMcalled = false;
        arUser.GetComponent<cshARShare>().isScalled = false;
        arUser.GetComponent<cshARSetting>().isDcalled = false;
        arUser.GetComponent<cshARChat>().isCcalled = false;
        arUser.GetComponent<cshARImplement>().isIcalled = false;
        ChatManagerPv.RPC("isMcalled", RpcTarget.All, false);
    }

    public void MySendMessage(string user, string msg)
    {
        if (!arUser.GetComponent<cshARChat>().isCcalled)
            return;
        Debug.Log(msg);
        ChatManagerPv.RPC("ReceiveMsg", RpcTarget.All, user, msg);
    }
}
