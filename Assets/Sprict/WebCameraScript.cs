using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCameraScript : MonoBehaviour
{
    [SerializeField] public RawImage _rawImage;
    WebCamTexture webCamTexture;

    void Start()
    {
        webCamTexture = new WebCamTexture();
        _rawImage.texture = webCamTexture;
        webCamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
