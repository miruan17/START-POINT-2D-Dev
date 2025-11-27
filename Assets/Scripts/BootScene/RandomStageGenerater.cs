using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public interface IMapStrategy
{
    public void GenerateRoom();
    public void SetSpawnRoom();
    public void GenerateBossRoom();
}

public class TestCase : IMapStrategy
{
    private int Case = 0;
    private int Topology;

    public TestCase()
    {
        Topology = 0;
    }
    public void GenerateRoom()
    {
        
    }
    public void SetSpawnRoom()
    {
        
    }
    public void GenerateBossRoom()
    {
        
    }
}

public class Case1 : IMapStrategy
{
    private int Case = 1;
    private int Topology;

    public Case1()
    {
        Topology = 0;
    }
    public void GenerateRoom()
    {
        
    }
    public void SetSpawnRoom()
    {
        
    }
    public void GenerateBossRoom()
    {
        
    }
}


public class RandomStageGenerater
{
    private int mapSeed = UnityEngine.Random.Range(0,3);
    
}
