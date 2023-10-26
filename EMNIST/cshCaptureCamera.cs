﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class cshCaptureCamera : MonoBehaviour
{
    // 4k = 3840 x 2160   1080p = 1920 x 1080
    public int captureWidth = 1920;
    public int captureHeight = 1080;

    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;

    // optimize for many screenshots will not destroy any objects so future screenshots will be fast
    public bool optimizeForManyScreenshots = true;

    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.PPM;

    // folder to write output (defaults to data path)
    public string folder;

    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private int counter = 0; // image #

    // commands
    private bool captureScreenshot = false;
    //private bool captureVideo = false;

    public Camera CaptureCamera;

    //public OVRInput.Controller o_controller;

    // 제스쳐 인식 or 행동 수행 여부를 판단
    public bool m_Predict = false; // false: 제스쳐 인식이 가능, true: 행동 수행이 가능

    // create a unique filename using a one-up variable
    private string uniqueFilename(int width, int height)
    {

        // if folder not specified by now use a good default
        if (folder == null || folder.Length == 0)
        {
            //folder = Application.dataPath;
            //안드로이드 빌드시
            folder = Application.persistentDataPath;
            /*
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                //var stringPath = folder + "/..";
                var stringPath = folder + "/..";
                folder = Path.GetFullPath(stringPath);
            }
            //folder += "/Assets/Resources/images";*/
            //안드로이드 빌드시
            folder += "/Resources/images";

            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(folder);

            // count number of files of specified format in folder
            string mask = string.Format("screen_{0}x{1}*.{2}", width, height, format.ToString().ToLower());
            counter = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly).Length;
        }

        // use width, height, and counter for unique file name
        // 파일명 다르게 저장하는 방법
        /*
        var filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", folder, width, height, counter, format.ToString().ToLower());

        // up counter for next call
        ++counter;
        */


        //파일명 같게 저장하는 방법
        var filename = string.Format("{0}/screen_{1}x{2}.{3}", folder, width, height, format.ToString().ToLower());


        // return unique filename
        return filename;
    }

    public void CaptureScreenshot()
    {
        captureScreenshot = true;
    }

    public void CaptureText()
    {
        // hide optional game object if set
        if (hideGameObject != null) hideGameObject.SetActive(false);

        // create screenshot objects if needed
        if (renderTexture == null)
        {
            // creates off-screen render texture that can rendered into
            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }

        // get main camera and manually render scene into rt
        //Camera camera = this.GetComponent<Camera>(); // NOTE: added because there was no reference to camera in original script; must add this script to Camera
        Camera camera = CaptureCamera.GetComponent<Camera>();
        camera.targetTexture = renderTexture;
        camera.Render();

        // read pixels will read from the currently active render texture so make our offscreen 
        // render texture active and then read the pixels
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);

        // reset active camera texture and render texture
        camera.targetTexture = null;
        RenderTexture.active = null;


        // 이미지 저장을 통해 확인
        // get our unique filename

        string filename = uniqueFilename((int)rect.width, (int)rect.height);

        // pull in our file header/data bytes for the specified image format (has to be done from main thread)
        byte[] fileHeader = null;
        byte[] fileData = null;

        if (format == Format.RAW)
        {
            fileData = screenShot.GetRawTextureData();
        }
        else if (format == Format.PNG)
        {
            fileData = screenShot.EncodeToPNG();
        }
        else if (format == Format.JPG)
        {
            fileData = screenShot.EncodeToJPG();

        }
        else // ppm
        {
            // create a file header for ppm formatted file
            string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
            fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
            fileData = screenShot.GetRawTextureData();
        }

        // create new thread to save the image to file (only operation that can be done in background)
        // 안드로이드 스레드 여부 확인 필요
        //new System.Threading.Thread(() =>
        //{
            // create file and write optional header with image bytes
            var f = System.IO.File.Create(filename);
            if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
            f.Write(fileData, 0, fileData.Length);
            f.Close();
            Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
        //}).Start();


        // unhide optional game object if set
        if (hideGameObject != null) hideGameObject.SetActive(true);

        // cleanup if needed
        if (optimizeForManyScreenshots == false)
        {
            Destroy(renderTexture);
            renderTexture = null;
            screenShot = null;
        }
    }

    public void Inference()
    {
        //CaptureText();
        if (screenShot != null)
            GetComponent<cshInceptionv3ImageClassifierApply>().PredictedText(false);
    }

    IEnumerator CaptureScene()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.T) /*|| OVRInput.GetDown(OVRInput.Button.One, o_controller)*/)
            {
                if (!m_Predict)
                {
                    //CaptureText();
                    if (screenShot != null)
                        GetComponent<cshInceptionv3ImageClassifierApply>().PredictedText(false);

                    //m_Predict = true;
                }
                else
                {
                    m_Predict = false;
                }

                //UnityEditor.AssetDatabase.Refresh();
            }

            yield return null;
        }
    }

    private void Start()
    {
        CaptureText();
        if (screenShot != null) {
            GetComponent<cshInceptionv3ImageClassifierApply>().InitTensorflow();
            GetComponent<cshInceptionv3ImageClassifierApply>().PredictedText(true);
        }

        // 안드로이드 빌드 시 주석
        //StartCoroutine("CaptureScene");
    }

    void Update()
    {

    }
}
