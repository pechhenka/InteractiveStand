using UnityEngine;

namespace MaterialUI
{
	[ExecuteInEditMode()]
	public class ShadowSnap : MonoBehaviour
	{
		public RectTransform targetRect;
		private RectTransform thisRect;

		public float xPadding = 0f;
		public float yPadding = 0f;

		public bool percentage;

		public float xPercent;
		public float yPercent;

		private Rect lastRect;
		private Vector3 lastPos;

		private void Start()
		{
			if (!thisRect)
			{
				thisRect = gameObject.GetComponent<RectTransform>();
			}
		}

		private void LateUpdate()
		{
			if (targetRect)
			{
				if (!thisRect)
				{
					thisRect = gameObject.GetComponent<RectTransform>();
				}

				Vector2 tempVect2;

				if (percentage)
				{
					tempVect2.x = targetRect.sizeDelta.x*xPercent * 0.01f;
					tempVect2.y = targetRect.sizeDelta.y*yPercent * 0.01f;
				}
				else
				{
					tempVect2.x = targetRect.sizeDelta.x + xPadding;
					tempVect2.y = targetRect.sizeDelta.y + yPadding;
				}

				thisRect.sizeDelta = tempVect2;
			}
		}
	}
}