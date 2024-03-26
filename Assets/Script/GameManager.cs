using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState GameState;

    public Tilemap LevelTilemap;
    
    public TextAsset BlockingTilesJson;
    public Dictionary<string, bool> BlockingTiles;

    public Stack<Stack<IReversibleAction>> actionStack = new Stack<Stack<IReversibleAction>>();
    public Stack<IReversibleAction> currentTurnActions;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        BlockingTiles = JsonConvert.DeserializeObject<Dictionary<string, bool>>(BlockingTilesJson.text);

        ChangeGameState(0);
        InputManager.Instance.enabled = true;
    }

    public void ChangeGameState(GameState gameState)
    {
        GameState = gameState;
        Debug.Log("Game State: " + gameState);
        switch (gameState)
        {
            case GameState.SpawnObject:
                GameObject test_ett = Resources.Load<GameObject>("Prefabs/RandomMovingEtt");
                Vector2Int test_ett_position = new Vector2Int(-1, 3);
                EntityManager.Instance.CreateEntity(test_ett, test_ett_position);

                GameObject skeleton = Resources.Load<GameObject>("Prefabs/skeleton_static");
                Vector2Int skeleton_pos= new Vector2Int(-5, -3);
                EntityManager.Instance.CreateEntity(skeleton, skeleton_pos, new Vector2Int(1, 0));

                Item item = Resources.Load<Item>("Items/New Key");
                Vector2Int item_position = new Vector2Int(3, 3);
                EntityManager.Instance.CreateItemEntity(item, item_position);
                ChangeGameState(GameState.SpawnPlayer);
                break;
            case GameState.SpawnPlayer:
                GameObject player = Resources.Load<GameObject>("Prefabs/PlayableChar");
                Vector2Int playerPosition = new Vector2Int(-5, 0);
                EntityManager.Instance.CreateEntity(player, playerPosition);
                ChangeGameState(GameState.PlayerTurn);
                break;
            case GameState.PlayerTurn:
                currentTurnActions = new Stack<IReversibleAction>();
                break;
            case GameState.EnvTurn:
                EntityManager.Instance.EnvTurn();
                break;
            case GameState.EndTurn:
                actionStack.Push(currentTurnActions);
                ChangeGameState(GameState.PlayerTurn);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null);
        }
    }

    public void AddAction(IReversibleAction action)
    {
        currentTurnActions.Push(action);
        action.Perform();
    }

    public void Undo()
    {
        if (actionStack.Count == 0)
        {
            Debug.Log("No actions to undo");
            return;
        }
        Stack<IReversibleAction> lastTurnActions = actionStack.Pop();
        Debug.Log("Undoing " + lastTurnActions.Count + " actions");
        while (lastTurnActions.Count > 0)
        {
            IReversibleAction action = lastTurnActions.Pop();
            action.Undo();
        }
        EntityManager.Instance.UpdateEntites();
    }
}

public enum GameState
{
    SpawnObject,
    SpawnPlayer,
    PlayerTurn,
    EnvTurn,
    EndTurn,
}
