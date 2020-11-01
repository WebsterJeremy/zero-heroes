using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


public class BuildingFarm : Building
{
    #region AccessVariables

    [SerializeField] private SpriteRenderer[] growSpots;
    [SerializeField] private Sprite emptySpot;

    [Serializable]
    public struct CropStage
    {
        public string itemID;
        public Sprite[] stages;
    }
    [SerializeField] public CropStage[] cropStages;

    #endregion
    #region PrivateVariables

    private CropStage cropStage;
    private int growthStage = 0;
    private string crop = null;
    private int oldGrowthStage = 0;

    #endregion
    #region Core

    public override string GetProducedItem()
    {
        return crop;
    }

    public override string GetStockedItem()
    {
        return crop +"_seeds";
    }

    public override bool GetProduces()
    {
        return crop != null && GetStockQuantity() > 0;
    }

    protected override void Update()
    {
        base.Update();

        if (GetProduces())
        {

            growthStage = (int)((1 - ((GetNextProduceTime() - Time.time) / GetProduceTime())) * 3);

            if (oldGrowthStage != growthStage)
            {
                oldGrowthStage = growthStage;
                DisplayGrowth();
            }
        }
    }

    public override BuildingSave GetSave()
    {
        BuildingSave buildingSave = base.GetSave();

        return buildingSave;
    }

    public override void GetLoad(BuildingSave save)
    {
        base.GetLoad(save);
    }

    public void SetCrop(string crop)
    {
        if (this.crop != null)
        {
            if (GetProducedItems() > 0) Collect();
            if (GetStockQuantity() > 0) GameController.Instance.GetInventory().GiveItem(this.crop + "_seeds", GetStockQuantity(), -1);
            SetStockQuantity(0);
        }

        this.crop = crop;

        for (int i = 0; i < cropStages.Length; i++) {
            if (cropStages[i].itemID == crop + "_seeds")
            {
                cropStage = cropStages[i];
            }
        }

        growthStage = 0;
        DisplayGrowth();
    }

    public void DisplayGrowth()
    {
        if (growSpots == null || growSpots.Length == 0) return;

        for (int i = 0;i < growSpots.Length;i++)
        {
            growSpots[i].sprite = cropStage.stages[Mathf.Clamp(growthStage,0,3)];
        }
    }

    #endregion

}
