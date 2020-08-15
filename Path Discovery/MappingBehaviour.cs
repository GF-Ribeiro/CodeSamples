using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Front, Right, Back, Left}

public class MappingBehaviour : MonoBehaviour
{
    public SensorHitCheck sensorsFront;
    public SensorHitCheck sensorsLeft;
    public SensorHitCheck sensorsRight;

    public NodeTable nodeTable;

    public IEnumerator MapSurroundings(Direction facingDirection)
    {
        //Get the results from the front sensors
        CollisionType[] sensorsFrontResults = sensorsFront.GetIsPathFree();

        //Get the results from the left sensors
        CollisionType[] sensorsLeftResults = sensorsLeft.GetIsPathFree();

        //Get the results from the right sensors
        CollisionType[] sensorsRightResults = sensorsRight.GetIsPathFree();

        NodeIndex previousNodeIndex = null;


        previousNodeIndex = nodeTable.GetCurrentNodeIndex();
        for (int i = 0; i < sensorsFrontResults.Length; i++)
        {
            if (sensorsFrontResults[i] == CollisionType.None)
            {
                NodeIndex newNodeIndex = null;
                Direction nodesConnection = Direction.Front;

                switch (facingDirection)
                {
                    case Direction.Front:
                        nodesConnection = Direction.Front;
                        newNodeIndex = GetNodeIndexUp(i);
                        break;

                    case Direction.Left:
                        nodesConnection = Direction.Left;
                        newNodeIndex = GetNodeIndexLeft(i);
                        break;

                    case Direction.Right:
                        nodesConnection = Direction.Right;
                        newNodeIndex = GetNodeIndexRight(i);
                        break;

                    case Direction.Back:
                        nodesConnection = Direction.Back;
                        newNodeIndex = GetNodeIndexDown(i);
                        break;
                }

                nodeTable.NewAddNode(newNodeIndex, previousNodeIndex, nodesConnection);
                previousNodeIndex = newNodeIndex;
            }
            else if (sensorsFrontResults[i] == CollisionType.Objective)
            {
                NodeIndex newNodeIndex = null;
                Direction nodesConnection = Direction.Front;

                switch (facingDirection)
                {
                    case Direction.Front:
                        nodesConnection = Direction.Front;
                        newNodeIndex = GetNodeIndexUp(i);
                        break;

                    case Direction.Left:
                        nodesConnection = Direction.Left;
                        newNodeIndex = GetNodeIndexLeft(i);
                        break;

                    case Direction.Right:
                        nodesConnection = Direction.Right;
                        newNodeIndex = GetNodeIndexRight(i);
                        break;

                    case Direction.Back:
                        nodesConnection = Direction.Back;
                        newNodeIndex = GetNodeIndexDown(i);
                        break;
                }
                nodeTable.NewAddNode(newNodeIndex, previousNodeIndex, nodesConnection, true);
                previousNodeIndex = newNodeIndex;
            }
            else
            {
                NodeIndex newNodeIndex = null;
                Direction nodesConnection = Direction.Front;

                switch (facingDirection)
                {
                    case Direction.Front:
                        nodesConnection = Direction.Front;
                        newNodeIndex = GetNodeIndexUp(i);
                        break;

                    case Direction.Left:
                        nodesConnection = Direction.Left;
                        newNodeIndex = GetNodeIndexLeft(i);
                        break;

                    case Direction.Right:
                        nodesConnection = Direction.Right;
                        newNodeIndex = GetNodeIndexRight(i);
                        break;

                    case Direction.Back:
                        nodesConnection = Direction.Back;
                        newNodeIndex = GetNodeIndexDown(i);
                        break;
                }

                nodeTable.AddNodeNoConnection(newNodeIndex, previousNodeIndex, nodesConnection);
                break;
            }
        }

        previousNodeIndex = nodeTable.GetCurrentNodeIndex();
        for (int i = 0; i < sensorsLeftResults.Length; i++)
        {
            if (sensorsLeftResults[i] == CollisionType.None)
            {
                NodeIndex newNodeIndex = null;
                Direction nodesConnection = Direction.Front;

                switch (facingDirection)
                {
                    case Direction.Front:
                        newNodeIndex = GetNodeIndexLeft(i);
                        nodesConnection = Direction.Left;
                        break;

                    case Direction.Left:
                        newNodeIndex = GetNodeIndexDown(i);
                        nodesConnection = Direction.Back;
                        break;

                    case Direction.Right:
                        newNodeIndex = GetNodeIndexUp(i);
                        nodesConnection = Direction.Front;
                        break;

                    case Direction.Back:
                        newNodeIndex = GetNodeIndexRight(i);
                        nodesConnection = Direction.Right;
                        break;
                }

                nodeTable.NewAddNode(newNodeIndex, previousNodeIndex, nodesConnection);
                previousNodeIndex = newNodeIndex;
            }
            else
            {
                NodeIndex newNodeIndex = null;
                Direction nodesConnection = Direction.Front;

                switch (facingDirection)
                {
                    case Direction.Front:
                        newNodeIndex = GetNodeIndexLeft(i);
                        nodesConnection = Direction.Left;
                        break;

                    case Direction.Left:
                        newNodeIndex = GetNodeIndexDown(i);
                        nodesConnection = Direction.Back;
                        break;

                    case Direction.Right:
                        newNodeIndex = GetNodeIndexUp(i);
                        nodesConnection = Direction.Front;
                        break;

                    case Direction.Back:
                        newNodeIndex = GetNodeIndexRight(i);
                        nodesConnection = Direction.Right;
                        break;
                }

                nodeTable.AddNodeNoConnection(newNodeIndex, previousNodeIndex, nodesConnection);
                break;
            }
        }

        previousNodeIndex = nodeTable.GetCurrentNodeIndex();
        for (int i = 0; i < sensorsRightResults.Length; i++)
        {
            if (sensorsRightResults[i] == CollisionType.None)
            {
                NodeIndex newNodeIndex = null;
                Direction nodesConnection = Direction.Front;

                switch (facingDirection)
                {
                    case Direction.Front:
                        newNodeIndex = GetNodeIndexRight(i);
                        nodesConnection = Direction.Right;
                        break;

                    case Direction.Left:
                        newNodeIndex = GetNodeIndexUp(i);
                        nodesConnection = Direction.Front;
                        break;

                    case Direction.Right:
                        newNodeIndex = GetNodeIndexDown(i);
                        nodesConnection = Direction.Back;
                        break;

                    case Direction.Back:
                        newNodeIndex = GetNodeIndexLeft(i);
                        nodesConnection = Direction.Left;
                        break;
                }

                nodeTable.NewAddNode(newNodeIndex, previousNodeIndex, nodesConnection);
                previousNodeIndex = newNodeIndex;
            }
            else
            {
                NodeIndex newNodeIndex = null;
                Direction nodesConnection = Direction.Front;

                switch (facingDirection)
                {
                    case Direction.Front:
                        newNodeIndex = GetNodeIndexRight(i);
                        nodesConnection = Direction.Right;
                        break;

                    case Direction.Left:
                        newNodeIndex = GetNodeIndexUp(i);
                        nodesConnection = Direction.Front;
                        break;

                    case Direction.Right:
                        newNodeIndex = GetNodeIndexDown(i);
                        nodesConnection = Direction.Back;
                        break;

                    case Direction.Back:
                        newNodeIndex = GetNodeIndexLeft(i);
                        nodesConnection = Direction.Left;
                        break;
                }

                nodeTable.AddNodeNoConnection(newNodeIndex, previousNodeIndex, nodesConnection);
                break;
            }
        }
        yield return null;
    }

    private NodeIndex GetNodeIndexUp (int index)
    {
        return new NodeIndex(nodeTable.GetCurrentNodeIndex().line + (index + 1), nodeTable.GetCurrentNodeIndex().column);
    }

    private NodeIndex GetNodeIndexDown(int index)
    {
        return new NodeIndex(nodeTable.GetCurrentNodeIndex().line - (index + 1), nodeTable.GetCurrentNodeIndex().column );
    }

    private NodeIndex GetNodeIndexLeft(int index)
    {
        return new NodeIndex(nodeTable.GetCurrentNodeIndex().line, nodeTable.GetCurrentNodeIndex().column - (index + 1));
    }

    private NodeIndex GetNodeIndexRight(int index)
    {
        return new NodeIndex(nodeTable.GetCurrentNodeIndex().line , nodeTable.GetCurrentNodeIndex().column + (index + 1));
    }

}
