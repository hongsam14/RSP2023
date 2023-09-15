using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class BossController : MonoBehaviour
{
    /**
     * Basically this component is for spine animation control (boss)
     */
    [Space(2)]
    [Header("Spine Animation name for Basic Boss Pattern")]
    [SerializeField] protected string _attack_near;
    [SerializeField] protected string _attack_middle;
    [SerializeField] protected string _attack_far;
    [SerializeField] protected string _damaged;
    [SerializeField] protected string _idle;

    [Space(2)]
    [Header("Spine Animation name for Rock Boss Pattern")]
    [SerializeField] private string _ready_attack_near;
    [SerializeField] private string _ready_attack_middle;
    [SerializeField] private string _ready_attack_far;

    /**
     * Spine Animation
     */
    public Spine.AnimationState spineAnimationState { get; private set; }
    public Spine.Skeleton skeleton { get; private set; }
    public bool isAnimationEnd
    {
        get => _currentTrack.IsComplete; 
    }

    /**
     * Target Bone : Aim
     */
    [Space(2)]
    [Header("Spine Bone Name for Aim")]
    [SerializeField] private string aimBoneName;
    protected Spine.Bone _aimBone;


    protected SkeletonAnimation _skeletonAnimation;
    protected Spine.TrackEntry _currentTrack;
    
    private Rigidbody2D _rigidbody;
    
    // Start is called before the first frame update.
    public virtual void Start()
    {
        _rigidbody = GetComponentInChildren<Rigidbody2D>();

        _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        spineAnimationState = _skeletonAnimation.AnimationState;
        skeleton = _skeletonAnimation.skeleton;

        _aimBone = skeleton.FindBone(aimBoneName);
    }

    public virtual void Attack_near()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _attack_near, false);
    }

    public virtual void Attack_middle()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _attack_middle, false);
    }

    public virtual void Attack_far()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _attack_far, false);
    }

    public virtual void Ready_attack_near()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _ready_attack_near, true);
    }

    public virtual void Ready_attack_middle()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _ready_attack_middle, true);
    }

    public virtual void Ready_attack_far()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _ready_attack_far, true);
    }

    public virtual void Damaged()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _damaged, false);
    }

    public virtual void Idle()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _idle, true);
    }

    public void Aim(Vector2 targetVec)
    {
        //inverse world position to local position.
        Vector3 skeletonSpacePoint = _skeletonAnimation.transform.InverseTransformPoint(targetVec);
        //multiply scale.
        skeletonSpacePoint.x *= skeleton.ScaleX;
        skeletonSpacePoint.y *= skeleton.ScaleY;
        //set aimbone's local position.
        _aimBone.SetLocalPosition(skeletonSpacePoint);
    }
}
