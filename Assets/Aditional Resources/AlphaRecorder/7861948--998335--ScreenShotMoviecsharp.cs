using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShotMoviecsharp : MonoBehaviour
{
	string folder = "UnityBakes";
	public int frameRate = 25;
	public int framesToCapture = 25;
	
	private int frame = 0;

	private string realFolder = "";

    private void Start()
    {

		Time.captureFramerate = frameRate;
		// Set the playback framerate!
		// (real time doesn't influence time anymore)

		// Find a folder that doesn't exist yet by appending numbers!
		realFolder = folder;
		int count = 1;
		while (System.IO.Directory.Exists(realFolder))
		{
			realFolder = folder + count;
			count++;
		}
		// Create the folder
		System.IO.Directory.CreateDirectory(realFolder);
	}
    private void Update()
    {
		StartCoroutine(Capture());
    }

	private IEnumerator Capture() 
	{
		if (frame < framesToCapture)
		{
			var name = string.Format("{0}/{1:D04} shot.png", realFolder, Time.frameCount);
			yield return new WaitForEndOfFrame();

			// Create a texture the size of the screen, RGB24 format
			int width = Screen.width;
			int height = Screen.height;
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
			// Read screen contents into the texture
			tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			tex.Apply();

			var tex2 = new Texture2D(width / 2, height, TextureFormat.ARGB32, false);

			width = width / 2;
			for (int y = 0; y < tex2.height; ++y) {
				for (int x = 0; x < tex2.width; ++x) {

					Color color;

					float alpha = tex.GetPixel(x + width, y).r - tex.GetPixel(x, y).r;
					alpha = 1.0f - alpha;

					if (alpha == 0)
					{
						color = Color.clear;
					}
					else
					{
						color = tex.GetPixel(x, y) / alpha;
					}
					color.a = alpha;
					tex2.SetPixel(x, y, color);
				}
			}

			byte[] pngShot = tex2.EncodeToPNG();
			Destroy(tex);
			Destroy(tex2);

			File.WriteAllBytes(name, pngShot);

			Debug.Log("Frame " + frame);
			frame++;
		}


	}

}