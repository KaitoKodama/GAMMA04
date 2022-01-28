using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SPHealing : MonoBehaviour
{
	[SerializeField] AudioClip healSound;
	[SerializeField] AudioClip circlingSound;
	[SerializeField] GameObject healEffect;
	[SerializeField] Image circleImage;
	private AudioSource audioSource;
	private Tweener circleTween;
	private float duration = 1.5f;


	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		circleImage.fillAmount = 0f;
		transform.DOScaleY(transform.localScale.y + 0.1f, 0.7f).SetLoops(-1, LoopType.Yoyo);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
			audioSource.PlayOneShot(circlingSound);
			circleTween = circleImage.DOFillAmount(1, duration).OnComplete(OnStayComplete);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			circleTween.Pause();
			circleImage.fillAmount = 0f;
		}
	}

	private void OnStayComplete()
	{
		audioSource.PlayOneShot(healSound);
		healEffect.SetActive(false);
		healEffect.SetActive(true);
		GameManager.instance.SaveState.Hp = GameManager.instance.SaveState.MaxHp;
	}
}
