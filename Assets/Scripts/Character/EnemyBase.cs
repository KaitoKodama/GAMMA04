using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected SOEnemyData data;
	protected Transform _transform;
    protected float hp, maxhp;
	protected float scaleY;
    protected bool isInit = false;
    protected bool deathRequested = false;
    protected bool isAttackEnable = false;


	public float Hp { get => hp;  }
    public float Maxhp { get => maxhp; }
    public SOEnemyData Data { get => data; }

	public void Init(SOEnemyData data)
    {
        hp = data.Hp;
        maxhp = data.Hp;
        _transform = transform;
        scaleY = _transform.localScale.y;
        this.data = data;
        GetComponent<SpriteRenderer>().sprite = data.EnemySprite;

        isInit = true;
    }
    public abstract void ApplyDamage(float damage);
    public abstract void OnEnterAttackArea(Actor actor);
    public abstract void OnExitAttackArea();

    public delegate void OnAttackNotifyer();
    public delegate void OnDeathNotifyer(EnemyBase enemy);
    public delegate void OnDamageNotifyer(EnemyBase enemy, float damage);
    public OnAttackNotifyer OnAttackNotifyerHandler;
    public OnDeathNotifyer OnDeathNotifyerHandler;
    public OnDamageNotifyer OnDamageNotifyerHandler;
}
