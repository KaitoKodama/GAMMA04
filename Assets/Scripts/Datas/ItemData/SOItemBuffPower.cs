using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOItem", menuName = "SO/BuffPower")]
public class SOItemBuffPower : SOItemBase
{
	[SerializeField] float elapse;
	[SerializeField] float additive;

	public override void OnExecute(Actor actor)
	{
		actor.StartCoroutine(actor.SetPowerBuff(this, elapse, additive));
	}
}
