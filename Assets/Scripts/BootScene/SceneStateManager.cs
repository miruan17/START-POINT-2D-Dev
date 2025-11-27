
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager
{
    private IGameState currentState;
    public SceneStateManager()
    {
        
    }
    public void changeScene(IGameState state)
    {
        currentState.UnGenerateScene();
        currentState = state;
        currentState.GenerateScene();
    }
}
