using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
	[Header("オーディオ")]
	[SerializeField] AudioClip stageCompSound;
	[SerializeField] AudioClip attackSound;
	[SerializeField] AudioClip deathSound;

	[Header("エフェクトプレハブ")]
	[SerializeField] GameObject appearPrefab;
	[SerializeField] GameObject deathPrefab;

	[Header("ボスキャラ情報")]
	[SerializeField] StagePatch patch;
	[SerializeField] SOEnemyData bossData;
	[SerializeField] GameObject boss;

	[Header("Null許容")]
	[SerializeField] Entrance entranceHigher;

	[Header("出現場所と敵キャラクタ")]
	[SerializeField] GameObject prefab;
	[SerializeField] Transform[] transforms;
	[SerializeField] SOEnemyData[] enemyDatas;

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
		OnGenerateBossEnemy();
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
	private void OnBossDeathReciever(EnemyBase enemy)
	{
		audioSource.PlayOneShot(deathSound);
		DOVirtual.DelayedCall(0.5f, () =>
		{
			audioSource.PlayOneShot(stageCompSound);
		});

		Instantiate(deathPrefab, enemy.transform.position, Quaternion.identity);
		GameManager.instance.SaveState.SetStagePatch(patch);
		entranceHigher?.RefleshEntrance();
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
		var enemy = obj.GetComponent<EnemyBase>();
		enemy.Init(enemyDatas[rnd]);
		enemy.OnAttackNotifyerHandler = OnAttackReciever;
		enemy.OnDamageNotifyerHandler = OnDamageReciever;
		enemy.OnDeathNotifyerHandler = OnDeathReciever;
	}
	private void OnGenerateBossEnemy()
	{
		var enemy = boss.GetComponent<EnemyBase>();
		enemy.Init(bossData);
		enemy.OnAttackNotifyerHandler = OnAttackReciever;
		enemy.OnDamageNotifyerHandler = OnDamageReciever;
		enemy.OnDeathNotifyerHandler = OnBossDeathReciever;
	}
}