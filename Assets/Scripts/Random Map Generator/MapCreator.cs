using UnityEngine;

namespace RandomMapGenerator
{
    public class MapCreator
    {
        struct Node
        {
            public Vector2 worldPosition;
            public int gridX;
            public int gridY;
            public bool isEmpty;

            public Node(Vector2 _worldPos, int _gridX, int _gridY, bool _state)
            {
                isEmpty = _state;
                worldPosition = _worldPos;
                gridX = _gridX;
                gridY = _gridY;
            }
        }

        private Node[,] Grid;
        private int gridSizeX;
        private int gridSizeY;
        private Vector2 nodeDiameter;
        private Vector2[] BaseCoordinate;
        private  Vector2 MapPosition;
        
        private Vector2 GridWorldSize;
        private int CountBase;
        private float BaseRadius;
        
        private bool DisplayGizmos = true;

        
        public MapCreator(Vector2 MapSize, int _CountBase, float _BaseRadius, Vector2 _MapPosition)
        {
            GridWorldSize = MapSize;
            CountBase = _CountBase;
            BaseRadius = _BaseRadius;
            MapPosition = _MapPosition;
            
            nodeDiameter.x = GridWorldSize.x / CountBase;
            nodeDiameter.y = GridWorldSize.y / CountBase;
            gridSizeX = Mathf.RoundToInt(GridWorldSize.x / nodeDiameter.x);
            gridSizeY = Mathf.RoundToInt(GridWorldSize.y / nodeDiameter.y);
            CreateBaseMap();
        }

        public void CreateGrid()
        {
            Grid = new Node[gridSizeX, gridSizeY];
            Vector2 worldBottomLeft = MapPosition - Vector2.right * GridWorldSize.x / 2 -
                                      Vector2.up * GridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter.x + nodeDiameter.x / 2) +
                                         Vector2.up * (y * nodeDiameter.y + nodeDiameter.y / 2);
                    Grid[x, y] = new Node(worldPoint, x, y, true);
                }
            }
        }

        public Vector2[] CreateBaseMap()
        {
            CreateGrid();
            BaseCoordinate = new Vector2[CountBase];

            int layerCount = 1; //Mathf.RoundToInt( CountBase / 8);
            if (BaseRadius > nodeDiameter.x || BaseRadius > nodeDiameter.y)
                layerCount = Mathf.RoundToInt( BaseRadius / nodeDiameter.x);

            for (int i = 0; i < CountBase; i++)
            {
                if (i < 4)
                {
                    switch (i)
                    {
                        case 0:
                            BaseCoordinate[i] = Grid[0, 0].worldPosition;
                            Grid[0, 0].isEmpty = false;
                            CloseNeighbours(Grid[0, 0], layerCount);
                            break;
                        case 3:
                            BaseCoordinate[i] = Grid[0, gridSizeY - 1].worldPosition;
                            Grid[0, gridSizeY - 1].isEmpty = false;
                            CloseNeighbours(Grid[0, gridSizeY - 1], layerCount);

                            break;
                        case 2:
                            BaseCoordinate[i] = Grid[gridSizeX - 1, 0].worldPosition;
                            Grid[gridSizeX - 1, 0].isEmpty = false;
                            CloseNeighbours(Grid[gridSizeX - 1, 0], layerCount);

                            break;
                        case 1:
                            BaseCoordinate[i] = Grid[gridSizeX - 1, gridSizeY - 1].worldPosition;
                            Grid[gridSizeX - 1, gridSizeY - 1].isEmpty = false;
                            CloseNeighbours(Grid[gridSizeX - 1, gridSizeY - 1], layerCount);

                            break;
                    }
                }
                else
                {
                    int x = 0;
                    int y = 0;

                    while (!Grid[x, y].isEmpty)
                    {
                        x = Mathf.RoundToInt(Random.Range(0, gridSizeX));
                        y = Mathf.RoundToInt(Random.Range(0, gridSizeY));
                    }

                    BaseCoordinate[i] = Grid[x, y].worldPosition;
                    Grid[x, y].isEmpty = false;
                    CloseNeighbours(Grid[x, y], layerCount);
                    if (i < CountBase - 1)
                    {
                        Node n = NodeFromWorldPoint(Grid[x, y].worldPosition * (-1.0f));
                        if (n.isEmpty)
                        {
                            i++;
                            BaseCoordinate[i] = n.worldPosition;
                           // n.isEmpty = false;
                            CloseNeighbours(n, layerCount);
                            Grid[n.gridX, n.gridY].isEmpty = false;
                        }
                    }
                }
            }

            return BaseCoordinate;
        }

        private void CloseNeighbours(Node node, int depth = 1)
        {

            for (int x = -depth; x <= depth; x++)
            {
                for (int y = -depth; y <= depth; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        Grid[checkX, checkY].isEmpty = false;
                    }
                }
            }
        }

        private Node NodeFromWorldPoint(Vector2 worldPosition)
        {
            float percentX = (worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x;
            float percentY = (worldPosition.y + GridWorldSize.y / 2) / GridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return Grid[x, y];
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(MapPosition, new Vector2(GridWorldSize.x, GridWorldSize.y));
            if (Grid != null && DisplayGizmos)
            {
                Color temp;
                foreach (Node n in Grid)
                {
                    if (n.isEmpty)
                        temp = Color.white;
                    else
                    {
                        temp = Color.red;
                    }
                    temp.a = 0.5f;
                    Gizmos.color = temp;
                    Gizmos.DrawCube(n.worldPosition, new Vector3((nodeDiameter.x - 0.1f), (nodeDiameter.y - 0.1f)));
                }

                if (BaseCoordinate != null)
                {
                    foreach (var pos in BaseCoordinate)
                    {
                        temp = Color.red;
                        Gizmos.color = temp;

                        Gizmos.DrawSphere(pos, BaseRadius);
                    }
                }
            }
        }
    }
}
