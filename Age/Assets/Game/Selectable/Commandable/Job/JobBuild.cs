﻿using UnityEngine;

public class JobBuild : Job {

    private readonly int playerId;
    private readonly TemporaryBuilding building;
    private readonly Vector3 buildingPos;
    private readonly Collider buildingCollider;
    private readonly float minTime = 1;
    private float timeElapsed = 0;

    private readonly float buildingIncrease = 0.005f;
    private readonly float slowBuildingIncrease = 0.001f;

    public JobBuild(TemporaryBuilding building, int playerId)
    {
        this.building = building;
        buildingPos = building.transform.position;
        buildingCollider = building.GetComponent<Collider>();
        this.playerId = playerId;
    }

    public override Job Following
    {
        get
        {
            TemporaryBuilding tempBuilding = PlayerState.Get(playerId).GetNearestTempBuilding(building, buildingPos, 20);
            if (tempBuilding == null)
                return null;
            return new JobGo(tempBuilding.FrontPosition, tempBuilding.GetOwnJob(null));
        }
    }


    public override void Do(Unit worker)
    {
        if (!building || Vector3.Distance(buildingCollider.ClosestPointOnBounds(worker.transform.position), worker.transform.position) > 3)
        {
            worker.SetNextJob();
            return;
        }
        timeElapsed += Time.deltaTime;
        while (timeElapsed > minTime)
        {
            building.Build(worker.Building);
            if (worker.Building < worker.Intelligence)
                worker.owner.ChangeAttribute(worker, AttEnum.Building, worker.Building + buildingIncrease * worker.Intelligence);
            else
                worker.owner.ChangeAttribute(worker, AttEnum.Building, worker.Building + slowBuildingIncrease * worker.Intelligence);
            timeElapsed -= minTime;
        }
    }

}
