using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPos
{
    NEAR,
    MIDDLE,
    FAR
}

public class BossPatternManager : MonoBehaviour
{

    [Space(1)]
    [Header("target Boss")]
    public BossController boss;
    public PlayerPos player_position { get; private set; }

    [Space(1)]
    [Header("Attack Area Collider")]
    [SerializeField] private Collider2D _near_collider;
    [SerializeField] private Collider2D _middle_collider;
    [SerializeField] private Collider2D _far_collider;

    [Space(1)]
    [Header("Ready time")]
    public float ready_time;
    public float ready_time_near;
    public float ready_time_middle;
    public float ready_time_far;

    public delegate void BossPattern();
    public BossPattern nearPattern { get; set; }
    public BossPattern middlePattern { get; set; }
    public BossPattern farPattern { get; set; }

    private WaitForSecondsRealtime _ready_time;
    private WaitForSecondsRealtime _near_wait_time;
    private WaitForSecondsRealtime _middle_wait_time;
    private WaitForSecondsRealtime _far_wait_time;


    private WaitForSecondsRealtime _waitForSeconds
    {
        get
        {
            switch (player_position)
            {
                case PlayerPos.NEAR:
                    return _near_wait_time;
                case PlayerPos.MIDDLE:
                    return _middle_wait_time;
                case PlayerPos.FAR:
                    return _far_wait_time;
            }
            return new WaitForSecondsRealtime(1f);
        }
    }


    private LayerMask playerMask;

    // Start is called before the first frame update
    void Start()
    {
        playerMask = LayerMask.GetMask("Player");

        _ready_time = new WaitForSecondsRealtime(ready_time);
        _near_wait_time = new WaitForSecondsRealtime(ready_time_near);
        _middle_wait_time = new WaitForSecondsRealtime(ready_time_middle);
        _far_wait_time = new WaitForSecondsRealtime(ready_time_far);

        StartCoroutine(Basic_boss_pattern());
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
        if (_far_collider.IsTouchingLayers(playerMask))
        {
            player_position = PlayerPos.FAR;
        }
        if (_middle_collider.IsTouchingLayers(playerMask))
        {
            player_position = PlayerPos.MIDDLE;
        }
        if (_near_collider.IsTouchingLayers(playerMask))
        {
            player_position = PlayerPos.NEAR;
        }
    }

    /**
     * Basic BossPattern has Charging time.
     */


    void BossAttackReady(PlayerPos pos)
    {
        switch (pos)
        {
            case PlayerPos.NEAR:
                //play animation
                boss.Ready_attack_near();
                //pattern
                nearPattern();
                break;
            case PlayerPos.MIDDLE:
                //play animation
                boss.Ready_attack_middle();
                //pattern
                middlePattern();
                break;
            case PlayerPos.FAR:
                //play animation
                boss.Ready_attack_far();
                //pattern
                farPattern();
                break;
        }
    }

    void BossAttack(PlayerPos pos)
    {
        switch (pos)
        {
            case PlayerPos.NEAR:
                boss.Attack_near();
                break;
            case PlayerPos.MIDDLE:
                boss.Attack_middle();
                break;
            case PlayerPos.FAR:
                boss.Attack_far();
                break;
        }
    }

    /**
     * This is simple pattern of boss. ToDo: Must fix this.
     */
    IEnumerator Basic_boss_pattern()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            PlayerPos _pos;

            boss.Idle();
            yield return _ready_time;
            _pos = player_position;
            BossAttackReady(_pos);
            yield return _waitForSeconds;

            BossAttack(_pos);
            yield return new WaitUntil(() => boss.isAnimationEnd);
        }
    }
}
