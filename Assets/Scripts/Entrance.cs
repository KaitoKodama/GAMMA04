using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
	[SerializeField] AudioClip stairSound;
	[SerializeField] EntryType entryType;
	[SerializeField] SceneName nextScene;

	[SerializeField] SpriteRenderer render;
	[SerializeField] Sprite stairUP;
	[SerializeField] Sprite stairDOWN;

	public EntryType EntryType { get => entryType; }

	private void Start()
	{
		switch (entryType)
		{
			case EntryType.ToLowStage:
				render.sprite = stairDOWN;
				break;
			case EntryType.ToHighStage:
				render.sprite = stairUP;
				break;
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
			GameManager.instance.EntryType = entryType;
			GameManager.instance.OnSceneTransit(nextScene, stairSound);
		}
	}

	public Vector3 GetVector3()
	{
		var pos = transform.position;
		pos.z = 0;
		pos.y -= 2f;
		return pos;
	}
}
