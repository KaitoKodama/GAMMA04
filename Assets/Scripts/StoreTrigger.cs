using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoreTrigger : MonoBehaviour
{
	[SerializeField] AudioClip circlingSound;
	[SerializeField] GameObject cvsStore;
	[SerializeField] Image circleImage;

	private Tweener circleTween;
	private AudioSource audioSource;
	private float duration = 1.5f;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
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
			cvsStore.SetActive(false);
		}
	}

	private void OnStayComplete()
	{
		cvsStore.SetActive(true);
	}
}
