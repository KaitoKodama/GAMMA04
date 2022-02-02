using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SPHealing : SPBase
{
	[SerializeField] AudioClip healSound;
	[SerializeField] GameObject healEffect;

	protected override void OnStayComplete()
	{
		audioSource.PlayOneShot(healSound);
		healEffect.SetActive(false);
		healEffect.SetActive(true);
		GameManager.instance.SaveState.Hp = GameManager.instance.SaveState.MaxHp;
	}
}
