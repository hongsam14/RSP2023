using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ObjectAnimationController : MonoBehaviour, spineAnimeController 
{
    public bool heading
    {
        get => _heading;
        set
        {
            if (value == _heading)
                return;
            TurnHead(value);
        }
    }
    
    public Spine.AnimationState spineAnimationState { get; private set; } = null;
    public Spine.Skeleton skeleton { get; private set; } = null;
    public Spine.TrackEntry currentTrack { get; protected set; } = null;

    [Header("Basic Animation")]
    [SerializeField] [SpineAnimation] protected string move;
    [SerializeField] [SpineAnimation] protected string stand;
    [SerializeField] [SpineAnimation] protected string jump;
    [SerializeField] [SpineAnimation] protected string fall;

    protected bool _isjumping;

    private bool _heading;

    protected virtual void Awake()
    {
        spineAnimationState = GetComponent<SkeletonAnimation>().AnimationState;
        skeleton = GetComponent<SkeletonAnimation>().Skeleton;
    }

    protected virtual void Start()
    {
    }

    public virtual void Move()
    {
        if (_isjumping)
            return;
        currentTrack = spineAnimationState?.SetAnimation(0, move, true);
    }

    public virtual void Stand()
    {
        if (_isjumping)
            return;
        currentTrack = spineAnimationState?.SetAnimation(0, stand, true);
    }

    public virtual void Jump(AirStatus status)
    {
        _isjumping = true;
        switch (status)
        {
            case AirStatus.UP:
                currentTrack = spineAnimationState?.SetAnimation(0, jump, false);
                break;
            case AirStatus.DOWN:
                currentTrack = spineAnimationState?.SetAnimation(0, fall, true);
                break;
            default:
                break;
        }
    }

    public virtual void Land(MoveStatus status)
    {
        _isjumping = false;
        switch (status)
        {
            case MoveStatus.MOVE:
                this.Move();
                break;
            case MoveStatus.STAND:
                this.Stand();
                break;
        }
    }

    public virtual void TurnHead(bool value)
    {
        _heading = value;
        //flip or not
        if (skeleton != null)
            skeleton.ScaleX = value ? 1f : -1f;
    }
}
