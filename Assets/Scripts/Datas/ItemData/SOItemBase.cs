using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOItemBase : ScriptableObject
{
	[SerializeField] ItemID id;
	[SerializeField] Sprite _sprite;
	[SerializeField] int requireGold = 5;
	[SerializeField] string _name;
	[SerializeField] string _explain;

	public ItemID Id { get => id; }
	public Sprite Sprite { get => _sprite; }
	public int RequireGold { get => requireGold; }
	public string Name { get => _name; }
	public string Explain { get => _explain; }

	public abstract void OnExecute(Actor actor);
}
