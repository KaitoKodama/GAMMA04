using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource), typeof(CircleCollider2D))]
public abstract class SPBase : MonoBehaviour
{
	[SerializeField] AudioClip circlingSound;
	[SerializeField] Transform scalingTarget;
	[SerializeField] Image circleImage;
	protected AudioSource audioSource;
	private Tweener circleTween;
	private float duration = 1.5f;


	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		circleImage.fillAmount = 0f;
		scalingTarget.DOScaleY(scalingTarget.localScale.y + 0.1f, 0.7f).SetLoops(-1, LoopType.Yoyo);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			OnEnterNotify();
			audioSource.PlayOneShot(circlingSound);
			circleTween = circleImage.DOFillAmount(1, duration).OnComplete(OnStayComplete);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			OnExitNotify();
			circleTween.Pause();
			circleImage.fillAmount = 0f;
		}
	}

	protected abstract void OnStayComplete();

	protected virtual void OnEnterNotify() { }
	protected virtual void OnExitNotify() { }
}
