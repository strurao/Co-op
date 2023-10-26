using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.IO;
using UnityEditor;
using System;



public class cshInceptionv3ImageClassifier
{
    private TFGraph graph;
    private TFSession session;
    private TFSession.Runner runner;

    private static string[] labels = new string[] {
            "0",
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "A",
                "B",
                "C",
                "D",
                "E",
                "F",
                "G",
                "H",
                "I",
                "J",
                "K",
                "L",
                "M",
                "N",
                "O",
                "P",
                "Q",
                "R",
                "S",
                "T",
                "U",
                "V",
                "W",
                "X",
                "Y",
                "Z",
                "a",
                "b",
                "c",
                "d",
                "e",
                "f",
                "g",
                "h",
                "i",
                "j",
                "k",
                "l",
                "m",
                "n",
                "o",
                "p",
                "q",
                "r",
                "s",
                "t",
                "u",
                "v",
                "w",
                "x",
                "y",
                "z"
    };

    public void LoadModel(string your_name_graph)
    {
        if (graph == null)
        {
            Debug.Log("Loading tensor graph " + your_name_graph);
            TextAsset graphModel = Resources.Load<TextAsset>(your_name_graph);

            if (graphModel == null)
            {
                Debug.Log("Failed to load tensor graph " + your_name_graph);
            }

            graph = new TFGraph();
            graph.Import(graphModel.bytes);
            //graph.Import(new TFBuffer(graphModel.bytes));
            session = new TFSession(graph);            
            
        }
    }

    byte[] GetImageAsBytes(string imageFilePath)
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

    Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }


    //public int PredictClass(Texture2D image)
    public int PredictClass()
    {
        try
        {
            runner = session.GetRunner();
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex + "runner is not founded.");
        }
        if (runner == null) return -1;

        // Editor 모드
        //string pathRsc = Application.dataPath.ToString() + "/Resources/images/screen_89x89.jpg";
        //AssetDatabase.ImportAsset(pathRsc, ImportAssetOptions.ImportRecursive);

        // Build 모드
        //string pathRsc = Application.dataPath.ToString() + "/Resources/images/screen_28x28.jpg";
        // 안드로이드 빌드
        /* 주의사항
         * 이미지 경로 문제 (datapath 는 안드로이드에서 읽기 쓰기 불가능): persistentDataPath 사용
         * 텐서플로우 DecodeJpeg 함수 사용 불가: 이미지 버퍼 저장 기능 직접 구현
         * TensorFlowSharp.Android.NativeBinding.Init();: 안드로이드의 경우 함수 호출 필수
         */
        string pathRsc = Application.persistentDataPath + "/Resources/images/screen_256x256.jpg";
        // IO로 경로를 한번 탐색해야 에러가 안생김
        //Debug.Log(System.IO.Directory.GetCurrentDirectory());
        // Build 모드
        // 필요하다면 image width와 height를 가져와 CreateTensorFromImageFile로 넘겨서 일반화
        //TFTensor input = ImageUtil.CreateTensorFromImageFile(pathRsc); // Editor
        //for Android(graph.DecodeJpeg is not used) modify code
        byte[] byteTexture = GetImageAsBytes(pathRsc);
        Texture2D orgtexture = new Texture2D(0, 0);
        if (byteTexture.Length > 0)
        {
            orgtexture.LoadImage(byteTexture);
        }
        //Color32[] pix = orgtexture.GetPixels32();
        //TFTensor input = ImageUtil.transformInput(pix, orgtexture.width, orgtexture.height);
        // 추가: 이미지 로딩 후 28x28 크기로 조정하는 기능
        Texture2D resizeTexture = ScaleTexture(orgtexture, 28, 28);
        Color32[] pix = resizeTexture.GetPixels32();
        TFTensor input = ImageUtil.transformInput(pix, resizeTexture.width, resizeTexture.height);

        runner
            .AddInput(graph["reshape_1_input"][0], input);
        runner.Fetch(graph["dense_3/Softmax"][0]);
        float[,] recurrent_tensor = runner.Run()[0].GetValue() as float[,];

        //float maxVal = float.MinValue;
        float maxVal = -0.1f;
        int bestIdx = -1;
        for (int i = 0; i < 62; ++i)
        {
            float val = recurrent_tensor[0, i];
           // Debug.Log(i + ": " + val);
            if (val > maxVal)
            {
                maxVal = val;
                bestIdx = i;
            }
        }

        return bestIdx;
    }

    //public string PredictLabel(Texture2D image)
    public string PredictLabel(bool start)
    {
        //int classId = PredictClass(image);
        if (start)
        {
            return "unknown";
        }

        int classId = PredictClass();

        if (classId >= 0 && classId < labels.Length)
        {
            return labels[classId];
        }
        return "unknown";

    }

    // 이미지를 TFTensor 클래스로 변환하기 위한 유틸
    public static class ImageUtil
    {
        // Convert the image in filename to a Tensor suitable as input to the Inception model.
        public static TFTensor CreateTensorFromImageFile(string file, TFDataType destinationDataType = TFDataType.Float)
        {
            //byte[] contents = GetImageAsBytes(file);
            
            byte[] contents;
            if (Application.platform == RuntimePlatform.Android)
            {
                WWW reader = new WWW(file);
                while (!reader.isDone) { }

                contents = reader.bytes;
            }
            else
            {
                contents = File.ReadAllBytes(file);
            }

            //var contents = File.ReadAllBytes (file);
            // DecodeJpeg uses a scalar String-valued tensor as input.
            var tensor = TFTensor.CreateString(contents);

            TFGraph graph;
            TFOutput input, output;

            // Construct a graph to normalize the image
            ConstructGraphToNormalizeImage(out graph, out input, out output, destinationDataType);

            // Execute that graph to normalize this one image
            using (var session = new TFSession(graph))
            {
                var normalized = session.Run(
                         inputs: new[] { input },
                         inputValues: new[] { tensor },
                         outputs: new[] { output });

                return normalized[0];
            }
        }
        
        //for Android(graph.DecodeJpeg is not used) modify code
        public static TFTensor transformInput(Color32[] pic, int texturewidth, int textureheight)
        {
            const int W = 28;
            const int H = 28;
            const float mean = 255.0f;
            const float mul = -1.0f;
            const float scale = 255.0f;

            float[] floatValues = new float[texturewidth * textureheight * 1];
            int idx = 0;
            for(int i= textureheight-1; i>=0 ; i--)
            {
                for(int j= 0; j<texturewidth ; j++)
                {
                    var color = pic[i * texturewidth + j];
                    float c = (color.r + color.g + color.b) / 3.0f;
                    floatValues[idx++] = mul * (c - mean) / scale;
                }
            }

            TFShape shape = new TFShape(1, W, H, 1);
            return TFTensor.FromBuffer(shape, floatValues, 0, floatValues.Length);
        }
        
        // The inception model takes as input the image described by a Tensor in a very
        // specific normalized format (a particular image size, shape of the input tensor,
        // normalized pixel values etc.).
        //
        // This function constructs a graph of TensorFlow operations which takes as
        // input a JPEG-encoded string and returns a tensor suitable as input to the
        // inception model.
        private static void ConstructGraphToNormalizeImage(out TFGraph graph, out TFOutput input, out TFOutput output, TFDataType destinationDataType = TFDataType.Float)
        {
            // Some constants specific to the pre-trained model at:
            // https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip
            //
            // - The model was trained after with images scaled to 256x267 pixels.
            // - The colors, represented as R, G, B in 1-byte each were converted to
            //   float using (value - Mean)/Scale.
            // 수정: float using -(value - Mean)/Scale. 이미지 반전 후 0~1로 스케일링

            // image의 사이즈가 바뀌면 수정
            const int W = 28;
            const int H = 28;
            const float Mean = 255.0f;
            const float Mul = -1.0f;
            const float Scale = 255.0f;

            graph = new TFGraph();
            input = graph.Placeholder(TFDataType.String);

            output = graph.Cast(graph.Div(
                x: graph.Mul(
                    x: graph.Sub(
                        x: graph.ResizeBilinear(
                            images: graph.ExpandDims(
                                input: graph.Cast(
                                    graph.DecodeJpeg(contents: input, channels: 1), DstT: TFDataType.Float),
                                dim: graph.Const(0, "make_batch")),
                            size: graph.Const(new int[] { W, H }, "size")),
                        y: graph.Const(Mean, "mean")),
                    y: graph.Const(Mul, "invert")),
                y: graph.Const(Scale, "scale")), destinationDataType);
        }
    }
}
