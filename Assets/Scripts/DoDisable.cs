using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDisable : MonoBehaviour
{
	[SerializeField] float disableTime = 0.5f;

	private void Start()
	{
		StartCoroutine(OnDurationBegin());
	}
	private void OnEnable()
	{
		StartCoroutine(OnDurationBegin());
	}
	private IEnumerator OnDurationBegin()
	{
		yield return new WaitForSeconds(disableTime);
		this.gameObject.SetActive(false);
	}
}
