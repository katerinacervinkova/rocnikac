﻿using Pathfinding;
using UnityEngine;

public class Unit : Commandable
{
    protected AIPath aiPath;

    public MovementController movementController;
    public Regiment Reg { get; set; }
    private Attributes atts;

    public float Gathering { get { return atts.Get(AttEnum.Gathering); } set { atts.Set(AttEnum.Gathering, value); } }
    public float Intelligence { get { return atts.Get(AttEnum.Intelligence); } set { atts.Set(AttEnum.Intelligence, value); } }
    public float Agility { get { return atts.Get(AttEnum.Agility); } set { atts.Set(AttEnum.Agility, value); } }
    public float Healing { get { return atts.Get(AttEnum.Healing); } set { atts.Set(AttEnum.Healing, value); } }
    public float Building { get { return atts.Get(AttEnum.Building); } set { atts.Set(AttEnum.Building, value); } }
    public float Accuracy { get { return atts.Get(AttEnum.Accuracy); } set { atts.Set(AttEnum.Accuracy, value); } }

    private Job job { get; set; }

    protected void Awake()
    {
        atts = GetComponent<Attributes>();
        aiPath = GetComponent<AIPath>();
        movementController = GetComponent<MovementController>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        minimapColor = owner.color;
        minimapIcon.color = minimapColor;
        transform.Find("Capsule").GetComponent<MeshRenderer>().material.color = owner.color;
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        PlayerState.Instance.units.Add(this);
    }
    protected override void Update()
    {
        JobUpdate();
        base.Update();
    }
    public override void SetSelection(bool selected, Player player)
    {
        base.SetSelection(selected, player);
        if (selected && IsMoving)
            ShowTarget();
        else
            HideTarget();

    }
    public void SetAttribute(AttEnum attEnum, float value)
    {
        atts.Set(attEnum, value);
    }

    protected virtual void JobUpdate()
    {
        if (!hasAuthority)
            return;
        if (job != null && job.Completed)
            SetJob(job.Following);
        job?.Do(this);
    }

    
    public override void RightMouseClickGround(Vector3 hitPoint)
    {
        if (!hasAuthority)
            return;
        SetJob(new JobGo(hitPoint));
    }

    public override string GetObjectDescription()
    {
        return $"Health: {(int)Health}/{(int)MaxHealth}\n{atts.GetDescription()}";
    }

   
    public override void DrawHealthBar()
    {
        DrawProgressBar(Health / MaxHealth);
    }

    public override void SetGoal(Selectable goal)
    {
        Job following = goal.CreateJob(this);
        SetJob(new JobGo(goal.transform.position, following));
    }

    public void SetJob(Job job)
    {
        HideTarget();
        destination = Vector3.positiveInfinity;
        this.job = job;
    }

    public void ResetJob()
    {
        SetJob(job.Following);
    }
    public void Go(Vector3 destination)
    {
        this.destination = destination;
        ShowTarget();
        aiPath.destination = destination;
        aiPath.endReachedDistance = 0.6f;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Reg?.Remove(this);
        PlayerState.Instance?.units.Remove(this);
    }

    public void OnTargetReached()
    {
        destination = Vector3.positiveInfinity;
        Reg?.MovementCompleted(this);
        if (job is JobGo)
            job.Completed = true;
    }
}