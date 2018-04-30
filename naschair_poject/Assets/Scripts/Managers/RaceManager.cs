using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.SceneManagement;

public class RaceManager : BaseManager
{
    public Race_Data raceData;
    public GameObject aiPrefab;
    public GameObject preGameCamera;
    public GameObject postGameCamera;

    public SplitScreen_SetUp cameras;
    public List<RacerCollection> racers = new List<RacerCollection>();
    public RacerCollection allRacers = new RacerCollection();

    public Transform[] winnerPos, losePos;

    float timeLeft;
    PlayerManager pm;
    LapManager lm;
    public ChairSpawn_Point pointBase;
    Coroutine raceUpdate;
    bool raceOver = false;
    WaitForSeconds oneSecond;

    private void Start()
    {
        BeginStart();
    }

    public void BeginStart()
    {
        pm = GameManager.GetPlayerManager();
        lm = GameManager.GetLapManager();

        if (pm.activePlayers.Count == 0)
            return;

        oneSecond = new WaitForSeconds(1);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        Init();

        if (preGameCamera != null)
            preGameCamera.SetActive(true);

        yield return new WaitForSeconds(raceData.preRaceSegmentLength);

        if (preGameCamera != null)
            preGameCamera.SetActive(false);

        cameras.UpdateCount(pm.activePlayers.Count);
        cameras.SetUp();

        //Count down | badum badum badadumdum
        for (int i = raceData.raceCountDown; i > 0; i--)
        {
            RaceUI_Display.Ref.SetCountDown(i.ToString(), 1);
            yield return oneSecond;
        }

        RaceUI_Display.Ref.SetCountDown("Go!", 1);
        EnableAllracers();

        yield return oneSecond;

        RaceUI_Display.Ref.SetCountDown("", 0f); //Clear countdown
    }

    void EnableAllracers()
    {
        foreach (Racers r in allRacers.racerCollection)
        {
            r.motor.disableMovement = false;
        }
    }

    public void Init()
    {
        SceneManager.LoadScene("RaceUI", LoadSceneMode.Additive);

        racers.Clear();

        for (int i = 0; i <= raceData.lapCount; i++)
        {
            racers.Add(new RacerCollection());
        }

        StartCoroutine(SetUpRacers());
        lm.Init();
    }

    IEnumerator SetUpRacers()
    {
        //int racerCount = pm.activePlayers.Count + Mathf.Clamp((raceData.maxRacers - pm.activePlayers.Count), 0, raceData.maxRacers); //Store how many racers there are

        if (raceData.maxRacers > pointBase.points.Count)
        {
            Debug.Break();
            Debug.LogError("Not enough positions defined for spawning players.");
        }

        for (int i = 0; i < raceData.maxRacers; i++)
        {
            GameObject clone = null;
            PlayerClass playerClass = null;

            if (i < pm.activePlayers.Count)
            {
                clone = pm.spawner.SpawnPlayer(pm.activePlayers[i], pointBase.points[i].position, pointBase.points[i].rotation); //Spawn the player at their spawn point
                pm.activePlayers[i].activePlayerPrefab = clone; //Store the active prefab
                playerClass = pm.activePlayers[i];
            }
            else
            {
                clone = Instantiate(aiPrefab, pointBase.points[i].position, pointBase.points[i].rotation); //Spawn the player at their spawn point
            }

            if (clone != null) //If a racer has been spawned set them up
            {
                Racers tempPos = new Racers();
                tempPos.racerPrefab = clone;

                tempPos.motor = clone.GetComponent<ChairMotor>();
                if (tempPos.motor != null)
                    tempPos.motor.disableMovement = true;

                if (playerClass == null)
                {
                    //Set random ai path
                    AI_Racer ai = clone.GetComponent<AI_Racer>();
                    ai.UpdateAiPaths(lm.GetAiSplines());

                    tempPos.character = clone.GetComponent<CharacterSwitcher>().SetRandomCharacter();
                    tempPos.displayName = tempPos.character.displayName;

                    int count = 0;
                    for (int y = 0; y < allRacers.racerCollection.Count; y++)
                    {
                        if(tempPos.character == allRacers.racerCollection[y].character)
                        {
                            count++;
                        }
                    }

                    if(count > 0)
                        tempPos.displayName += "[" + count + "]";
                }
                else
                {
                    tempPos.character = pm.activePlayers[i].characterProfile;
                    tempPos.playerClass = pm.activePlayers[i];
                    tempPos.displayName = "Player " + (tempPos.playerClass.playerID + 1).ToString();
                }

                tempPos.trackDistance = 0;
                tempPos.splineProjector = clone.GetComponent<SplineProjector>();

                racers[0].racerCollection.Add(tempPos); //Add racer to the start lap collection
                allRacers.racerCollection.Add(racers[0].racerCollection[racers[0].racerCollection.Count - 1]); //Add the most recent racer to the all racers list
            }
        }

        timeLeft = raceData.raceTime;
        raceUpdate = StartCoroutine(RaceUpdate());

        yield return new WaitForEndOfFrame();
    }

    IEnumerator RaceUpdate()
    {
        if(allRacers.racerCollection.Count > 0)
        {
            while(true)
            {
                timeLeft -= Time.deltaTime;
                CheckIfGameIsFinsihed();
                yield return new WaitForEndOfFrame();
            }  
        }
    }

    public Racers GetRacerWithPrefab(GameObject prefab)
    {
        for (int i = 0; i < racers.Count; i++)
        {
            for (int y = 0; y < racers[i].racerCollection.Count; y++)
            {
                if (racers[i].racerCollection[y].racerPrefab == prefab)
                    return racers[i].racerCollection[y];
            }
        }

        return null;
    }

    public void CheckIfGameIsFinsihed()
    {
        //Count how many players are still in the race
        int count = pm.activePlayers.Count;
        for(int i = 0; i < pm.activePlayers.Count; i++)
        {
            for(int y = 0; y < racers[racers.Count - 1].racerCollection.Count; y++)
            {
                if(racers[racers.Count - 1].racerCollection[y].playerClass == pm.activePlayers[i]) //Count how many players are in the final lap list
                    count--;
            }
        }

        if((count <= 0 || timeLeft <= 0 || (pm.activePlayers.Count > 1 && count <= 1)) && !raceOver)
        {
            raceOver = true;

            if(raceUpdate != null)
                StopCoroutine(raceUpdate);

            timeLeft = raceData.raceTime;
            StartCoroutine(FinishGame());
        }
    }

    public IEnumerator FinishGame()
    {
        for(int i = 0; i < allRacers.racerCollection.Count; i++)
        {
            allRacers.racerCollection[i].racerPrefab.GetComponent<ChairMotor>().enabled = false; //Disable all racers
        }

        cameras.ClearAll();
        SetUpWinners();
        ClearAllRacers();

        if (postGameCamera != null)
            postGameCamera.SetActive(true);

        yield return new WaitForSeconds(4);
        SceneManager.LoadSceneAsync("EndScreen", LoadSceneMode.Additive);
    }

    public Transform GetRacerByPlace(int place)
    {
        for (int i = 0; i < allRacers.racerCollection.Count; i++)
        {
            int p = allRacers.racerCollection[i].placment;

            if (p == place)
                return allRacers.racerCollection[i].racerPrefab.transform;
        }

        return null;
    }

    public Transform GetFirstActiveRacer()
    {
        Transform returnValue = allRacers.racerCollection[0].racerPrefab.transform;
        int bestPlacement = 100;

        for (int i = 0; i < allRacers.racerCollection.Count; i++)
        {
            int p = allRacers.racerCollection[i].placment;

            if (p < bestPlacement && !allRacers.racerCollection[i].isFinished)
            {
                bestPlacement = p;
                returnValue = allRacers.racerCollection[i].racerPrefab.transform;
            }
        }

        return returnValue;
    }

    public void SetUpWinners()
    {
        allRacers.racerCollection.Sort((x, z) => x.placment.CompareTo(z.placment));

        for (int i = 0; i < allRacers.racerCollection.Count; i++)
        {
            if (i < winnerPos.Length)
            {
                GameObject clone = Instantiate(allRacers.racerCollection[i].character.characterPrefab, winnerPos[i].position, winnerPos[i].rotation);
                clone.GetComponent<PlayerAnimationController>().TriggerWin();
            }

            if (i < (losePos.Length + winnerPos.Length) && i >= winnerPos.Length)
            {
                GameObject clone = Instantiate(allRacers.racerCollection[i].character.characterPrefab, losePos[i - (winnerPos.Length - 1)].position, losePos[i - (winnerPos.Length - 1)].rotation);
                clone.GetComponent<PlayerAnimationController>().TriggerLose();
            }
        }
    }

    public void ClearAllRacers()
    {
        for (int i = 0; i < allRacers.racerCollection.Count; i++)
        {
            allRacers.racerCollection[i].racerPrefab.SetActive(false);
        }
    }

    public int GetActiveRacerCount()
    {
        int count = 0;

        for (int i = 0; i < allRacers.racerCollection.Count; i++)
        {
            if (!allRacers.racerCollection[i].isFinished)
                count++;
        }

        return count;
    }

    public int GetFinishedRacerCount()
    {
        int count = 0;

        for (int i = 0; i < allRacers.racerCollection.Count; i++)
        {
            if (allRacers.racerCollection[i].isFinished)
                count++;
        }

        return count;
    }
}
