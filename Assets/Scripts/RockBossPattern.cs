using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBossPattern : MonoBehaviour
{
    [SerializeField] private BossPatternManager bossPatternManager;
    [SerializeField] private GameObject aimPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject laser;

    private Animator _animator;

    private void Start()
    {
        bossPatternManager.nearPattern += new BossPatternManager.BossPattern(nearSearch);
        bossPatternManager.middlePattern += new BossPatternManager.BossPattern(middleSearch);
        bossPatternManager.farPattern += new BossPatternManager.BossPattern(farSearch);

        aimPoint.SetActive(false);
    }

    void nearSearch()
    {
    }

    void middleSearch()
    {
        StartCoroutine(AimPlayer());
    }

    void farSearch()
    {
    }

    IEnumerator AimPlayer()
    {
        aimPoint.SetActive(true);
        for (int i = 0; i < (int)bossPatternManager.ready_time_middle; i++)
        {
            if (bossPatternManager.player_position == PlayerPos.MIDDLE)
            {
                bossPatternManager.boss.Aim(player.transform.position);
                aimPoint.transform.position = player.transform.position;
            }
            yield return new WaitForSecondsRealtime(1f);
        }
        aimPoint.SetActive(false);
    }
}
