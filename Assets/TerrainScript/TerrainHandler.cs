using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* --- Terrain Generator Manual: --
    To interact with the Terrain you can use these methods:
    ----------------
    > public List<Vector3> GetMountainPeaks() - Returns a List of Vector3s that include all tiles where a mountain has spawned
    > public Vector3 GetTilePosition(int posX, int posY) - Returns the Vector3 right above the Tile at the given Position
    > public List<Vector3> GetObjectSpawnPoints(int amount) - Returns a List of Vector3 Position to place the objective items at, 
    evenly distributed on the entire Map, amount is the amount of objects that need to be placed
    > public Vector3 GetRandomPosition() - Returns a Vector3 Position randomly on the entire map

    */

public class TerrainHandler : MonoBehaviour
{
    // ---------------- Variables ----------------
    // to access the variables on the board from outside the class, use the get methods
    private int[,] board;
    [SerializeField] private int sizeY;
    [SerializeField] private int sizeX;
    public int Breite = 10;
    public int Höhe = 10;
    private bool sizeDefined = false;
    private bool boardCreated = false;
    public int amountOfHills = 2;
    public int hillsMaxHeight = 13;
    public int hillsMinHeight = 4;
    // the prefab Objects to be instantiated are saved in the list
    public List<GameObject> listOfTiles;
    private List<Vector3> listofPeaks = new List<Vector3>();

    // --------------- Main Methods -------------

    // Spawns Prefab Tiles according to the Landscape generated, if a board has been created
    public void CreateLandscape()
    {
        if(boardCreated)
        {
            Vector3 _spawnPos = new Vector3(0,0,0);
            GameObject _currentPrefab = listOfTiles[0];
            for(int i = 0; i < sizeX; i++)
            {
                for(int j = 0; j < sizeY; j++)
                {
                    if( board[i,j] > 8 )
                    {
                        if(listOfTiles.Count >=3)
                        {
                            _currentPrefab = listOfTiles[2];
                        }
                        else _currentPrefab = listOfTiles[0];
                    }
                    else if ( board[i,j] <= 4)
                    {
                        _currentPrefab = listOfTiles[0];
                    }
                    else
                    {
                        if(listOfTiles.Count >=3)
                        {
                            _currentPrefab = listOfTiles[1];
                        }
                        else _currentPrefab = listOfTiles[0];
                    }
                    int _currentValue = board[i,j];

                    Instantiate(_currentPrefab, _spawnPos + (Vector3.up * _currentValue / 2) + GenerateShift(10f) , Quaternion.identity);
                    _spawnPos = _spawnPos + new Vector3(0,0,1);

                }
                _spawnPos = _spawnPos + new Vector3(1,0,-sizeY);
            }
        }
    }
    private void InsertVfx(string effect, int posX, int posY)
    {
        if( effect == "snow")
        {
            //put in snow
        }
    }
    // alternative Spawner that stacks the prefabs on top of each other instead of shifting their height
    public void CreateLandscapeMinecraft()
    {
        if(boardCreated)
        {
            Vector3 _spawnPos = new Vector3(0,0,0);
            GameObject _currentPrefab = listOfTiles[0];
            for(int i = 0; i < sizeX; i++)
            {
                for(int j = 0; j < sizeY; j++)
                {
                    if( board[i,j] > 8 )
                    {
                        _currentPrefab = listOfTiles[2];
                    }
                    else if ( board[i,j] <= 4)
                    {
                        _currentPrefab = listOfTiles[0];
                    }
                    else
                    {
                        _currentPrefab = listOfTiles[1];
                    }
                    int _currentValue = board[i,j];

                    for ( int k = 0; k < _currentValue; k++)
                    {
                        Instantiate(_currentPrefab, _spawnPos + (Vector3.up * k) , Quaternion.identity);
                    }

                    _spawnPos = _spawnPos + new Vector3(0,0,1);

                }
                _spawnPos = _spawnPos + new Vector3(1,0,-sizeY);
            }
        }
    }

    // Generates random Hills and fills the board accordingly with the InsertHill method, the parameter decides how many hills
    public void GenerateBoard(int hills)
    {
        if(sizeDefined)
        {
            InitialiseBoard();
            listofPeaks = new List<Vector3>();
            // After the board is filled with 1s to prevent issues, the hills are inserted
            for(int l = 0; l < hills; l++)
            {
                int _currentHillHeight = Mathf.RoundToInt( UnityEngine.Random.Range(hillsMinHeight, hillsMaxHeight));
                // stay 4 squares away from the edge to enable creating a beach
                int _posX = Mathf.RoundToInt( UnityEngine.Random.Range(4, sizeX-4));
                int _posY = Mathf.RoundToInt( UnityEngine.Random.Range(4, sizeY-4));

                Debug.Log("Inserting new hill with a height of "+_currentHillHeight+" at position: "+_posX+" and"+_posY);

                InsertHill(_posX,_posY,_currentHillHeight);
                // This is the main peak of the hill, so we want to insert a Snow Vfx here
                listofPeaks.Add(new Vector3(_posX,_currentHillHeight,_posY));
                // the hill doesnt have just one peak
                int _amountOfAdditionalPeaks = UnityEngine.Random.Range(0, 4);
                for(int k = 0; k < _amountOfAdditionalPeaks; k++)
                {
                    int _vertShift = UnityEngine.Random.Range(-1, +1);
                    int _horiShift = UnityEngine.Random.Range(-1, +1);
                    InsertHill((_posX + _horiShift) , (_posY + _vertShift), _currentHillHeight);
                }
            }
            //the hills are done, lets create a beach all around it
            FlattenEdge();

            Debug.Log("board generated");
            boardCreated = true;

        }
    }
    private void FlattenEdge()
    {
        // go around the edge, set the edges to -1, then set the landscape further inland to an average of the existing value and 1
        for(int i = 0; i < sizeX; i++)
        {
            for(int k = 0; k < sizeY; k++)
            {
                if( i == 0 || i == sizeX-1 || k == 0 || k == sizeY-1) // at the edges of the X values
                {
                    board[i,k] = -1;
                }
                else if( i == 1 || i == sizeX-2 || k == 1 || k == sizeY-2) // 1 field from the edges
                {
                    board[i,k] = 0;
                }
                else if( i == 2 || i == sizeX-3 || k == 2 || k == sizeY-3) // 2 fields from the edges
                {
                    int _thisValue = board[i,k];
                    if ( _thisValue <= 3)
                    {
                        board[i,k] = 1; // use Math average instead
                    }
                    else
                    {
                        //board[i,k] = ((_thisValue +1) / 2);
                        //_thisValue = board[i,k];
                        /* Vector3 _spawnPos = new Vector3(i,0,k);
                        for ( int l = -1; l < _thisValue; l++)
                        {
                            Instantiate(listOfTiles[1], _spawnPos + (Vector3.up * l) , Quaternion.identity);
                        } */
                        Vector3 _spawnPos = new Vector3(i,10,k);
                        Instantiate(listOfTiles[1], _spawnPos , Quaternion.identity);
                    }
                    //board[i,k] = ((_thisValue +1) / 2); // use Math average instead
                }
                else if( i == 3 || i == sizeX-4 || k == 3 || k == sizeY-4) // 3 fields from the edges
                {
                    int _thisValue = board[i,k];
                    if( _thisValue < 6 )
                    {
                        board[i,k] = ((_thisValue +1) / 2); // use Math average instead
                    }
                    else
                    {
                        //board[i,k] = ((_thisValue +1) / 2);
                        //_thisValue = board[i,k];
                        Vector3 _spawnPos = new Vector3(i,0,k);
                        for ( int l = -1; l < _thisValue; l++)
                        {
                            Instantiate(listOfTiles[1], _spawnPos + (Vector3.up * l) , Quaternion.identity);
                        }
                    }
                }
                else if( i == 3 || i == sizeX-4 || k == 3 || k == sizeY-4) // 4 fields from the edges
                {
                    int _thisValue = board[i,k];
                    if( _thisValue > 6 )
                    {
                        Vector3 _spawnPos = new Vector3(i,0,k);
                        for ( int l = -1; l < _thisValue; l++)
                        {
                            Instantiate(listOfTiles[1], _spawnPos + (Vector3.up * l) , Quaternion.identity);
                        }
                    }
                }
                else if( i == 4 || i == sizeX-5 || k == 4 || k == sizeY-5) // 4 fields from the edges
                {
                    int _thisValue = board[i,k];
                    if( _thisValue > 6 )
                    {
                        Vector3 _spawnPos = new Vector3(i,0,k);
                        for ( int l = -1; l < _thisValue; l++)
                        {
                            Instantiate(listOfTiles[1], _spawnPos + (Vector3.up * l) , Quaternion.identity);
                        }
                    }
                }
                else if( i == 5 || i == sizeX-6 || k == 5 || k == sizeY-6) // 4 fields from the edges
                {
                    int _thisValue = board[i,k];
                    if( _thisValue > 6 )
                    {
                        Vector3 _spawnPos = new Vector3(i,0,k);
                        for ( int l = -1; l < _thisValue; l++)
                        {
                            Instantiate(listOfTiles[1], _spawnPos + (Vector3.up * l) , Quaternion.identity);
                        }
                    }
                }
                else
                {
                    // do nothing
                }
            }
        }
    }
    // Inserts the Hill into the Array, distance from the hills spawnpoint determines the height
    // A random chance decides if the new field doesnt lose height
    private void InsertHill(int x, int y, int height)
    {
        for(int i = 0; i < sizeX; i++)
        {
            for(int k = 0; k < sizeY; k++)
            {
                int _thisValue = board[i,k];
                int _distanceX = Mathf.Abs(x-i);
                int _distanceY = Mathf.Abs(y-k);
                    
                int _addheight;
                if ( GenerateRandom() < 3f )
                {
                    _addheight = 1;
                }
                else   
                {
                    _addheight = 0;
                }


                int _value = height + _addheight - Mathf.Max(_distanceX, _distanceY);

                if( _value > _thisValue)
                {
                    board[i,k] = _value;
                }
            }
        }
    }

    // ------ Helper Methods ------------

    private Vector3 FindRandomFlatSpot()
    {
        int _posX = Mathf.RoundToInt( UnityEngine.Random.Range(3, sizeX-3));
        int _posY = Mathf.RoundToInt( UnityEngine.Random.Range(3, sizeY-3));
        if( board[_posX,_posY] == 1)
        {
            Vector3 _spot = new Vector3(_posX,board[_posX,_posY] + 0.0001f,_posY);
            return _spot;
        }
        else
        {
            return FindRandomFlatSpot();
        }  
    }
    private Vector3 FindRandomFlatSpot(int section, int sectionSize)
    {
        int _posX = Mathf.RoundToInt( UnityEngine.Random.Range(3+section, 3+section+sectionSize));
        int _posY = Mathf.RoundToInt( UnityEngine.Random.Range(3, sizeY-3));
        if( board[_posX,_posY] == 1)
        {
            Vector3 _spot = new Vector3(_posX,board[_posX,_posY] + 0.0001f,_posY);
            return _spot;
        }
        else
        {
            return FindRandomFlatSpot();
        } 
    }
    private Vector3 FindRandomPeakSpot()
    {
        int _posX = Mathf.RoundToInt( UnityEngine.Random.Range(3, sizeX-3));
        int _posY = Mathf.RoundToInt( UnityEngine.Random.Range(3, sizeY-3));
        if( board[_posX,_posY] >= (hillsMaxHeight + hillsMinHeight / 2))
        {
            Vector3 _spot = new Vector3(_posX,board[_posX,_posY] + 0.0001f,_posY);
            return _spot;
        }
        else
        {
            return FindRandomPeakSpot();
        }  
    }

    // Takes the asset from the parameter and finds a random flat spot in the world to Instantiate it in at the given height
    private void InsertAsset(GameObject prefab, int height)
    {
        Vector3 _spot = FindRandomFlatSpot();
        Instantiate(prefab, _spot + (Vector3.up * height) , Quaternion.identity);
    }
    // returns an random number between 1 and 100
    private int GenerateRandom()
    {
        return UnityEngine.Random.Range(1, 100);
    }

    // Adds a random shift down or up for more realism, float value is the chance of a shift happening
    private Vector3 GenerateShift(float chance)
    {
        int _random = UnityEngine.Random.Range(1, 100);
        Vector3 _shift = new Vector3(0,0,0);
        if ( _random < (chance/2) )
        {
            _shift = new Vector3(0,0.04f,0);
        }
        else if( _random < (100 - chance/2) )
        {
            _shift = new Vector3(0,-0.04f,0);
        }
        return _shift;
    }


    /* ------ Methods for interacting with he Landscape from outside the class ------
       ------------------------------------------------------------------------------
       ------ Setters to influence the Board before Creation ------  */

    // board Setter, returns true if successfull, size can be from 5 to 1000
    public bool SetSize(int x, int y)
    {
        if ( x < 5 || y < 5)
        {
            Debug.Log("Size cant be smaller than 5");
            sizeDefined = false;
            return sizeDefined;
        }
        else if( x > 1000 || y > 1000)
        {
            Debug.Log("Size cant be larger than 1000");
            sizeDefined = false;
            return sizeDefined;
        }
        else
        {
            sizeY = y;
            sizeX = x;
            sizeDefined = true;
            return sizeDefined;
        }
    }

    /* ----- Getters to interact with the Landscape from outside the class ---
       -----------------------------------------------------------------------
       Goals: Returning a Vector3 to spawn trees and decorative objects
              Returning a List to spawn the objective-relative Items
              Returning a List of all the Mountain Peaks to add the Vfx to it
              Return the Vector3 of any Tile for spawning on that specific tile
    
     */
    public List<Vector3> GetMountainPeaks()
    {
        if(boardCreated)
        {
            return listofPeaks;
        }
        else
        {
            Debug.Log("board is not created yet");
            return listofPeaks;
        }
    }
    public Vector3 GetTilePosition(int posX, int posY)
    {
        if(boardCreated)
        {
            return new Vector3(posX, board[posX,posY] + 0.0001f, posY);
        }
        else
        {
            Debug.Log("board is not created yet");
            return new Vector3(0,0,0);
        }
    }
    public List<Vector3> GetObjectSpawnPoints(int amount)
    {
        List<Vector3> _spawnpoints= new List<Vector3>();
        if(boardCreated)
        {
            int _section = (sizeX-6) / amount;
            for(int i = 0; i < amount; i++)
            {
                _spawnpoints.Add(FindRandomFlatSpot(i* _section, _section));
            }
            return _spawnpoints;
        }
        else
        {
            Debug.Log("board is not created yet");
            return _spawnpoints;
        }
    }
    public Vector3 GetRandomPosition()
    {
        if(boardCreated)
        {
            int _posX = Mathf.RoundToInt( UnityEngine.Random.Range(3, sizeX-3));
            int _posY = Mathf.RoundToInt( UnityEngine.Random.Range(3, sizeY-3));
            
            Vector3 _spot = new Vector3(_posX,board[_posX,_posY] + 0.0001f,_posY);
            return _spot;
            
        }
        else return new Vector3(0,0,0);
    }
    public int GetSizeX()
    {
        return sizeX;
    }
    public int GetSizeY()
    {
        return sizeY;
    }
    // Getter for the board values
    public int GetBoardValue(int posX, int posY)
    {
        if(boardCreated)
        {
            return board[posX,posY];
        }
        else 
        {
            Debug.Log("The board hasnt been created yet, no values to return");
            return 0;
        }
    }




    /* ---- Helper methods ---- */

    // Fills the entire Array with 1s to prevent a Null reference when later changing and comparing values
    public void InitialiseBoard()
    {
        if(sizeDefined)
        {
            board = new int[sizeX,sizeY];
            //Debug.Log("The board is "+sizeX+" wide and"+sizeY+" high");
            for(int i = 0; i < sizeX; i++)
            {
                for(int k = 0; k < sizeY; k++)
                {
                    board[i,k] = 1;
                }
            }
        }
    }

    public void MyDebug()
    {
        string output = "";
        for(int i = 0; i < sizeX; i++)
        {
            for(int k = 0; k < sizeY; k++)
            {
                //Debug.Log(" "+board[i,k]);
                output += board[i,k];
            }
            Debug.Log(output);
            output = "";
        }
        //Debug.Log(output);
        
    }
    void Awake()
    {
        SetSize(Breite,Höhe);
        GenerateBoard(amountOfHills);
        MyDebug();
        CreateLandscape();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

}