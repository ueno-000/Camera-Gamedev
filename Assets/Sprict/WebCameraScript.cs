using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCameraScript : MonoBehaviour
{
    [SerializeField] public RawImage _rawImage;
    WebCamTexture webCamTexture;

    Color32 mask; //肌の基本色 
    Color32[] base32; //取得した画像データを入れる用 
    byte[,] pixelData;//抽出用

//    WebCamDevice[] camDevices = WebCamTexture.devices; //接続中カメラ取得

    void Start()
    {
        webCamTexture = new WebCamTexture();
        _rawImage.texture = webCamTexture;

        
        webCamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {

        webCamTexture.GetPixels32(base32); //Webカメラの画像データを代入 
        for (int x = 0; x < webCamTexture.width; x++)
        {
            for (int y = 0; y < webCamTexture.height; y++)
            {
                Color32 c32 = base32[x + y * webCamTexture.width];
                if (Near_Color_Chack_RGB(c32)) pixelData[x, y] = 1; //そのピクセルが肌に近ければ1をマーク   
                else pixelData[x, y] = 0;
            }
        }

        bool Near_Color_Chack_RGB(Color32 c)
        { // RGB判定
            if (Byte_Distance(c.r, mask.r) < rgb_aria)
            {
                if (Byte_Distance(c.g, mask.g) < rgb_aria)
                {
                    if (Byte_Distance(c.b, mask.b) < rgb_aria) return true;
                }
            }
            return false;
        }
    }
}
