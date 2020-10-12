using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup Objective", menuName = "Tasks/Objectives/Pickup")]
public class ObjectivePickup : Objective
{
//    public ObjectData objectData;
    public Vector2[] positions;

    public override void OnBeginTask()
    {
        if (positions.Length < 1) return;

        foreach (Vector2 pos in positions)
        {
//            GameController.Instance.World.SpawnObject(new Position(pos.x, pos.y), objectData);
        }
    }

    public override void OnFailTask()
    {

    }
}
