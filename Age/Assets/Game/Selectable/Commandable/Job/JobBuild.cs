﻿using System;
using UnityEngine;

public class JobBuild : Job {

    private readonly TemporaryBuilding building;
    private readonly Vector3 buildingPos;
    private readonly Collider buildingCollider;
    private readonly float minTime = 1;
    private float timeElapsed = 0;
    public JobBuild(TemporaryBuilding building)
    {
        this.building = building;
        buildingPos = building.transform.position;
        buildingCollider = building.GetComponent<Collider>();
    }

    // hledat další stavbu
    public override Job Following
    {
        get
        {
            TemporaryBuilding tempBuilding = PlayerState.Instance.GetNearestTempBuilding(building, buildingPos, 20);
            if (tempBuilding == null)
                return null;
            return new JobGo(tempBuilding.transform.position, tempBuilding.GetOwnJob(null));
        }
    }


    public override void Do(Unit worker)
    {
        if (!building || Vector3.Distance(buildingCollider.ClosestPointOnBounds(worker.transform.position), worker.transform.position) > 3)
        {
            worker.ResetJob();
            return;
        }
        timeElapsed += Time.deltaTime;
        while (timeElapsed > minTime)
        {
            building.Build(worker);
            timeElapsed -= minTime;
        }
    }

}