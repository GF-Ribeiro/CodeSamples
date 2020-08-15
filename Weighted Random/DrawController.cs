using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController
{
    //Returns a random element given a list of candidates and it's odds
    public static System.Object Draw(List<DrawCandidate> drawCandidates)
    {
        float candidatesTotalSum = 0;

        float startOfInterval = 0;

        float endOfInterval = 0;

        int selectedCandidate = 0;

        //Discovers the total sum
        for (int i = 0; i < drawCandidates.Count; i++)
        {
            candidatesTotalSum += drawCandidates[i].chance;
        }

        //Randomly generates a number from 0 to candidatesTotalSum
        float luckyNumber = Random.Range(0f, candidatesTotalSum); ;

        //Iterates through the candidates list and discovers in which interval the lucky number fits
        //A candidate interval starts from the sum of the previous candidates' chances to the sum of the previous candidates + its own chance
        //Condition: startOfInterval < luckyNumber < endOfInterval
        for (int i = 0; i < drawCandidates.Count; i++)
        {
            endOfInterval += drawCandidates[i].chance;

            if (luckyNumber > startOfInterval && luckyNumber < endOfInterval)
            {
                selectedCandidate = i;

                //breaks to prevent unnecessary interactions
                break;
            }

            startOfInterval = endOfInterval;
        }

        return drawCandidates[selectedCandidate].candidate;
    }
}


public class DrawCandidate
{
    //The object that can be drawn
    public System.Object candidate;
    //The object's chance of being drawn
    public float chance;

    public DrawCandidate(System.Object candidate, float chance)
    {
        this.candidate = candidate;
        this.chance = chance;
    }
}