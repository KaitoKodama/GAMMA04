using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData", menuName ="SO/EnemyData")]
public class SOEnemyData : ScriptableObject
{
	[SerializeField] Sprite enemySprite;
	[SerializeField] string _name;
	[SerializeField] int level;
    [SerializeField] float hp;
    [SerializeField] float power;
    [SerializeField] float defence;
	[SerializeField] float duration;
    [SerializeField] float exp;
    [SerializeField] float gold;

	public Sprite EnemySprite { get => enemySprite; }
	public int ID { get => level; }
	public int Level { get => level; }
	public string Name { get => _name; }
	public float Hp { get => hp; }
	public float Exp { get => exp; }
	public float Gold { get => gold; }
	public float Power { get => power; }
	public float Defence { get => defence; }
	public float Duration { get => duration; }
}
