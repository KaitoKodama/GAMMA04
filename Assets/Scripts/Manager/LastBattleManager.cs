using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBattleManager : MonoBehaviour
{
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip attackSound;
    [SerializeField] CVSEnemyDamage cvsEnemy;
    [SerializeField] GameObject cvsClear;
    [SerializeField] GameObject deathPrefab;

    private AudioSource audioSource;
    private int ttlEnemy;
    private int deathCount;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        var objs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var obj in objs)
		{
            var enemy = obj.GetComponent<EnemyBase>();
            enemy.OnAttackNotifyerHandler = OnAttackReciever;
            enemy.OnDamageNotifyerHandler = DamageReciever;
            enemy.OnDeathNotifyerHandler = DeathReciever;
		}
        ttlEnemy = objs.Length;
    }

    private void OnAttackReciever()
	{
        audioSource.PlayOneShot(attackSound);
	}
    private void DamageReciever(EnemyBase enemy, float damage)
	{
        cvsEnemy.DisplayHealth(enemy);
        cvsEnemy.DisplayDamage(enemy, damage);
    }
    private void DeathReciever(EnemyBase enemy)
	{
        audioSource.PlayOneShot(deathSound);
        Instantiate(deathPrefab, enemy.transform.position, Quaternion.identity);
        deathCount++;
        if (deathCount >= ttlEnemy)
		{
            cvsClear.SetActive(true);
        }
    }
}
