using UnityEngine;

namespace MaterialUI
{
	public static class RippleControl
	{
		static GameObject ripplePrefab;
		static GameObject currentRipple;

		public static void Initialize ()
		{
			if (ripplePrefab == null)
				ripplePrefab = Resources.Load ("InkBlot", typeof(GameObject)) as GameObject;
		}

		public static GameObject MakeRipple (Vector3 position, Transform parent, int size, Color color)
		{
			currentRipple = GameObject.Instantiate (ripplePrefab) as GameObject;
			
			Canvas parentCanvas = parent.GetComponentInParent<Canvas> ();
			
			if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
				currentRipple.GetComponent<RectTransform>().position = position;
			else
				currentRipple.transform.localPosition = position;
			
			currentRipple.transform.SetParent (parent);
			
			currentRipple.GetComponent<RectTransform> ().localRotation = new Quaternion (0f, 0f, 0f, 0f);
			
			currentRipple.GetComponent<RippleAnim> ().MakeRipple (size, 6f, 0.5f, 0.3f, color, new Vector3 (0, 0, 0));
			
			return currentRipple;
		}

		public static GameObject MakeRipple (Vector3 position, Transform parent, int size, float animSpeed, float startAlpha, float endAlpha, Color color)
		{
			currentRipple = GameObject.Instantiate (ripplePrefab) as GameObject;
			
			Canvas parentCanvas = parent.GetComponentInParent<Canvas> ();
			
			if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
				currentRipple.GetComponent<RectTransform>().position = position;
			else
				currentRipple.transform.localPosition = position;

			currentRipple.transform.SetParent (parent);
			
			currentRipple.GetComponent<RectTransform> ().localRotation = new Quaternion (0f, 0f, 0f, 0f);

			currentRipple.GetComponent<RippleAnim> ().MakeRipple (size, animSpeed, startAlpha, endAlpha, color, new Vector3 (0, 0, 0));

			return currentRipple;
		}

		public static GameObject MakeRipple (Vector3 position, Transform parent, int size, float animSpeed, float startAlpha, float endAlpha, Color color, Vector3 endPosition)
		{
			currentRipple = GameObject.Instantiate (ripplePrefab) as GameObject;
			
			Canvas parentCanvas = parent.GetComponentInParent<Canvas> ();
			
			if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
				currentRipple.GetComponent<RectTransform>().position = position;
			else
				currentRipple.transform.localPosition = position;
			
			currentRipple.transform.SetParent (parent);
			
			currentRipple.GetComponent<RectTransform> ().localRotation = new Quaternion (0f, 0f, 0f, 0f);
			
			currentRipple.GetComponent<RippleAnim> ().MakeRipple (size, animSpeed, startAlpha, endAlpha, color, endPosition);
			
			return currentRipple;
		}
	}
}