﻿using UnityEngine;

public class FirstUnitCreator : MonoBehaviour {

    public Player player;
	
	void Update ()
    {

        if (player.CreateInitialUnit())
        {
            for (int i = 0; i < 0; i++)
            {
                player.CreateInitialUnit();
            }
            Destroy(this);
        }
	}
}