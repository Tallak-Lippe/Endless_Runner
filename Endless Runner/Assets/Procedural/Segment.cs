using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment
{
    public List<ProceduralElement> elements;

    public float Xpos;

    public int listIndex;

    public Ground ground;

    public void Move(float distance)
    {
        Xpos += distance;
        ground.Move(distance);
        foreach (ProceduralElement element in elements)
        {
            element.Move(distance);
        }
    }

    public void Spawn()
    {
        ground.Spawn(Xpos);
        if(elements != null)
        {
            foreach (ProceduralElement element in elements)
            {
                element.CalculateAndSpawn(this);
            }
        }
        
    }

    public Segment(float x, int index)
    {
        Xpos = x;
        listIndex = index;
    }

    public void Delete()
    {
        ground.element.SetActive(false);
        if (elements != null)
        {
            foreach (ProceduralElement element in elements)
            {
                element.element.SetActive(false);
            }
        }
    }
}
