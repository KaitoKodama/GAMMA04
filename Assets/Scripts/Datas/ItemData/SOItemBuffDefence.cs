using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOItem", menuName = "SO/BuffDefence")]
public class SOItemBuffDefence : SOItemBase
{
	[SerializeField] float elapse;
	[SerializeField] float additive;

	public override void OnExecute(Actor actor)
	{
		actor.StartCoroutine(actor.SetDefenceBuff(this, elapse, additive));
	}
}
