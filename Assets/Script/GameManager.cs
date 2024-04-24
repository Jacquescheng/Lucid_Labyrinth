using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool isPaused = false;
    public static bool isDead = false;
    public static string killedBy;
    public static int deathCount = 0;
    public static bool won = false;
    private static bool showInstruction = true;
    public GameState state;

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

        InputManager.Instance.enabled = true;
        isDead = false;
        deathCount = 0;
        isPaused = false;
        won = false;
        ChangeGameState(0);
    }

    public void ChangeGameState(GameState gameState)
    {
        state = gameState;
        // Debug.Log("Game State: " + gameState);
        switch (gameState)
        {
            case GameState.SpawnObject:
                // GameObject test_ett = Resources.Load<GameObject>("Prefabs/RandomMovingEtt");
                // Vector2Int test_ett_position = new Vector2Int(-1, 3);
                // EntityManager.Instance.CreateEntity(test_ett, test_ett_position);

                //GameObject skeleton = Resources.Load<GameObject>("Prefabs/skeleton");
                //Vector2Int skeleton_pos= new Vector2Int(-5, -3);
                //EntityManager.Instance.CreateEntity(skeleton, skeleton_pos);

                //GameObject vampire = Resources.Load<GameObject>("Prefabs/vampire");
                //Vector2Int vampire_pos = new Vector2Int(3, -2);
                //EntityManager.Instance.CreateEntity(vampire, vampire_pos, new Vector2Int(1, 0));

                // Item item = Resources.Load<Item>("Items/New Key");
                // Vector2Int item_position = new Vector2Int(3, 3);
                // EntityManager.Instance.CreateItemEntity(item, item_position);

                // GameObject spike = Resources.Load<GameObject>("Prefabs/SpikeEntity");
                // spike.GetComponent<SpikeEntity>().open = true;
                // Vector2Int spike_position = new Vector2Int(0, 0);
                // EntityManager.Instance.CreateEntity(spike, spike_position);

                // GameObject spike_2 = Resources.Load<GameObject>("Prefabs/SpikeEntity");
                // spike_2.GetComponent<SpikeEntity>().open = false;
                // Vector2Int spike_position_2 = new Vector2Int(0, -1);
                // EntityManager.Instance.CreateEntity(spike_2, spike_position_2);

                ChangeGameState(GameState.SpawnPlayer);


                break;
            case GameState.SpawnPlayer:
                GameObject player = Resources.Load<GameObject>("Prefabs/PlayableChar");
                Vector2Int playerPosition = new Vector2Int(-5, 0);
                EntityManager.Instance.CreateEntity(player, playerPosition);

                if (showInstruction) {
                    gameObject.GetComponent<InstructionPage>().Create();
                    showInstruction = false;
                } else {
                    gameObject.GetComponent<OverlayUI>().Create();
                }

                ChangeGameState(GameState.PlayerTurn);
                break;
            case GameState.PlayerTurn:
                currentTurnActions = new Stack<IReversibleAction>();
                break;
            case GameState.EnvTurn:
                EntityManager.Instance.EnvTurn();
                break;
            case GameState.EndTurn:

                List<PlayableChar> players = new List<PlayableChar>();
                foreach (var entity in EntityManager.Instance.entities)
                {
                    if (entity.isActive)
                        if (entity is PlayableChar @char)
                        {
                            players.Add(@char);
                        }
                }

                foreach (var player_char in players)
                {
                    player_char.EndTurn();
                }


                actionStack.Push(currentTurnActions);
                if (isDead) {
                    deathCount++;
                    gameObject.GetComponent<DeadScreen>().Create();
                } else if (won) {
                    gameObject.GetComponent<EndScreen>().Create();
                }
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
