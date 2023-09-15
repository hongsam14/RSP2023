using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public enum BossState
{
}

public class RockBossController : BossController
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Attack_near()
    {
        base.Attack_near();
    }

    public override void Attack_middle()
    {
        base.Attack_middle();
    }

    public override void Attack_far()
    {
        base.Attack_far();
    }

    public override void Damaged()
    {
        base.Damaged();
    }

    public override void Ready_attack_near()
    {
        base.Ready_attack_near();
    }

    public override void Ready_attack_middle()
    {
        base.Ready_attack_middle();
    }

    public override void Ready_attack_far()
    {
        base.Ready_attack_far();
    }
}
