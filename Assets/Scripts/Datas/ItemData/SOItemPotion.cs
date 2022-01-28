using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOItem", menuName = "SO/Potion")]
public class SOItemPotion : SOItemBase
{
	[SerializeField] float healValue = 50f;
	public override void OnExecute(Actor actor)
	{
		GameManager.instance.SaveState.GetHeal(healValue);
	}
}
