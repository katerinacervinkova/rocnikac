﻿using System;

public class FoodResource : Resource
{
    private static readonly int maxCapacity = 100;

    protected override int MaxCapacity => maxCapacity;

    public override void Mine(Unit worker)
    {
        int amount = Math.Min(worker.Strength, capacity);
        worker.owner.Mine(amount, this);
        PlayerState.Instance.Food += amount;
    }

    public override Job GetEnemyJob(Commandable worker)
    {
        if (miningJob == null)
            miningJob = new JobMine<FoodResource>(this);
        return miningJob;
    }
}