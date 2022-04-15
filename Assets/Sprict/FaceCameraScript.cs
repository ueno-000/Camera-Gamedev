namespace OpenCvSharp.Demo
{
	using System;
	using UnityEngine;
	using System.Collections.Generic;
	using UnityEngine.UI;
	using OpenCvSharp;

	public class FaceCameraScript : WebCamera
	{
		public TextAsset faces;
		public TextAsset eyes;
		public TextAsset shapes;

		private FaceProcessorLive<WebCamTexture> processor;

		/// <summary>
		///MonoBehaviorサブクラスのデフォルトの初期化子
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			base.forceFrontalCamera = true; // ここでは正面camsを使用します。macOSの場合は強制的に正面カムを表示しません。

			byte[] shapeDat = shapes.bytes;
			if (shapeDat.Length == 0)
			{
				string errorMessage =
					"In order to have Face Landmarks working you must download special pre-trained shape predictor " +
					"available for free via DLib library website and replace a placeholder file located at " +
					"\"OpenCV+Unity/Assets/Resources/shape_predictor_68_face_landmarks.bytes\"\n\n" +
					"Without shape predictor demo will only detect face rects.";

#if UNITY_EDITOR
				// query user to download the proper shape predictor
				if (UnityEditor.EditorUtility.DisplayDialog("Shape predictor data missing", errorMessage, "Download", "OK, process with face rects only"))
					Application.OpenURL("http://dlib.net/files/shape_predictor_68_face_landmarks.dat.bz2");
#else
             UnityEngine.Debug.Log(errorMessage);
#endif
			}

			processor = new FaceProcessorLive<WebCamTexture>();
			processor.Initialize(faces.text, eyes.text, shapes.bytes);

			// data stabilizer - affects face rects, face landmarks etc.
			processor.DataStabilizer.Enabled = true;        // enable stabilizer
			processor.DataStabilizer.Threshold = 2.0;       // threshold value in pixels
			processor.DataStabilizer.SamplesCount = 2;      // how many samples do we need to compute stable data

			// performance data - some tricks to make it work faster
			processor.Performance.Downscale = 256;          // processed image is pre-scaled down to N px by long side
			processor.Performance.SkipRate = 0;             // we actually process only each Nth frame (and every frame for skipRate = 0)
		}

		//＝＝＝＝＝ここまでFaceDetectorScene.csからのコピペ＝＝＝＝＝

		/// <summary>
		///フレームごとのビデオキャプチャプロセッサ
		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
		{
			// detect everything we're interested in(興味のあるものをすべて検出する)
			processor.ProcessTexture(input, TextureParameters);

			// mark detected objects(検出されたオブジェクトにマークを付ける)
			processor.MarkDetected();

			// processor.Image now holds data we'd like to visualize(これで、processor.Imageは、視覚化するデータを保持します)
			output = Unity.MatToTexture(processor.Image, output);   // if output is valid texture it's buffer will be re-used, otherwise it will be re-created
																	// (出力が有効なテクスチャである場合、そのバッファは再利用されます。そうでない場合、再作成されます。)
			return true;
		}
	}

	public class TrimmingTexture
	{
		private Vector2Int pointRightTop;
		private Vector2Int pointLeftBottom;
		private Texture2D inputTex;
		public TrimmingTexture(Vector2Int pointRightTop, Vector2Int pointLeftBottom, Texture2D inputTex)
		{
			this.pointRightTop = pointRightTop;
			this.pointLeftBottom = pointLeftBottom;
			this.inputTex = inputTex;
		}
		public Texture2D Trim()
		{
			var tw = pointRightTop.x - pointLeftBottom.x;
			var th = pointRightTop.y - pointLeftBottom.y;
			var result = new Texture2D(tw, th);
			var pixels = inputTex.GetPixels(pointLeftBottom.x, pointLeftBottom.y, tw, th);
			result.SetPixels(pixels);
			result.Apply();
			return result;
		}
	}
}
