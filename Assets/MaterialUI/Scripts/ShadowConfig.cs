using UnityEngine;
using UnityEngine.EventSystems;

namespace MaterialUI
{
	public class ShadowConfig : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		public ShadowAnim[] shadows;
		[Range(0,3)]
		public int shadowNormalSize = 1;
		[Range(0, 3)]
		public int shadowActiveSize = 2;

		public enum ShadowsActive
		{
			Hovered,
			Clicked
		}

		public ShadowsActive shadowsActiveWhen = ShadowsActive.Hovered;

		public bool isEnabled = true;
		
		public void OnPointerDown (PointerEventData data)
		{
			if (shadowsActiveWhen == ShadowsActive.Clicked)
				SetShadows(shadowActiveSize);
		}

		public void OnPointerUp(PointerEventData data)
		{
			if (shadowsActiveWhen == ShadowsActive.Clicked)
				SetShadows(shadowNormalSize);
		}
		
		public void OnPointerEnter (PointerEventData data)
		{
			if (shadowsActiveWhen == ShadowsActive.Hovered)
				SetShadows(shadowActiveSize);
		}

		public void OnPointerExit (PointerEventData data)
		{
			SetShadows(shadowNormalSize);
		}

		public void SetShadows (int shadowOn)
		{
			if (isEnabled)
			{
				foreach (ShadowAnim shadow in shadows)
				{
					shadow.SetShadow(false);
				}
				
				if (shadowOn - 1 >= 0)
				{
					shadows [shadowOn - 1].SetShadow (true);
				}
			}
		}
	}
}