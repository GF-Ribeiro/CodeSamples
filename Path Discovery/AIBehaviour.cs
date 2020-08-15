using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public float movementTime;

    public float thinkingTime;

    public MappingBehaviour mappingBehaviour;

    public NodeGraphVisualizer nodeGraphVisualizer;

    public NodeTable nodeTable;

    public AIBrain aiBrain;

    private Direction facingDirection;

    public Direction startingOrientation;

    public void Awake()
    {
        facingDirection = startingOrientation;

        nodeTable.Initialize();

        nodeGraphVisualizer = GameObject.FindGameObjectWithTag("GraphVisualizer").GetComponent<NodeGraphVisualizer>();
    }

    private void Start()
    {
        StartCoroutine(MainBehaviour());
    }

    IEnumerator MainBehaviour()
    {
        //Mapping
        StartCoroutine(mappingBehaviour.MapSurroundings(facingDirection));

        //Showing Map
        //
        yield return StartCoroutine(nodeGraphVisualizer.GenerateGraph(nodeTable.GetNodesList()));

        //Thinking
        NodeIndex newNodeIndex = aiBrain.Think();

        yield return new WaitForSeconds(thinkingTime);
        
        //Moving
        yield return StartCoroutine(RotateAI(GetNewFacingDirection(nodeTable.GetCurrentNodeIndex(), newNodeIndex)));
        
        yield return StartCoroutine(AIMovement.GoForward(this.transform, movementTime));

        nodeTable.SetCurrentNodeIndex(newNodeIndex);

        StartCoroutine(MainBehaviour());
    }

    private IEnumerator RotateAI(Direction newFacingDirection)
    {
        if(newFacingDirection != facingDirection)
        {
            switch(newFacingDirection)
            {
                case Direction.Front:
                    yield return StartCoroutine(AIMovement.Rotate(this.transform, 0, new Vector3(0, 1, 0), 0.3f));
                    break;

                case Direction.Left:
                    yield return StartCoroutine(AIMovement.Rotate(this.transform, -90, new Vector3(0, 1, 0), 0.3f));
                    break;

                case Direction.Right:
                    yield return StartCoroutine(AIMovement.Rotate(this.transform, 90, new Vector3(0, 1, 0), 0.3f));
                    break;

                case Direction.Back:
                    yield return StartCoroutine(AIMovement.Rotate(this.transform, 180, new Vector3(0, 1, 0), 0.3f));
                    break;
            }
            
            this.facingDirection = newFacingDirection;
        }
    }

    public Direction GetNewFacingDirection(NodeIndex currentNodeIndex, NodeIndex newNodeIndex)
    {
        if (newNodeIndex.line == currentNodeIndex.line + 1 && newNodeIndex.column == currentNodeIndex.column)
        {
            return Direction.Front;
        }

        if (newNodeIndex.line == currentNodeIndex.line && newNodeIndex.column == currentNodeIndex.column + 1)
        {
            return Direction.Right;
        }

        if (newNodeIndex.line == currentNodeIndex.line && newNodeIndex.column == currentNodeIndex.column - 1)
        {
            return Direction.Left;
        }

        if (newNodeIndex.line == currentNodeIndex.line - 1 && newNodeIndex.column == currentNodeIndex.column)
        {
            return Direction.Back;
        }

        Debug.LogError("There is no relation between the nodes");
        return Direction.Front;
    }

    public Direction GetFacingDirection()
    {
        return facingDirection;
    }
}