using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharacterAnimationController : ObjectAnimationController
{
    public bool bring { get; set; } = false;

    [Header("Bring Weapon Animation")]
    [SerializeField] [SpineAnimation] protected string bring_move;
    [SerializeField] [SpineAnimation] protected string bring_stand;
    [SerializeField] [SpineAnimation] protected string bring_jump;
    [SerializeField] [SpineAnimation] protected string bring_fall;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Stand()
    {
        if (bring)
            currentTrack = spineAnimationState?.SetAnimation(0, bring_stand, true);
        else
            base.Stand();
    }

    public override void Move()
    {
        if (bring)
            currentTrack = spineAnimationState?.SetAnimation(0, bring_move, true);
        else
            base.Move();
    }

    public override void Jump(AirStatus status)
    {
        if (bring)
        {
            switch (status)
            {
                case AirStatus.UP:
                    currentTrack = spineAnimationState?.SetAnimation(0, bring_jump, false);
                    break;
                case AirStatus.DOWN:
                    currentTrack = spineAnimationState?.SetAnimation(0, bring_fall, true);
                    break;
                default:
                    break;
            }
        }
        else
            base.Jump(status);
    }
}
