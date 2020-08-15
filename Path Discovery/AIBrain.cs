using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    public enum AlgorithymType { AlwaysRight, PathDiscovery};

    public AlgorithymType algorithymType;

    public AIBehaviour aiBehaviour;

    private bool knowsObjLocation;

    private bool justFoundObjective;

    private NodeIndex objectiveLocation;

    private bool hasGoal;

    private NodeIndex currentGoal;

    private Path currentPath;

    public NodeIndex Think()
    {
        switch (algorithymType)
        {
            case AlgorithymType.AlwaysRight:
                return AlwaysRightBehaviour();

            case AlgorithymType.PathDiscovery:
                return PathDiscoveryBehaviour();
        }

        return new NodeIndex(0,0) ;
    }

    private NodeIndex PathDiscoveryBehaviour()
    {
        //Check if the AI have recently mapped the objective or if the last path followed reached its destination. if so, get a new path
        if(justFoundObjective || currentPath == null || (currentPath != null && currentPath.ReachedDestination()))
        {
            currentPath = GetNewPath();
        }

        //return null;

        //Returns the current node index in the path
        return currentPath.GetNextDestination();
    }

    private Path GetNewPath()
    {
        return GetNewPathDestination();

        /*
        Debug.Log("---------------- DESTINATION ----------------");

        Utilities.PrintNode(destinationNode.GetNodeIndex());
        */

        //Path path = Utilities.AStar(Utilities.GetNodeFromNodeIndex(NodeTable.instance.GetNodesList(), NodeTable.instance.GetCurrentNodeIndex()), destinationNode);

        //path.PrintPath();

        //return path;
    }

    
    private Path GetNewPathDestination()
    {
        if(knowsObjLocation)
        {
            return Utilities.AStar(Utilities.GetNodeFromNodeIndex(NodeTable.instance.GetNodesList(), NodeTable.instance.GetCurrentNodeIndex()), Utilities.GetNodeFromNodeIndex(NodeTable.instance.GetNodesList(), objectiveLocation));
        }
        
        List<PathDestinationCandidate> canditates = new List<PathDestinationCandidate>();

       // Debug.Log("-----------------------------");

        for (int i = 0; i < aiBehaviour.nodeTable.nonVisitedNodes.Count; i++)
        {
            //The non discovered connections must lead to, at least, one non known node

            int promisingValue = aiBehaviour.nodeTable.nonVisitedNodes[i].GetPromissingValue();

            if (promisingValue > 0)
            {
                PathDestinationCandidate newCandidate = new PathDestinationCandidate(aiBehaviour.nodeTable.nonVisitedNodes[i], promisingValue);
                //Debug.Log("Adicionou candidato line " + aiBehaviour.nodeTable.nonVisitedNodes[i].GetNodeIndex().line + "  column " + aiBehaviour.nodeTable.nonVisitedNodes[i].GetNodeIndex().column);
                canditates.Add(newCandidate);
            }
            //If thet node is not a promissing one, remove from the list to avoid future validations
            else
            {
                aiBehaviour.nodeTable.nonVisitedNodes.RemoveAt(i);
            }
        }

        int chosen = 0;
        int chosenSize = 100;
        int chosenValue = -100;

        List<Path> paths = new List<Path>();

        for (int i = 0; i < canditates.Count; i++)
        {
            Path path = Utilities.AStar(Utilities.GetNodeFromNodeIndex(NodeTable.instance.GetNodesList(), NodeTable.instance.GetCurrentNodeIndex()), canditates[i].GetNode());
            path.SetPromissingValue(canditates[i].GetPromissingValue());
            paths.Add(path);
        }

        List<Path> minimunPath = new List<Path>();

        for (int i = 0; i < paths.Count; i++)
        {
            int pathSize = paths[i].GetNodesList().Count;
            if (pathSize < chosenSize)
            {
                chosenSize = pathSize;

                minimunPath.Clear();
                minimunPath.Add(paths[i]);
            }
            else if (pathSize == chosenSize)
            {
                minimunPath.Add(paths[i]);
            }

           // Path path = Utilities.AStar(Utilities.GetNodeFromNodeIndex(NodeTable.instance.GetNodesList(), NodeTable.instance.GetCurrentNodeIndex()), canditates[i].GetNode());
        }

        for (int i = 0; i < minimunPath.Count; i++)
        {
            int pathValue = minimunPath[i].GetPromissingValue();
            if (pathValue > chosenValue)
            {
                chosenValue = pathValue;
                chosen = i;
            }

            // Path path = Utilities.AStar(Utilities.GetNodeFromNodeIndex(NodeTable.instance.GetNodesList(), NodeTable.instance.GetCurrentNodeIndex()), canditates[i].GetNode());
        }

        return minimunPath[chosen];
    }

    private NodeIndex CheckNeighbourNonVisitedNodes()
    {
        NodeIndex newNodeIndex;

        //Tries to move to the right
        newNodeIndex = NodeDirectionUtilities.GetRightDirectionOfNode(aiBehaviour.nodeTable.GetCurrentNodeIndex(), aiBehaviour.GetFacingDirection());
        if (aiBehaviour.nodeTable.CheckNodesConnection(aiBehaviour.nodeTable.GetCurrentNodeIndex(), newNodeIndex))
        {
            return newNodeIndex;
        }

        //Tries to move to the front
        newNodeIndex = NodeDirectionUtilities.GetFrontDirectionOfNode(aiBehaviour.nodeTable.GetCurrentNodeIndex(), aiBehaviour.GetFacingDirection());
        if (aiBehaviour.nodeTable.CheckNodesConnection(aiBehaviour.nodeTable.GetCurrentNodeIndex(), newNodeIndex))
        {
            return newNodeIndex;
        }

        //Tries to move to the left
        newNodeIndex = NodeDirectionUtilities.GetLeftDirectionOfNode(aiBehaviour.nodeTable.GetCurrentNodeIndex(), aiBehaviour.GetFacingDirection());
        if (aiBehaviour.nodeTable.CheckNodesConnection(aiBehaviour.nodeTable.GetCurrentNodeIndex(), newNodeIndex))
        {
            return newNodeIndex;
        }

        //Returns null if there is no elegible neighbour
        return null;
    }
    
    private NodeIndex AlwaysRightBehaviour()
    {
        NodeIndex newNodeIndex;

        //Tries to move to the right
        newNodeIndex = NodeDirectionUtilities.GetRightDirectionOfNode(aiBehaviour.nodeTable.GetCurrentNodeIndex(), aiBehaviour.GetFacingDirection());
        if (aiBehaviour.nodeTable.CheckNodesConnection(aiBehaviour.nodeTable.GetCurrentNodeIndex(),newNodeIndex))
        {
            return newNodeIndex;
        }

        //Tries to move to the front
        newNodeIndex = NodeDirectionUtilities.GetFrontDirectionOfNode(aiBehaviour.nodeTable.GetCurrentNodeIndex(), aiBehaviour.GetFacingDirection());
        if (aiBehaviour.nodeTable.CheckNodesConnection(aiBehaviour.nodeTable.GetCurrentNodeIndex(), newNodeIndex))
        {
            return newNodeIndex;
        }

        //Tries to move to the left
        newNodeIndex = NodeDirectionUtilities.GetLeftDirectionOfNode(aiBehaviour.nodeTable.GetCurrentNodeIndex(), aiBehaviour.GetFacingDirection());
        if (aiBehaviour.nodeTable.CheckNodesConnection(aiBehaviour.nodeTable.GetCurrentNodeIndex(), newNodeIndex))
        {
            return newNodeIndex;
        }

        //Tries to move to the back
        newNodeIndex = NodeDirectionUtilities.GetBackDirectionOfNode(aiBehaviour.nodeTable.GetCurrentNodeIndex(), aiBehaviour.GetFacingDirection());
        if (aiBehaviour.nodeTable.CheckNodesConnection(aiBehaviour.nodeTable.GetCurrentNodeIndex(), newNodeIndex))
        {
            return newNodeIndex;
        }

        Debug.LogError("Direction calculation is wrong. Didnt find any");
        return newNodeIndex;
    }
}

public class PathDestinationCandidate
{
    private Node node;

    private int distance;

    private int promissingValue;

    public PathDestinationCandidate(Node node, int promissingValue)
    {
        this.node = node;
        this.promissingValue = promissingValue;
    }

    public Node GetNode()
    {
        return node;
    }

    public int GetPromissingValue()
    {
        return promissingValue;
    }
}

public class Path
{
    private int currentNode;

    private int cost;

    private int promissingValue;

    List<Node> nodes;

    Node destinationNode;

    public Path(Node startingNode, Node destination)
    {
        currentNode = 1;

        nodes = new List<Node>();
        nodes.Add(startingNode);

        destinationNode = destination;
    }

    public Path(Path startingPath)
    {
        currentNode = 1;

        this.destinationNode = startingPath.GetDestination();

        nodes = new List<Node>();

        for (int i = 0; i < startingPath.GetNodesList().Count; i++)
        {
            nodes.Add(startingPath.GetNodesList()[i]);
        }

       // nodes = startingPath.GetNodesList().GetRange(0, startingPath.GetNodesList().Count - 1);
    }

    public void SetPromissingValue(int value)
    {
        promissingValue = value;
    }

    public int GetPromissingValue()
    {
        return promissingValue;
    }

    public void AddNode(Node node)
    {
        nodes.Add(node);
        CalulateCost();
    }

    public NodeIndex GetNextDestination()
    {
        currentNode++;

        return (nodes[currentNode - 1]).GetNodeIndex();
    }

    public Node GetLastNode()
    {
        return nodes[nodes.Count - 1];
    }

    public Node GetLastButOneNode()
    {
        if (nodes.Count > 1)
        {
            return nodes[nodes.Count - 2];
        }
        else
        {
            return nodes[nodes.Count - 1];
        }
    }

    public bool ReachedDestination()
    {
        return (currentNode == nodes.Count);
    }

    public List<Node> GetNodesList()
    {
        return nodes;
    }

    private void CalulateCost()
    {
        this.cost = nodes.Count + Utilities.GetRawDistance(GetLastNode().GetNodeIndex(), destinationNode.GetNodeIndex());
    }

    public int GetPathCost()
    {
        return cost;
    }

    public Node GetDestination()
    {
        return destinationNode;
    }

    public void PrintPath()
    {
        Debug.Log("--------------- PATH ---------------");

        for (int i = 0; i < nodes.Count; i++)
        {
            Debug.Log(i + " - line: " + nodes[i].GetNodeIndex().line + " column: " + nodes[i].GetNodeIndex().column);
        }
    }
}