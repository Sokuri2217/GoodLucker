using UnityEngine;

public class WolfController : BossController
{
    [Header("移動")]
    public bool isChase; //追跡中
    [Header("攻撃")]
    public float normalAttackDistance;                  //通常攻撃出来る距離
    public float jumpAttackDistance;                    //ジャンプ攻撃出来る距離
    public float jumpForce;                             //上方向の力
    public float forwardForce;                          //前方向のジャンプ力
    public float attackCoolTimeLimit;                   //ジャンプ攻撃のクールタイム
    public float attackCoolTimeTimer;                   //計測用
    public bool isJumpAttack;                           //ジャンプ攻撃中
    public WeaponBase[] jumpAttack = new WeaponBase[2]; //ジャンプ攻撃の当たり判定

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //徘徊中でないときは追跡
        if(isWalk)
        {
            isChase = false;
        }
        else
        {
            isChase = true;
        }

        //攻撃中は移動しない
        if (isAttack || isJumpAttack) 
        {
            agent.isStopped = true;
            agent.speed = 0;
        }
        else
        {
            agent.isStopped = false;
        }
    }

    //移動処理
    public void UniqueEnemyMove()
    {
        //追跡
        if ((isChase && !isWalk) || invincible)   
        {
            if (!isAttack)
            {
                //プレイヤーの方に向く(水平回転のみ)
                Vector3 chaseForward = (playerPos.position - transform.position).normalized;
                chaseForward.y = 0;
                transform.forward = chaseForward;
            }
            //プレイヤーを追いかける
            agent.SetDestination(playerPos.position);
            //プレイヤーとの距離
            float playerDistance = Vector3.Distance(transform.position, playerPos.position);
            //一定距離近づくと攻撃する
            if (playerDistance <= normalAttackDistance && !isAttack)
            {
                ActiveNormalAttack();
            }
            //通常攻撃が届かない距離かつジャンプ攻撃が可能な距離
            else if (playerDistance > normalAttackDistance && 
                     playerDistance <= jumpAttackDistance)
            {
                if (!isJumpAttack)
                {
                    if(attackCoolTimeTimer <= 0.0f)
                    {
                        //クールタイムをリセット
                        attackCoolTimeTimer = attackCoolTimeLimit;
                        //攻撃
                        ActiveJumpAttack();
                    }
                    else
                    {
                        //クールタイムを計測
                        attackCoolTimeTimer -= Time.deltaTime;
                    }
                }
            }
        }
    }

    //通常攻撃
    public void ActiveNormalAttack()
    {
        critical = ActiveCritical();
        weapon.currentAttack = status[(int)StatusName.STR];
        animator.SetTrigger("NormalAttack");
    }

    //ジャンプ攻撃
    public void ActiveJumpAttack()
    {
        critical = ActiveCritical();
        weapon.currentAttack = status[(int)StatusName.STR];
        animator.SetTrigger("JumpAttack");
        // 相手の方向へベクトルを計算
        Vector3 direction = (playerPos.position - transform.position).normalized;

        // 上方向と前方向に力を加える
        Vector3 jumpDirection = new Vector3(direction.x * forwardForce, jumpForce, direction.z * forwardForce);
        rb.linearVelocity = Vector3.zero; // 前の速度をリセット
        rb.AddForce(jumpDirection, ForceMode.VelocityChange);
    }

    //ジャンプ攻撃の当たり判定を有効化
    public void ActiveJumpAttackCollider()
    {
        agent.enabled = false;
        rb.isKinematic = false;
        jumpAttack[0].HitActive();
        jumpAttack[1].HitActive();
    }
    //ジャンプ攻撃の当たり判定を無効化
    public void InactiveJumpAttackCollider()
    {
        agent.enabled = true;
        rb.isKinematic = true;
        jumpAttack[0].HitInactive();
        jumpAttack[1].HitInactive();
    }
}
