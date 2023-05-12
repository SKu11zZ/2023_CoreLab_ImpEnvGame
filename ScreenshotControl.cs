using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenshotControl : MonoBehaviour
{
    [SerializeField]
    private RenderTexture renderTexture;
    [SerializeField]
    private List<GameObject> gosList;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath);
        for (int i = 0; i < gosList.Count; i++)
        {
            if (i > 0)
            {
                gosList[i - 1].SetActive(false);
            }
            gosList[i].SetActive(true);
            yield return null;
            yield return new WaitForEndOfFrame();
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;
            Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            texture2D.ReadPixels(new Rect(0f, 0f, texture2D.width, texture2D.height), 0, 0);
            File.WriteAllBytes($"{Application.streamingAssetsPath}/{i}.png", texture2D.EncodeToPNG());
            Destroy(texture2D);
        }
    }
}
