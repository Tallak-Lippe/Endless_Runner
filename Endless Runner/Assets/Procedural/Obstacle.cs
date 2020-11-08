using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ProceduralElement
{
    public Obstacle(string _tag)
    {
        objectTag = _tag;
    }

    public override void CalculateAndSpawn(Segment segment)
    {
        int halfSegmentlenght = (int)MapCreator.instance.segmentLenght / 2;
        int random = Random.Range(- halfSegmentlenght , halfSegmentlenght);
        position = new Vector3(segment.Xpos + random, segment.ground.height + 1);
        rotation = Quaternion.Euler(0, 0, 0);
        element = ObjectPooler.Instance.SpawnFromPool(objectTag, position, rotation);
    }
}
