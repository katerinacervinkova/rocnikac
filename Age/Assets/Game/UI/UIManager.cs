﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UIManager>();
            return instance;
        }
    }

    [SerializeField]
    private GameObject healthBarsContainer;
    [SerializeField]
    private Image toolTip;
    [SerializeField]
    private Text toolTipText;
    [SerializeField]
    private List<PurchaseButton> buttons;
    [SerializeField]
    private List<Scheduler> schedulers;
    [SerializeField]
    private Text objectText;
    [SerializeField]
    private Text playerStateText;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private Button unitsButton;
    [SerializeField]
    private Button destroyButton;
    [SerializeField]
    private BuildingWindow buildingWindow;
    [SerializeField]
    private HealthBar healthBarPrefab;

    public Building BuildingWindowShown { get; private set; }

    public void ShowPurchaseButtons(List<Purchase> purchases, Selectable selectable)
    {
        int j = 0;
        for (int i = 0; i < schedulers.Count; i++)
        {
            while (j < purchases.Count && !purchases[j].IsActive(selectable))
                j++;
            buttons[i].gameObject.SetActive(j < purchases.Count);
            if (j < purchases.Count)
                buttons[i].SetPurchase(purchases[j++]);
        }
    }

    public void HidePurchaseButtons()
    {
        buttons.ForEach(b => { if (b != null) b.gameObject.SetActive(false); });
    }

    public void ShowTransactions(List<Transaction> transactions)
    {
        for (int i = 0; i < schedulers.Count; i++)
        {
            if (i < transactions.Count)
            {
                schedulers[i].toolTippedObject.Description = transactions[i].purchase.Name;
                schedulers[i].image.fillAmount = 1 - (transactions[i].Progress / transactions[i].MaxProgress);
                schedulers[i].gameObject.SetActive(true);
            }
            else
                schedulers[i].gameObject.SetActive(false);
        }
    }

    public void HideTransactions()
    {
        schedulers.ForEach(s => { if (s != null) s.gameObject.SetActive(false); });
    }

    public HealthBar CreateHealthBar(Selectable selectable, float offset)
    {
        HealthBar healthBar = Instantiate(healthBarPrefab, healthBarsContainer.transform);
        healthBar.positionOffset = offset;
        healthBar.selectable = selectable;
        return healthBar;
    }

    public void ShowBuildingWindowButton()
    {
        unitsButton.gameObject.SetActive(true);
    }

    public void HideBuildingWindowButton()
    {
        if (unitsButton != null)
            unitsButton.gameObject.SetActive(false);
    }
    public void ShowDestroyButton()
    {
        destroyButton.gameObject.SetActive(true);
    }

    public void HideDestroyButton()
    {
        if (destroyButton != null)
            destroyButton.gameObject.SetActive(false);
    }

    public void OnClickScheduler(int index)
    {
        ((Building)PlayerState.Get().SelectedObject).RemoveTransaction(index);
    }

    public void OnClickBuildingWindow()
    {
        ((Building)PlayerState.Get().SelectedObject).ShowUnitsWindow();
    }

    public void OnClickDestroyButton()
    {
        HideToolTip();
        PlayerState.Get().SelectedObject.owner.DestroySelectedObject(PlayerState.Get().SelectedObject);
    }

    public void OnClickIdleButton()
    {
        PlayerState.Get().SelectIdle();
    }

    public void ChangePlayerStateText(string playerName, PlayerState.AgeEnum age, string description)
    {
        if (playerStateText != null)
            playerStateText.text = $"<b><i>{playerName}</i></b>\n<i>{age} Age</i>\n{description}";
    }

    public void ShowObjectText(string objectName, string description)
    {
        objectText.text = $"<b><i>{objectName}</i></b>\n{description}";
        objectText.gameObject.SetActive(true);
    }

    public void HideObjectText()
    {
        if (objectText != null)
            objectText.gameObject.SetActive(false);
    }
    
    public void ShowToolTip(Vector3 position, string description)
    {
        toolTip.transform.position = position;
        toolTipText.text = description;
        toolTip.gameObject.SetActive(true);
    }
    public void HideToolTip()
    {
        toolTip.gameObject.SetActive(false);
    }

    public void ShowTarget(Vector3 destination)
    {
        target.transform.position = destination;
        target.SetActive(true);
    }

    public void HideTarget()
    {
        if (target != null)
            target.SetActive(false);
    }

    public void ShowBuildingWindow(Building building, List<Unit> units, Action<Unit> action = null)
    {
        buildingWindow.Show(building, units, action);
        BuildingWindowShown = building;
    }

    public void HideBuildingWindow()
    {
        buildingWindow.Hide();
        BuildingWindowShown = null;
    }

    public void UpdateBuildingWindowDescriptions()
    {
        buildingWindow.UpdateDescriptions();
    }
}