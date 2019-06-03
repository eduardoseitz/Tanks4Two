using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Declarations
    [SerializeField] int roundNumber = 10;
    [SerializeField] float matchStartDelay = 3f;
    [SerializeField] float matchEndDelay = 3f;

    [SerializeField] Player[] players;
    [SerializeField] GameObject[] spawnPoints;

    [SerializeField] UIController uiController;
    [SerializeField] CameraControl cameraControl;

    int _playerCount = 1;
    int _currentRound = 1;
    #endregion

    #region Main Methods
    private void Start()
    {

    }
    #endregion

    #region Helper Methods
    //
    void ResetMatch()
    {
        _currentRound = 1;
        cameraControl.enabled = true;
    }

    //
    void SpawnAllTanks()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (spawnPoints[i])
            {
                players[i].prefab.transform.position = spawnPoints[i].transform.position;
                players[i].prefab.transform.rotation = spawnPoints[i].transform.rotation;
                players[i].prefab.SetActive(true);
            }
            else
            {
                Debug.LogWarning("[" + this.name + "] " + "There are not enought spawn points assigned.");
            }
        }
    }

    //
    void StartRound()
    {
        SpawnAllTanks();
    }

    //
    void FinishRound()
    {
        _currentRound++;
        // Delays before starting new round
        StartRound();
    }

    //
    public void StartMatch()
    {
        ResetMatch();
        StartRound();
    }

    //
    void FinishMatch()
    {

    }
    #endregion
}
