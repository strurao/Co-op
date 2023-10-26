using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshARSetting : MonoBehaviour
{
    public bool isDcalled = false;

    //Audio
    private GameObject AudioManager;
    public AudioClip[] bgm;
    public GameObject SettingCanvas;

    public GameObject[] CircleUIs;
    public GameObject[] SquareUIs;
    public GameObject[] SimpleUIs;
    public GameObject backgroundUI;
    public GameObject ChatUI;

    Sprite[] sprites;

    public Button BGMbutton;

    //UI Color
    public Image UIColor;
    public Button UIbutton;
    private int idx = 0;
    // Start is called before the first frame update
    void Start()
    {
        //오디오 변경 이벤트 세팅
        AudioManager = GameObject.FindWithTag("BGMmanager"); // AudioManager를 찾는다.
        Button btn = BGMbutton.GetComponent<Button>(); //BGM변경을 위한 버튼  
        btn.onClick.AddListener(        
            delegate { AudioManager.GetComponent<cshPlayBGM>().playSound(bgm, AudioManager.GetComponent<AudioSource>());}   // 버튼에 리스너를 달아준다, AudioManager에 부착된 스크립트와 AudioSource를 인자로 넘겨 실행
        );
        //delegate를 사용한 이유는 AddListener가 매개변수를 필요로하는 함수를 바로 호출 할 수 없기 때문.


        //UI Color 변경 이벤트 세팅
        Button btn2 = UIbutton.GetComponent<Button>(); //UI Color 변경을 위한 버튼
        btn2.onClick.AddListener(UIButtonClicked);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDcalled)
            SettingCanvas.transform.gameObject.SetActive(false);
        else
            SettingCanvas.transform.gameObject.SetActive(true);
    }

    private void UIButtonClicked()
    {
        string[] mycolor = { "black", "red", "blue" };
       
        sprites = Resources.LoadAll<Sprite>("sprites/" + mycolor[idx % mycolor.Length]);
        

        foreach (GameObject SquareUI in SquareUIs)
        {
            SquareUI.GetComponent<Image>().sprite = sprites[0];
        }

        foreach (GameObject CircleUI in CircleUIs)
        {
            CircleUI.GetComponent<Image>().sprite = sprites[1];
        }

        backgroundUI.GetComponent<Image>().sprite = sprites[2];
        ChatUI.GetComponent<Image>().sprite = sprites[3];

        SimpleUIColor(mycolor[idx % mycolor.Length]);
        idx++;
    }
    void SimpleUIColor(string str)
    {
        Color temp;
        if (str == "blue")
            temp = Color.blue;
        else if (str == "black")
            temp = Color.black;
        else
            temp = Color.red;

        foreach (GameObject SimpleUI in SimpleUIs)
        {
            
            SimpleUI.GetComponent<Image>().color = temp;
        }
    }
}
