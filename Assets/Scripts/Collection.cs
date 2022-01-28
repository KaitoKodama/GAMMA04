using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------------
// クラス
//------------------------------------------
[System.Serializable]
public class AnyDictionary<Tkey, Tvalue>
{
	public Tkey key;
	public Tvalue value;

	public AnyDictionary(Tkey key, Tvalue value)
	{
		this.key = key;
		this.value = value;
	}
	public AnyDictionary(KeyValuePair<Tkey, Tvalue> pair)
	{
		this.key = pair.Key;
		this.value = pair.Value;
	}
}
[System.Serializable]
public class SaveState{

	[SerializeField] int level = 1;
	[SerializeField] int gifted = 0;
	[SerializeField] float power = 5;
	[SerializeField] float defence = 1;
	[SerializeField] float luck = 1;
	[SerializeField] float gold = 100;
	[SerializeField] float exp = 0;
	[SerializeField] float requireExp = 3.4f;
	[SerializeField] float hp = 100;
	[SerializeField] float maxHp = 100;
	
	public int Level { get => level; set => level = value; }
	public float Exp { get => exp; set => exp = value; }
	public float RequireExp { get => requireExp; set => requireExp = value;  }
	public float Luck { get => luck; set => luck = value; }
	public float MaxHp { get => maxHp; set => maxHp = value; }
	public float Power { get => power; set => power = value; }
	public float Defence { get => defence; set => defence = value; }
	public float Hp { get => hp; set => hp = value; }
	public float Gold { get => gold; set => gold = value; }
	public int Gifted { get => gifted; set => gifted = value; }

	public void GetHeal(float value)
	{
		hp = Mathf.Clamp(hp + value, 0, maxHp);
	}
}

[System.Serializable]
public class SaveItem
{
	public SaveItem(ItemID itemID)
	{
		this.itemID = itemID;
		num++;
	}
	public ItemID itemID;
	public int num;
}
[System.Serializable]
public class SaveItems
{
	public List<SaveItem> list = new List<SaveItem>();
}
[System.Serializable]
public class SaveEnedex
{
	public List<int> ids = new List<int>();
}




//------------------------------------------
// インターフェイス
//------------------------------------------
interface IItemReciever
{
	void ApplyItem(SOItemBase itemdata);
}
interface IEnemyReciever
{
	void ApplyDamage(float damage);
	void EnterAttackArea(Actor actor);
	void ExitAttackArea();
}

//------------------------------------------
// 列挙
//------------------------------------------
public enum ItemID
{
	Potion01,
	Potion02,
	Potion03,
	Potion04,
	Potion05,
	Potion06,
	PBuff01,
	PBuff02,
	PBuff03,
	PBuff04,
	PBuff05,
	DBuff01,
	DBuff02,
	DBuff03,
	DBuff04,
	DBuff05,
}
public enum ActorDir
{
	Forward,
	Back,
	Left,
	Right,
	None,
}
public enum EntryType
{
	Unset,
	ToLowStage,
	ToHighStage,
}
public enum SceneName
{
	TitleScene,
	Stage01,
	Stage02,
	Stage03,
	Stage04,
	Stage05,
	Stage06,
}


//------------------------------------------
// ユーティリティ
//------------------------------------------
namespace CommonUtility
{
	public static class Utility
	{
		public static TValue GetDICVal<TValue, TKey>(TKey component, List<AnyDictionary<TKey, TValue>> dics)
		{
			foreach (var dic in dics)
			{
				if (dic.key.Equals(component))
				{
					return dic.value;
				}
			}
			return default;
		}
		public static T GetNextEnum<T>(int currentEnum)
		{
			int nextIndex = currentEnum + 1;
			T nextEnum = (T)Enum.ToObject(typeof(T), nextIndex);
			int length = Enum.GetValues(typeof(T)).Length;
			if (nextIndex >= length)
			{
				nextEnum = (T)Enum.ToObject(typeof(T), 0);
			}
			return nextEnum;
		}
		public static T GetIntToEnum<T>(int targetInt)
		{
			T targetEnum = (T)Enum.ToObject(typeof(T), targetInt);
			return targetEnum;
		}
		public static bool FilpFlop(bool value)
		{
			return !value;
		}
		public static bool Probability(float fPercent)
		{
			float fProbabilityRate = UnityEngine.Random.value * 100.0f;

			if (fPercent == 100.0f && fProbabilityRate == fPercent) return true;
			else if (fProbabilityRate < fPercent) return true;
			else return false;
		}
	}
}