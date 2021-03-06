﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class used for instantiating prefabs and initializing some of its variables.
/// </summary>
public class Factory : MonoBehaviour
{
    protected System.Random rnd = new System.Random();
    protected int sumOfProperties = 35;

    public Player player;
    [SerializeField]
    private PlayerState playerStatePrefab;
    [SerializeField]
    private PlayerPurchases playerPurchasesPrefab;
    [SerializeField]
    private TemporaryBuilding mainBuildingPrefab;
    [SerializeField]
    private TemporaryBuilding libraryPrefab;
    [SerializeField]
    private TemporaryBuilding barracksPrefab;
    [SerializeField]
    private TemporaryBuilding infirmaryPrefab;
    [SerializeField]
    private TemporaryBuilding housePrefab;
    [SerializeField]
    private TemporaryBuilding millPrefab;
    [SerializeField]
    private TemporaryBuilding sawmillPrefab;
    [SerializeField]
    private TemporaryBuilding bankPrefab;
    [SerializeField]
    private Regiment regimentPrefab;
    [SerializeField]
    private Unit unitPrefab;
    [SerializeField]
    private VisibilitySquares humanVisibilitySquaresPrefab;
    [SerializeField]
    private VisibilitySquares aiVisibilitySquaresPrefab;
    [SerializeField]
    private AIPlayer aiPlayerPrefab;
    [SerializeField]
    private SimpleAI simpleAIPrefab;

    public Regiment CreateRegiment(Player owner, List<Unit> units)
    {
        Regiment regiment = Instantiate(regimentPrefab);
        regiment.owner = owner;
        regiment.SetUnits(units);
        regiment.gameObject.SetActive(true);
        return regiment;
    }

    public Unit CreateUnit(Vector3 spawnPoint, NetworkInstanceId playerId)
    {
        Unit unit = Instantiate(unitPrefab, spawnPoint, Quaternion.identity);
        unit.playerNetId = playerId;
        SetRandomParameters(unit);
        unit.gameObject.SetActive(true);
        return unit;
    }

    public PlayerState CreatePlayerState()
    {
        var playerState = Instantiate(playerStatePrefab);
        playerState.player = player;
        playerState.OnPlayerStateChange();
        return playerState;
        
    }

    public PlayerPurchases CreatePlayerPurchases()
    {
        PlayerPurchases playerPurchases = Instantiate(playerPurchasesPrefab);
        playerPurchases.player = player;
        return playerPurchases;

    }

    public VisibilitySquares CreateVisibilitySquares()
    {
        VisibilitySquares visibilitySquares;
        if (player.IsHuman)
            visibilitySquares = Instantiate(humanVisibilitySquaresPrefab);
        else
            visibilitySquares = Instantiate(aiVisibilitySquaresPrefab);
        return visibilitySquares;

    }

    public AIPlayer CreateAIPlayer()
    {
        var aiPlayer = Instantiate(aiPlayerPrefab);
        aiPlayer.player = player;
        aiPlayer.playerId = player.playerControllerId;

        return aiPlayer;
    }

    public SimpleAI CreateAI() => Instantiate(simpleAIPrefab);

    public TemporaryBuilding CreateTemporaryMainBuilding(NetworkInstanceId playerId, BuildingEnum buildingType)
    {
        TemporaryBuilding building;
        switch (buildingType)
        {
            case BuildingEnum.MainBuilding:
                building = Instantiate(mainBuildingPrefab);
                break;
            case BuildingEnum.Library:
                building = Instantiate(libraryPrefab);
                break;
            case BuildingEnum.Barracks:
                building = Instantiate(barracksPrefab);
                break;
            case BuildingEnum.Infirmary:
                building = Instantiate(infirmaryPrefab);
                break;
            case BuildingEnum.House:
                building = Instantiate(housePrefab);
                break;
            case BuildingEnum.Mill:
                building = Instantiate(millPrefab);
                break;
            case BuildingEnum.Sawmill:
                building = Instantiate(sawmillPrefab);
                break;
            case BuildingEnum.Bank:
                building = Instantiate(bankPrefab);
                break;
            default:
                building = null;
                break;
        }
        building.playerNetId = playerId;
        return building;
    }

    private void SetRandomParameters(Unit unit)
    {
        int health = rnd.Next(100);
        int gathering = rnd.Next(100);
        int intelligence = rnd.Next(100);
        int swordsmanship = rnd.Next(100);
        int healing = rnd.Next(100);
        int building = rnd.Next(100);

        int ratio = (health + gathering + intelligence + swordsmanship + healing + building);

        unit.MaxHealth = 500 + 10 * health * sumOfProperties / ratio + 1;
        unit.Health = unit.MaxHealth;
        unit.Gathering = gathering * sumOfProperties / ratio + 1;
        unit.Intelligence = intelligence * sumOfProperties / ratio + 1;
        unit.Swordsmanship = swordsmanship * sumOfProperties / ratio + 1;
        unit.Healing = healing * sumOfProperties / ratio + 1;
        unit.Building = building * sumOfProperties / ratio + 1;
    }
}
