using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BuffGrid : MonoBehaviour
{
	[SerializeField] Image self;
	[SerializeField] Image icon;
	public Image Icon { get => icon; }

	public void BeginIconFill(float duration)
	{
		self.fillAmount = 1;
		self.DOFillAmount(0, duration);
	}
}
