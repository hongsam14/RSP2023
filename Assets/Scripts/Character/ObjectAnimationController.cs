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
            _heading = value;
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
    [SerializeField] [SpineAnimation] protected string hit;

    protected bool _isjumping = false;
    protected AirStatus _status;
    
    private bool _heading = true;
    private (string, bool) _tmp_anime;

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
        _status = status;
        
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
        //flip or not
        if (skeleton != null)
            skeleton.ScaleX = value ? 1f : -1f;
    }

    public virtual void Hit(Vector2 enempyPos)
    {
    }

    protected void RegisterAnimation(string anime_name, bool loop)
    {
        _tmp_anime.Item1 = anime_name;
        _tmp_anime.Item2 = loop;
    }

    protected void RegisterAnimation(Spine.TrackEntry track)
    {
        _tmp_anime.Item1 = track.Animation.Name;
        _tmp_anime.Item2 = track.Loop;
    }

    protected void PlayRegisteredAnimation()
    {
        currentTrack = spineAnimationState?.SetAnimation(0, _tmp_anime.Item1, _tmp_anime.Item2);
    }
}
