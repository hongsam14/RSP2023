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
    [SerializeField] private BossController _boss;
    public PlayerPos player_position { get; private set; }
    
    /**
     * ToDo : Need to add timeline controll
     */

    [SerializeField] private Collider2D _near_collider;
    [SerializeField] private Collider2D _middle_collider;
    [SerializeField] private Collider2D _far_collider;

    private LayerMask playerMask;

    // Start is called before the first frame update
    void Start()
    {
        playerMask = LayerMask.GetMask("Player");
        StartCoroutine(simple_boss_pattern());
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
        Debug.Log(player_position.ToString());
    }

    void BossAttack()
    {
        switch (player_position)
        {
            case PlayerPos.NEAR:
                _boss.Attack_near();
                break;
            case PlayerPos.MIDDLE:
                _boss.Attack_middle();
                break;
            case PlayerPos.FAR:
                //_boss.Attack_far();
                break;
        }
    }

    /**
     * This is simple pattern of boss. ToDo: Must fix this.
     */
    IEnumerator simple_boss_pattern()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            BossAttack();
            yield return new WaitUntil(() => _boss.isAnimationEnd);
        }
    }
}
