using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    //to move enviroment call: MapCreator.instance.Move(distance to move in float)
    
    List<Segment> segments;//segments currently present

    
    public List<Biome> biomes;
    Biome currentBiome;
    int segmentsLeftOfBiome;

    public float despawnX;         //the X coordinate where segments will spawn
    public float spawnX;           //the X coordinate where segments will despawn
    public float maxY;             //the highest the platform can be
    public float minY;             //the lowest the platform can be
    public float segmentLenght;
    
    public float difficulty;       //used to determine the distance of jumps etc.
                                   //will be changed during the run


    #region Singleton
    public static MapCreator instance;
    
    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Start()
    {
        SetupBiomes();

        segments = new List<Segment>();

        GetBiome();

        float distanceSpawnDespawn = Mathf.Abs(despawnX) + Mathf.Abs(spawnX);
        int numberOfSegments = Mathf.RoundToInt(distanceSpawnDespawn / segmentLenght);

        
        for(int i = 1; i <= numberOfSegments; i++)
        {
            float x = despawnX + i * segmentLenght;
            Segment segment = new Segment(x, i - 1);
            segments.Add(segment);
            CalculateSegment(segment);
            
            segment.Spawn();
        }
    }

    public void Move(float distance)
    {
        foreach(Segment segment in segments)
        {
            segment.Move(distance);
        }
    }

    private void CalculateSegment(Segment segment)
    {
        if(segment.listIndex > 0)
        {
            float height = GetHeight(segments[segment.listIndex - 1].ground.height);
            segment.ground = new Ground(height, currentBiome.GroundName);
        }
        else
        {
            segment.ground = new Ground(0, currentBiome.GroundName);
        }

        segment.elements = new List<ProceduralElement>();
        segment.elements.Add(new Obstacle(currentBiome.ObstacleName));
    }

    private void Update()
    {
        if(segments[0].Xpos < despawnX)
        {
            DeleteLastSegment();
        }
        if(segments[segments.Count - 1].Xpos < spawnX)
        {
            float firstSegmentDistance = spawnX - segments[segments.Count - 1].Xpos;
            if(firstSegmentDistance > segmentLenght)
            {
                SpawnNewSegment(segments[segments.Count - 1].Xpos + segmentLenght);
                Debug.Log("hello");
            }
        }
    }

    void SpawnNewSegment(float xPos)
    {
        Segment segment = new Segment(xPos, segments.Count);
        segments.Add(segment);
        CalculateSegment(segment);

        segment.Spawn();
    }

    void DeleteLastSegment()
    {
        Segment lastSegment = segments[0];
        lastSegment.Delete();
        segments.RemoveAt(0);
        foreach(Segment segment in segments)
        {
            segment.listIndex--;
        }
    }

    private float GetHeight(float previousHeight)
    {
        float value = previousHeight + Random.Range(-difficulty, difficulty);
        value = Mathf.Clamp(value, minY, maxY);
        
        return value;
    }

    void GetBiome()
    {
        int random = Random.Range(0, biomes.Count - 1);
        currentBiome = biomes[random];
        //lenghtOfBiomeHaveToBeAdded
    }

    void SetupBiomes()
    {
        foreach(Biome biome in biomes)
        {
            biome.SetUpObjectPooler();
        }
    }

    
}
