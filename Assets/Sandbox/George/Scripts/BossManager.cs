using System.Collections;
using System.Collections.Generic;
using PlusMusic;
using UnityEngine;
using UserInterface;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance { get; private set; }
    public GameObject[] bosses;
    [SerializeField]
    private int currentBoss = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void OnGameStart()
    {
        bosses[currentBoss].SetActive(true);
        DropsSpawner.Instance.SpawnDrop();
        PlusMusic_DJ.Instance.PlayArrangement(new TransitionInfo(PlusMusic_DJ.PMTags.low_backing,true));
    }

    public void OnBossKilled()
    {
        // Spawn healings
        // Delayed next spawn
        currentBoss++;
        if ( currentBoss < bosses.Length )
        {
            Invoke(nameof( SpawnNewBoss ), 3.5f);
        }
        else
        {
            PlusMusic_DJ.Instance.PlayArrangement(new TransitionInfo(PlusMusic_DJ.PMTags.victory, true));
            var messages = FindObjectOfType<MainMessagesUI>();
            if ( messages )
                messages.DisplayWin();
        }
    }

    private void SpawnNewBoss()
    {
        bosses[currentBoss].SetActive(true);
        if ( currentBoss == 1 )
            PlusMusic_DJ.Instance.PlayArrangement(new TransitionInfo(PlusMusic_DJ.PMTags.backing_track, true));
        if ( currentBoss == 2 )
            PlusMusic_DJ.Instance.PlayArrangement(new TransitionInfo(PlusMusic_DJ.PMTags.high_backing, true));
    }
}