using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public enum BossState
{
}

public class RockBossController : BossController
{
    [Space(2)]
    [Header("Spine Animation name for Rock Boss Pattern")]
    [SerializeField] private string _ready_attack_near;
    [SerializeField] private string _ready_attack_middle;

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

    public void Ready_attack_near()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _ready_attack_near, true);
    }

    public void Ready_attack_middle()
    {
        _currentTrack = spineAnimationState.SetAnimation(0, _ready_attack_middle, true);
    }
    
    /*
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
    */
}
