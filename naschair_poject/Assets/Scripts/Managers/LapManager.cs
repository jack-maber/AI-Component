using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class LapManager : BaseManager
{
    public SplineComputer spline;

    public SplineComputer[] aiSplines;
    
    [Range(0.1f, 0.5f)]
    public float checkPointPercentage = 0.5f;
    RaceManager rm;
    PlayerManager pm;

    void Start() 
    {
        Init();
    }

    public void Init()
    {
        rm = GameManager.GetRaceManager();
        pm = GameManager.GetPlayerManager();

        for(int i = 0; i < rm.racers.Count; i++)
        {
            for (int y = 0; y < rm.racers[i].racerCollection.Count; y++)
            {
                rm.racers[i].racerCollection[y].splineProjector.computer = spline; //Set the tracks spline to all racers spline projectors
                rm.racers[i].racerCollection[y].splineProjector.projectTarget = rm.racers[i].racerCollection[y].racerPrefab.transform; //Set the project target to the players transform
            }
        }
    }

    private void Update()
    {
        UpdateTrackDistance();
        SortRacers();
        UpdatePlacments();
        UpdatePlayerClassLaps();
    }

    void UpdateTrackDistance()
    {
        //Update the track distance and the checkPoint 
        for (int i = 0; i < rm.racers.Count; i++) //Go thorugh all the places
        {
            for (int y = 0; y < rm.racers[i].racerCollection.Count; y++) //Go through the racers in that place
            {              
                Racers racer = rm.racers[i].racerCollection[y]; 
                UpdateCheckPoint(racer); //Update the racers checkpoints first to avoid bugs with reversing the track dist

                float trackDistance = (float)racer.splineProjector.clampedPercent;
                bool reverseTrackDist = (trackDistance > (racer.checkPoint + checkPointPercentage * 2));
                float newTrackDistance = (reverseTrackDist) ? trackDistance - 1 : trackDistance; 

                racer.trackDistance = newTrackDistance; //Get the distance along the track

                if(racer.playerClass != null)
                    racer.playerClass.splineDist = newTrackDistance;
            }
        }
    }

    void SortRacers()
    {
        //Sort the racers
        for (int i = 0; i < rm.racers.Count - 1; i++)
        {
            for (int y = 0; y < rm.racers[i].racerCollection.Count - 1; y++)
            {
                rm.racers[i].racerCollection.Sort((x, z) => z.trackDistance.CompareTo(x.trackDistance)); //Sort the racers into furthest first
            }
        }
    }

    void UpdatePlacments()
    {
        //Update placments and update player class values
        int pos = 0;
        for (int i = rm.racers.Count - 1; i >= 0; i--) //Go in reverse so we get people on the latest lap first
        {
            for (int y = 0; y < rm.racers[i].racerCollection.Count; y++)
            {
                pos++;
                rm.racers[i].racerCollection[y].placment = pos;

                if (rm.racers[i].racerCollection[y].playerClass != null)
                {
                    rm.racers[i].racerCollection[y].playerClass.position = pos;
                }   
            }
        }
    }

    void UpdatePlayerClassLaps()
    {
        //Update the laps
        for (int i = 0; i < rm.racers.Count; i++)
        {
            for (int y = 0; y < rm.racers[i].racerCollection.Count; y++)
            {
                PlayerClass p = rm.racers[i].racerCollection[y].playerClass;

                if (p != null && (rm.racers[i].racerCollection[y].lap + 1) != rm.raceData.lapCount + 1)
                    p.lap = rm.racers[i].racerCollection[y].lap + 1;
            }
        }
    }

    void UpdateCheckPoint(Racers racer)
    {
        float td = racer.trackDistance; //Store the distance to keep things clean

        if(td > (racer.checkPoint + checkPointPercentage) && (td < racer.checkPoint + (checkPointPercentage * 2)))
        {
            racer.checkPoint += checkPointPercentage; //Update the check point value
        }
    }

    public void IncreaseLapForRacer(Racers racer)
    {
        rm.racers[racer.lap].racerCollection.Remove(racer); //Remove racer for their active position
        racer.lap++; //Increase their lap counter

        if (racer.playerClass != null)
            racer.playerClass.lap = racer.lap;

        if (racer.lap < rm.racers.Count - 1)
        {
            rm.racers[racer.lap].racerCollection.Add(racer); //With the new lap counter add them to their new lap group
        }
        else
        {
            rm.racers[racer.lap].racerCollection.Add(racer); //Add the racer to the finished racer list.
            
            if(racer.playerClass != null)
                racer.playerClass.activePlayerPrefab.GetComponent<ChairMotor>().enabled = false;

            racer.isFinished = true;     
        }
    }

    public SplineComputer[] GetAiSplines()
    {
        if(aiSplines.Length <= 0)
        {
            aiSplines = new SplineComputer[1];
            aiSplines[0] = spline;
        }

        return aiSplines;
    }
}
