using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class WebCamController : MonoBehaviour
{
    WebCamTexture webcamTexture;
    RawImage rawImage;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, 1920, 1080, 30);
        this.rawImage = GetComponent<RawImage>();
        this.rawImage.texture = webcamTexture;
        this.rawImage.enabled = false;
        webcamTexture.Play();
    }

    void TakeShot()
    {
        Texture tex = this.rawImage.texture;
        int w = tex.width;
        int h = tex.height;

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture rt = new RenderTexture(w, h, 32);

        Graphics.Blit(tex, rt);
        RenderTexture.active = rt;

        Texture2D result = new Texture2D(w, h, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        result.Apply();
        RenderTexture.active = currentRT;

        GetComponent<MeshRenderer>().material.mainTexture = result;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TakeShot();
        }
    }
}