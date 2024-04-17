using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Skull : Enemy
{
    int curDir = 4;
    int clockwise = 1;
    bool lastTurnStop = false;
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

    }
    public override void Action()
    {
        MoveCircle();
    }
}
