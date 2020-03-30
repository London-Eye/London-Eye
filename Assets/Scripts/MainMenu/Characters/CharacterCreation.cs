using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class CharacterCreation : MonoBehaviour
{
    private Suspect suspect;
    private Suspect victim;
    private Suspect murderer;

    public int suspectsGiven = 5;

    public List<int> mNames = new List<int>(), fNames = new List<int>();
    public List<int> mRelation = new List<int>(), fRelation = new List<int>();

    void Start()
    {
        int generateSuspects = suspectsGiven + 2;
        System.Random rnd = new System.Random();

        bool isMale, hasAlibi;
        int name, relation, emotion;

        for (int i = 0; i < generateSuspects; i++)
        {
            Suspect currentSuspect = Instantiate(suspect) as Suspect;
            if(i == 0)
            {
                isMale = rnd.NextDouble() > 0.5;
                name = rnd.Next(0, 9);
                relation = rnd.Next(0, 6);
                emotion = rnd.Next(0, 7);
                hasAlibi = false;
                currentSuspect.startSuspect(isMale, name, relation, emotion, hasAlibi);
                murderer = currentSuspect;

                if(isMale)
                {
                    mNames.Add(name);
                    if(relation > 1)
                    {
                        mRelation.Add(relation);
                    }
                } else
                {
                    fNames.Add(name);
                    if (relation > 1)
                    {
                        fRelation.Add(relation);
                    }
                }

            } else
            {
                isMale = rnd.NextDouble() > 0.5;

                if(isMale)
                {
                    name = rnd.Next(0, 9);
                    relation = rnd.Next(0, 6);
                    while (mNames.Contains(name) || mRelation.Contains(relation))
                    {
                        name = rnd.Next(0, 9);
                        relation = rnd.Next(0, 6);
                    }

                    mNames.Add(name);
                    if (relation > 1)
                    {
                        mRelation.Add(relation);
                    }
                } else
                {
                    name = rnd.Next(0, 9);
                    relation = rnd.Next(0, 6);
                    while (fNames.Contains(name) || fRelation.Contains(relation))
                    {
                        name = rnd.Next(0, 9);
                        relation = rnd.Next(0, 6);
                    }

                    fNames.Add(name);
                    if (relation > 1)
                    {
                        fRelation.Add(relation);
                    }
                }

                emotion = rnd.Next(0, 7);
                hasAlibi = rnd.NextDouble() > 0.5;

                currentSuspect.startSuspect(isMale, name, relation, emotion, hasAlibi);
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
