using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public enum AttackType
{
}
public class RockBossController : MonoBehaviour
{
    public GameObject player;
    public GameObject aimPoint;
    public Collider2D close_collider;

    public string attack_close, attack_far, charge_close, charge_far, hit, idle;
    public Spine.AnimationState spineAnimationState { get; private set; }
    public Spine.Skeleton skeleton { get; private set; }

    [SerializeField] string boneName;
    
    private Spine.TrackEntry currentTrack = null;

    private Rigidbody2D rigidbody;    
    private LayerMask playerMask;
    private SkeletonAnimation skeletonAnimation;

    private bool isPlayerClose = false;

    private Spine.Bone aimBone;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponentInChildren<Rigidbody2D>();
        playerMask = LayerMask.GetMask("Player");

        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        
        aimBone = skeleton.FindBone(boneName);

        StartCoroutine(Pattern());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        SearchPlayer();
    }

    void SearchPlayer()
    {
        if (close_collider.IsTouchingLayers(playerMask))
        {
            isPlayerClose = true;
        }
        else
        {
            isPlayerClose = false;
        }
    }

    IEnumerator Pattern()
    {
        while (true)
        {
            bool tmp;
            
            // charge
            if (tmp = isPlayerClose)
            {
                //close pattern
                aimPoint.SetActive(false);
                //play animation
                currentTrack = spineAnimationState?.SetAnimation(0, charge_close, true);
                yield return new WaitForSeconds(5f);
            }
            else
            {
                //far pattern
                //play animation
                currentTrack = spineAnimationState?.SetAnimation(0, charge_far, true);
                //search player
                aimPoint.SetActive(true);
                for (int i = 0; i < 5; i++)
                {
                    Vector3 skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(player.transform.position);
                    skeletonSpacePoint.x *= skeleton.ScaleX;
                    skeletonSpacePoint.y *= skeleton.ScaleY;

                    if (!isPlayerClose)
                    {
                        aimPoint.transform.position = player.transform.position;
                        aimBone.SetLocalPosition(skeletonSpacePoint);
                    }
                    yield return new WaitForSeconds(1f);
                }
            }
            if (tmp)
            {
                //close pattern
                currentTrack = spineAnimationState?.SetAnimation(0, attack_close, false);
            }
            else
            {
                //far pattern
                currentTrack = spineAnimationState?.SetAnimation(0, attack_far, false);
            }
            yield return new WaitForSpineAnimationComplete(currentTrack);
        }
    }
}
