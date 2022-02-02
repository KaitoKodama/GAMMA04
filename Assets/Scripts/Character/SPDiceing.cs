using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPDiceing : SPBase
{
	[SerializeField] AudioClip panelSound;
	[SerializeField] GameObject dicePanel;

	protected override void OnStayComplete()
	{
		audioSource.PlayOneShot(panelSound);
		dicePanel.SetActive(true);
	}
	protected override void OnExitNotify()
	{
		dicePanel.SetActive(false);
	}
}
