using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class cshVRSetting : MonoBehaviourPun
{
    public bool isScalled = false;
    public Material[] Myskybox;
    public AudioClip[] bgm;
    public GameObject VR3dCanvasP;

    int idx = 0;
    private GameObject AudioManager;
    
    private void Start()
    {
        /*Button btn = SkyBoxbutton.GetComponent<Button>();
        btn.onClick.AddListener(ChangeSkybox);

        
        Button btnbgm = BGMbutton.GetComponent<Button>();
        btn.onClick.AddListener(
            delegate { playSound(bgm, AudioManager); }
        );*/
        AudioManager = GameObject.FindWithTag("BGMmanager");
        //VR3dCanvasP = GameObject.Find("VR3dCanvasP");
    }
    private void Update()
    {
        if (!isScalled)
            VR3dCanvasP.transform.GetChild(1).gameObject.SetActive(false);
        else
            VR3dCanvasP.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ChangeSkybox()
    {
        RenderSettings.skybox = Myskybox[idx % Myskybox.Length];
        idx++;
    }

    public void ChangeBGM()
    {
        AudioManager.GetComponent<cshPlayBGM>().playSound(bgm, AudioManager.GetComponent<AudioSource>());
    }
}
