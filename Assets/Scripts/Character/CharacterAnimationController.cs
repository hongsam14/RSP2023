using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharacterAnimationController : ObjectAnimationController
{
    /**
     * http://en.esotericsoftware.com/spine-applying-animations
     */
    public bool isBring { get; set; } = false;
    public bool isAttacking { get; private set; } = false;

    [Header("Bring Weapon Animation")]
    [SerializeField] [SpineAnimation] protected string bring_move;
    [SerializeField] [SpineAnimation] protected string bring_stand;
    [SerializeField] [SpineAnimation] protected string bring_jump;
    [SerializeField] [SpineAnimation] protected string bring_fall;
    [Header("Attack Animation")]
    [SerializeField] [SpineAnimation] protected string attack;

    private Spine.TrackEntry _tmp_track;
    private bool _tmp_head;

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
        if (_isjumping)
            return;
        if (isAttacking)
        {
            //clear exist animation queue
            if (currentTrack.Next != null)
                spineAnimationState?.ClearNext(currentTrack);
            //add animation to queue;
            spineAnimationState?.AddAnimation(0, stand, true, 0);
            return;
        }
        
        if (isBring)
            currentTrack = spineAnimationState?.SetAnimation(0, bring_stand, true);
        else
            base.Stand();
    }

    public override void Move()
    {
        if (_isjumping)
            return;
        if (isAttacking)
        {
            //clear exist animation queue
            if (currentTrack.Next != null)
                spineAnimationState?.ClearNext(currentTrack);
            //add animation to queue;
            spineAnimationState?.AddAnimation(0, move, true, 0);
            return;
        }
        
        if (isBring)
            currentTrack = spineAnimationState?.SetAnimation(0, bring_move, true);
        else
            base.Move();
    }

    public override void Jump(AirStatus status)
    {
        if (isAttacking)
        {
            //clear exist animation queue
            if (currentTrack.Next != null)
                spineAnimationState?.ClearNext(currentTrack);
            //add animation to queue;
            spineAnimationState?.AddAnimation(0, fall, true, 0);
            return;
        }
        if (isBring)
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

    public override void TurnHead(bool value)
    {
        //save head data
        _tmp_head = value;
        if (isAttacking)
            return;
        base.TurnHead(value);
    }

    public void Attack(Vector2 enemyPos)
    {
        if (isAttacking)
            return;
        base.TurnHead(((enemyPos.x - gameObject.transform.position.x) > 0));
        StartCoroutine(AttackAnime());
    }

    private IEnumerator AttackAnime()
    {
        _tmp_track = currentTrack;
        
        isAttacking = true;
        
        currentTrack = spineAnimationState?.SetAnimation(0, attack, false);
        //add current animation to queue;
        spineAnimationState?.AddAnimation(0, _tmp_track.Animation, _tmp_track.Loop, 0);
        
        yield return new WaitForSpineAnimationComplete(currentTrack);

        isAttacking = false;
        //restore head data
        base.TurnHead(_tmp_head);
    }
}
