
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PathFinding : MonoBehaviour
{
    public Tilemap roadTile;
    public Vector3Int startNode;
    public Vector3Int endNode;
    private Node currentNode;
    [SerializeField] private PathfindingType modeType;
    [SerializeField] private List<Vector3Int> path;

    protected HashSet<Vector3Int> obstacles = new();
    public virtual List<Vector3Int> FindPath(Vector3Int startNode, Vector3Int endNode)
    {
        switch (modeType)
        {
            case PathfindingType.Automatic:
                currentNode = new Node(startNode);
                currentNode.parentNode = null;
                List<Vector3Int> path = new List<Vector3Int>();
                path.Add(currentNode.posittion);
                while (currentNode.posittion != endNode)
                {
                    foreach (Node newNode in GetNeighbors(currentNode))
                    {
                        if (IsWalking(newNode.posittion) && (currentNode.parentNode == null || !newNode.CompareTo(currentNode.parentNode)))
                        {
                            path.Add(newNode.posittion);
                            newNode.parentNode = currentNode;
                            currentNode = newNode;
                            break;
                        }
                    }


                }
                return path;
            case PathfindingType.Manual:
                return this.path;
            default:
                return null;
                
        }
    }
    protected List<Node> GetNeighbors(Node node)
    {

        Vector3Int posittion = node.posittion;
        List<Node> result = new List<Node>()
        {
            new Node(new Vector3Int(posittion.x + 1, posittion.y, posittion.z)),
            new Node(new Vector3Int(posittion.x - 1, posittion.y, posittion.z)),
            new Node(new Vector3Int(posittion.x, posittion.y + 1, posittion.z)),
            new Node(new Vector3Int(posittion.x, posittion.y - 1, posittion.z))
        };
        List<Node> randomResult = new List<Node>();
        while (result.Count > 0)
        {
            int randomIndex = Random.Range(0, result.Count);
            randomResult.Add(result[randomIndex]);
            result.RemoveAt(randomIndex);
        }


        return randomResult;  
    }
    protected bool IsWalking(Vector3Int posittion)
    {
        return roadTile.HasTile(posittion) && !obstacles.Contains(posittion);
    }
    public void SetObstacle(object obj)
    {
        if(obj is Vector3Int position)
        {
            obstacles.Add(position);
        }

    }
    public void RemoveObstacle(Vector3Int posittion)
    {
        obstacles.Remove(posittion);
    }
    public Vector3Int GetCellInMap(Vector3 posstion)
    {
        return roadTile.WorldToCell(posstion);
    }

}
