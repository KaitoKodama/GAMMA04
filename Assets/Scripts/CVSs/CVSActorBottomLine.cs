using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CVSActorBottomLine : MonoBehaviour
{
	class BuffItemGrid
	{
		public BuffItemGrid(GameObject obj)
		{
			this.obj = obj;
			buffGrid = obj.GetComponent<BuffGrid>();
		}
		public GameObject obj;
		public BuffGrid buffGrid;
		public SOItemBase item;
	}
	[SerializeField] GameObject gridBasePrefab;
	private List<BuffItemGrid> gridPool;
	private int poolNum = 50;


	private void Awake()
	{
		SetGridPool();
	}
	private void Start()
	{
		var actor = GameObject.FindWithTag("Player").GetComponent<Actor>();
		actor.OnBuffStartNotifyerHandler = OnBuffBeginReciever;
		actor.OnBuffEndNotifyerHandler = OnBuffEndReciever;
	}


	private void OnBuffBeginReciever(SOItemBase item, float elapse)
	{
		var pool = GetEnablePool();
		if (pool != null)
		{
			pool.item = item;
			pool.buffGrid.Icon.sprite = item.Sprite;
			pool.buffGrid.BeginIconFill(elapse);
			pool.obj.SetActive(true);
		}
	}
	private void OnBuffEndReciever(SOItemBase item)
	{
		var pool = GettSameItemGrid(item);
		if (pool != null)
		{
			pool.item = null;
			pool.buffGrid.Icon.sprite = null;
			pool.obj.SetActive(false);
		}
	}

	private void SetGridPool()
	{
		gridPool = new List<BuffItemGrid>(poolNum);
		for (int i = 0; i < poolNum; i++)
		{
			var obj = Instantiate(gridBasePrefab, this.gameObject.transform);
			obj.SetActive(false);
			gridPool.Add(new BuffItemGrid(obj));
		}
	}
	private BuffItemGrid GettSameItemGrid(SOItemBase item)
	{
		foreach (var grid in gridPool)
		{
			if (grid.item == item)
			{
				return grid;
			}
		}
		return null;
	}
	private BuffItemGrid GetEnablePool()
	{
		foreach (var grid in gridPool)
		{
			if (!grid.obj.activeSelf)
			{
				return grid;
			}
		}
		return null;
	}
}
