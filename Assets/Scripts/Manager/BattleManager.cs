using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
	class EnemyGene
	{
		public EnemyGene(GameObject obj)
		{
			transform = obj.transform;
			enemy = obj.GetComponent<EnemyBase>();
		}
		public EnemyBase enemy;
		public Transform transform;
	}

	[Header("オーディオ")]
	[SerializeField] AudioClip attackSound;
	[SerializeField] AudioClip deathSound;

	[Header("エフェクトプレハブ")]
	[SerializeField] GameObject appearPrefab;
	[SerializeField] GameObject deathPrefab;

	[Header("出現場所と敵キャラクタ")]
	[SerializeField] GameObject prefab;
	[SerializeField] Transform[] transforms;
	[SerializeField] SOEnemyData[] enemyDatas;

	private List<EnemyGene> enemyGenes = new List<EnemyGene>();
	private AudioSource audioSource;
	private CVSEnemyDamage cvsEnemy;
	private float regeneTime = 5f;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		cvsEnemy = GetComponent<CVSEnemyDamage>();
		foreach (var form in transforms)
		{
			OnGenerateEnemy(form);
		}
	}


	//------------------------------------------
	// デリゲート通知
	//------------------------------------------
	private void OnAttackReciever()
	{
		audioSource.PlayOneShot(attackSound);
	}
	private void OnDeathReciever(EnemyBase enemy)
	{
		StartCoroutine(GenerateEntry(enemy));
		audioSource.PlayOneShot(deathSound);
	}
	private void OnDamageReciever(EnemyBase enemy, float damage)
	{
		cvsEnemy.DisplayHealth(enemy);
		cvsEnemy.DisplayDamage(enemy, damage);
	}


	//------------------------------------------
	// コルーチン
	//------------------------------------------
	private IEnumerator GenerateEntry(EnemyBase enemy)
	{
		Instantiate(deathPrefab, enemy.transform.position, Quaternion.identity);
		yield return new WaitForSeconds(regeneTime);

		Instantiate(appearPrefab, enemy.transform.position, Quaternion.identity);
		OnGenerateEnemy(enemy.transform);
	}


	//------------------------------------------
	// 内部共有関数
	//------------------------------------------
	private void OnGenerateEnemy(Transform targetform)
	{
		int rnd = Random.Range(0, enemyDatas.Length);
		var obj = Instantiate(prefab, targetform.position, Quaternion.identity);
		var ene = new EnemyGene(obj);
		ene.enemy.Init(enemyDatas[rnd]);
		ene.enemy.OnAttackNotifyerHandler = OnAttackReciever;
		ene.enemy.OnDamageNotifyerHandler = OnDamageReciever;
		ene.enemy.OnDeathNotifyerHandler = OnDeathReciever;
		enemyGenes.Add(ene);
	}
}