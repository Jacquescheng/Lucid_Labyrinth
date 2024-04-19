using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Skull : Enemy
{
    public override string Label => "Skull";
    public int curDir = 4;
    public int clockwise = 1;
    public bool lastTurnStop = false;
    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(1, 0)
    };
    // Update is called once per frame
    void Update()
    {

    }

    public void MoveCircle()
    {

        int curDir = this.curDir;
        int clockwise = this.clockwise;
        bool lastTurnStop = this.lastTurnStop;
        //check continue clockwise
        if (EntityManager.Instance.IsPositionBlocked(position + clockwise * directions[curDir % 4]))
        {
            clockwise *= -1;
            if(!lastTurnStop)
            {
                curDir += clockwise;
            }
            
            if (curDir < 0)
            {
                curDir += 4;
            }
        }

        //check can move
        if (!EntityManager.Instance.IsPositionBlocked(position + clockwise * directions[curDir % 4]))
        {
            //if ((curDir % 2 == 1 && clockwise > 0) || (curDir % 2 == 0 && clockwise < 0))
            //{
            //    GameManager.Instance.AddAction(new ChangeFacingAction(this, -facingDirection));
            //}
            GameManager.Instance.AddAction(new MoveAction(this, clockwise * directions[curDir % 4]));

            curDir += clockwise;
        }
        else
        {
            clockwise *= -1;
            if (!EntityManager.Instance.IsPositionBlocked(position + clockwise * directions[curDir % 4]))
            {
                //if ((curDir % 2 == 1 && clockwise > 0) || (curDir % 2 == 0 && clockwise < 0))
                //{
                //    GameManager.Instance.AddAction(new ChangeFacingAction(this, -facingDirection));
                //}
                GameManager.Instance.AddAction(new MoveAction(this, clockwise * directions[curDir % 4]));

                curDir += clockwise;
            }
            else
            {
                lastTurnStop = true;
            }
        }
    
        if (curDir < 0)
        {
            curDir += 4;
        }
        GameManager.Instance.AddAction(new SkullUpdateAction(this, curDir, clockwise, lastTurnStop));

    }
    public override void Action()
    {
        MoveCircle();
    }
}

public class SkullUpdateAction : IReversibleAction
{
    public Skull skull;
    public int curDirBefore;
    public int curDirAfter;
    public int clockwiseBefore;
    public int clockwiseAfter;
    public bool lastTurnStopBefore;
    public bool lastTurnStopAfter;
    public SkullUpdateAction(Skull skull, int curDir, int clockwise, bool lastTurnStop)
    {
        this.skull = skull;
        this.curDirBefore = skull.curDir;
        this.curDirAfter = curDir;
        this.clockwiseBefore = skull.clockwise;
        this.clockwiseAfter = clockwise;
        this.lastTurnStopBefore = skull.lastTurnStop;
        this.lastTurnStopAfter = lastTurnStop;
    }

    public void Perform()
    {
        skull.curDir = curDirAfter;
        skull.clockwise = clockwiseAfter;
        skull.lastTurnStop = lastTurnStopAfter;
    }
    public void Undo()
    {
        skull.curDir = curDirBefore;
        skull.clockwise = clockwiseBefore;
        skull.lastTurnStop = lastTurnStopBefore;
    }
}
