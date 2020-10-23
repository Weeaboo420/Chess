using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    private Dictionary<int, Piece> _pieces = new Dictionary<int, Piece>();
    private List<GameObject> _moves = new List<GameObject>();

    private PieceColor _currentPlayerColor = PieceColor.White;
    private GameObject _piecePrefab;
    private GameObject _movePrefab;
    private GameObject _piecesContainer;
    private GameObject _movesContainer;
    private GameObject _selectedPieceGameObject;
    private Piece _selectedPiece;

    private ushort _pieceIndex = 0, _previousSelectedIndex = 1000;
    private Vector2 _minBounds, _maxBounds;
    private bool _shouldLerpPiece = false;
    private Vector2 _newPiecePosition;

    private Transform _cameraTransform;
    private bool _cameraFlipped = false;

    //Is a within the margin of b?
    //Example: InRange(5, 6, 2) > true
    private bool InRange(float a, float b, float margin)
    {
        if(a >= (b - margin) && a <= (b + margin))
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        if(_shouldLerpPiece && _selectedPieceGameObject != null)
        {
            _selectedPieceGameObject.transform.position = Vector2.Lerp(_selectedPieceGameObject.transform.position, _newPiecePosition, 30f * Time.deltaTime);

            if((Vector2)_selectedPieceGameObject.transform.position == _newPiecePosition)
            {        
                _shouldLerpPiece = false;
                _selectedPieceGameObject = null;   
            }
        }

        if(Input.GetKeyDown(KeyCode.R)) //Reload the level if "R" is pressed
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Input.GetKeyDown(KeyCode.X)) //Flip the board 180 degrees
        {
            FlipCamera();
        }

    }

    private void Start()
    {
        GameObject tilePrefab = Resources.Load<GameObject>("Prefabs/tile");
        _piecePrefab = Resources.Load<GameObject>("Prefabs/piece");
        _movePrefab = Resources.Load<GameObject>("Prefabs/move");        

        GameObject board = new GameObject("Board");
        board.transform.position = new Vector3(0, 0, 0);

        _piecesContainer = new GameObject("Pieces Container");
        _piecesContainer.transform.position = new Vector3(0, 0, 0);

        _movesContainer = new GameObject("Moves Container");
        _movesContainer.transform.position = new Vector3(0, 0, 0);

        _cameraTransform = GameObject.FindObjectOfType<Camera>().GetComponent<Transform>();

        //Generate board
        int index = 0; //Used for making the checkered pattern

        for (int y = 0; y < Globals.BoardSize; y++, index++)
        {
            for(int x = 0; x < Globals.BoardSize; x++, index++)
            {
                GameObject newTile = Instantiate(tilePrefab, new Vector3(x*0.8f - 4*0.7f, -y*0.8f + 4*0.7f, 10), Quaternion.identity);
                newTile.transform.parent = board.transform;

                if(x == 0 && y == 0) //Set negative bounds for the board
                {
                    _minBounds = newTile.transform.position;
                }
                
                if(x == Globals.BoardSize-1 && y == Globals.BoardSize-1) //Set positive bounds for the board
                {
                    _maxBounds = newTile.transform.position;
                }

                //First row. White side, add rooks, knights, bishops, king and queen
                if(y == 0)
                {
                    //White king
                    if(x == 3)
                    {                        
                        Piece newPiece = new Piece(PieceColor.White, PieceType.King, "White King", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, 1), new Vector2(1, -1) }, "Sprites/king_white");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }

                    //White queen
                    else if (x == 4)
                    {
                        Piece newPiece = new Piece(PieceColor.White, PieceType.Queen, "White Queen", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, 1), new Vector2(1, -1) }, "Sprites/queen_white");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }

                    //White knights
                    else if(x == 1 || x == 6)
                    {
                        //Piece newPiece = PieceDefinitions.Pieces["White Knight"];
                        Piece newPiece = new Piece(PieceColor.White, PieceType.Knight, "White Knight", new Vector2[] { new Vector2(2, 1), new Vector2(1, 2), new Vector2(-2, 1), new Vector2(-1, 2), new Vector2(2, -1), new Vector2(1, -2), new Vector2(-2, -1), new Vector2(-1, -2) }, "Sprites/knight_white");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }

                    //White rooks
                    else if (x == 0 || x == 7)
                    {
                        Piece newPiece = new Piece(PieceColor.White, PieceType.Rook, "White Rook", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0) }, "Sprites/rook_white");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }

                    //White bishops
                    else if(x == 2 || x == 5)
                    {
                        Piece newPiece = new Piece(PieceColor.White, PieceType.Bishop, "White Bishop", new Vector2[] { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(1, 1) }, "Sprites/bishop_white");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }
                }

                //Add white pawns
                else if(y == 1)
                {
                    Piece newPiece = new Piece(PieceColor.White, PieceType.Pawn, "White Pawn", new Vector2[] { new Vector2(0, -1), new Vector2(0, -2) }, "Sprites/pawn_white");
                    newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                    PlacePiece(_pieceIndex, newPiece);                    
                }

                //Add black pawns
                else if(y == 6)
                {
                    Piece newPiece = new Piece(PieceColor.Black, PieceType.Pawn, "Black Pawn", new Vector2[] { new Vector2(0, 1), new Vector2(0, 2) }, "Sprites/pawn_black");
                    newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                    PlacePiece(_pieceIndex, newPiece);
                }

                //Last row. Black side, add rooks, knights, bishops, king and queen
                else if(y == 7)
                {
                    //Black king
                    if(x == 3)
                    {
                        Piece newPiece = new Piece(PieceColor.Black, PieceType.King, "Black King", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, 1), new Vector2(1, -1) }, "Sprites/king_black");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }

                    //Black queen
                    else if(x == 4)
                    {
                        Piece newPiece = new Piece(PieceColor.Black, PieceType.Queen, "Black Queen", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, 1), new Vector2(1, -1) }, "Sprites/queen_black");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }

                    //Black knights
                    else if (x == 1 || x == 6)
                    {
                        Piece newPiece = new Piece(PieceColor.Black, PieceType.Knight, "Black Knight", new Vector2[] { new Vector2(2, 1), new Vector2(1, 2), new Vector2(-2, 1), new Vector2(-1, 2), new Vector2(2, -1), new Vector2(1, -2), new Vector2(-2, -1), new Vector2(-1, -2) }, "Sprites/knight_black");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }

                    //Black rooks
                    else if(x == 0 || x == 7)
                    {
                        Piece newPiece = new Piece(PieceColor.Black, PieceType.Rook, "Black Rook", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0) }, "Sprites/rook_black");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }

                    //Black bishops
                    else if (x == 2 || x == 5)
                    {
                        Piece newPiece = new Piece(PieceColor.Black, PieceType.Bishop, "Black Bishop", new Vector2[] { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(-1, 1), new Vector2(1, 1) }, "Sprites/bishop_black");
                        newPiece.Position = newTile.transform.position + new Vector3(0, 0, -10);
                        PlacePiece(_pieceIndex, newPiece);
                    }
                }

                if(index % 2 == 0)
                {
                    newTile.GetComponent<SpriteRenderer>().color = Globals.WhiteTileColor;
                } else
                {
                    newTile.GetComponent<SpriteRenderer>().color = Globals.BlackTileColor;
                }               
            }
        }

        FlipCamera();

    }

    private void FlipCamera()
    {        
        _cameraFlipped = !_cameraFlipped;
        switch(_cameraFlipped)
        {
            case true:
                _cameraTransform.rotation = Quaternion.Euler(new Vector3(_cameraTransform.localEulerAngles.x, _cameraTransform.localEulerAngles.y, 180f));
                break;
            case false:
                _cameraTransform.rotation = Quaternion.Euler(new Vector3(_cameraTransform.localEulerAngles.x, _cameraTransform.localEulerAngles.y, 0));
                break;
        }

        foreach(Transform piece in _piecesContainer.transform)
        {   
            SpriteRenderer pieceSpriteRenderer = piece.GetComponent<SpriteRenderer>();         
            pieceSpriteRenderer.flipX = _cameraFlipped;
            pieceSpriteRenderer.flipY = _cameraFlipped;

        }
        
    }

    public void MovePiece(Vector2 NewPosition)
    {
        if(!_shouldLerpPiece)
        {
            _pieces[_previousSelectedIndex].Position = NewPosition;
            _selectedPieceGameObject.GetComponent<PieceObject>().SetPosition(NewPosition);        

            _pieces[_previousSelectedIndex].HasMoved = true;

            _selectedPiece = null;
            _previousSelectedIndex = 1000;
            _shouldLerpPiece = true;
            _newPiecePosition = NewPosition;

            foreach (GameObject move in _moves)
            {
                Destroy(move);
            }

            _moves.Clear();

            //Turn management for black and white
            if(_currentPlayerColor == PieceColor.White)
            {
                _currentPlayerColor = PieceColor.Black;
            } else {
                _currentPlayerColor = PieceColor.White;
            }
        }

    }

    private int AddMove(Vector2 PieceObjectPosition, Vector2 Move)
    {
        //Return codes:
        //0 = Success, continue adding moves if possible
        //1 = Obstruction, stop adding moves in this direction


        Vector3 newPosition = new Vector3(PieceObjectPosition.x + Move.x * 0.8f, PieceObjectPosition.y + Move.y * 0.8f, -1);        

        //Make sure that we don't add moves on occupied squares
        foreach(KeyValuePair<int, Piece> entry in _pieces)
        {
            if(entry.Value.Position == (Vector2)newPosition)
            {
                if(entry.Value.GetColor() != _selectedPiece.GetColor())
                {
                    if (newPosition.x >= _minBounds.x && newPosition.x <= _maxBounds.x)
                    {
                        if (newPosition.y <= _minBounds.y && newPosition.y >= _maxBounds.y)
                        {
                            //Adds a special red capture move
                            GameObject newMove = Instantiate(_movePrefab, new Vector3(newPosition.x, newPosition.y, 2), Quaternion.identity);
                            
                            Move moveScript = newMove.GetComponent<Move>();
                            moveScript.SetInfo(this);
                            moveScript.SetColor(Globals.CaptureColor);

                            newMove.transform.localScale = new Vector3(4, 4, 1);
                            newMove.transform.Find("center").localScale = new Vector3(1, 1, 1);                            

                            _moves.Add(newMove);
                            newMove.transform.parent = _movesContainer.transform;
                        }
                    }
                }

                return 1; 
            }
        }

        //Before adding a move, make sure it is inside the confines of the board.
        if (newPosition.x >= _minBounds.x && newPosition.x <= _maxBounds.x)
        {
            if (newPosition.y <= _minBounds.y && newPosition.y >= _maxBounds.y)
            {
                GameObject newMove = Instantiate(_movePrefab, newPosition, Quaternion.identity);
                newMove.GetComponent<Move>().SetInfo(this);
                _moves.Add(newMove);
                newMove.transform.parent = _movesContainer.transform;
            }
        }

        return 0;
    }

    private void PlacePiece(ushort PieceIndex, Piece Piece)
    {     
        GameObject newPiece = Instantiate(_piecePrefab, Piece.Position, Quaternion.identity);
        newPiece.transform.parent = _piecesContainer.transform;
        Piece.Name = Piece.Name + "_" + PieceIndex.ToString();
        newPiece.transform.name = Piece.Name;

        SpriteRenderer spriteRenderer = newPiece.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Piece.GetSprite();

        if(Piece.GetColor() == PieceColor.White)
        {
            spriteRenderer.color = Globals.WhitePieceColor;
        } else
        {
            spriteRenderer.color = Globals.BlackPieceColor;
        }

        PieceObject pieceObject = newPiece.AddComponent<PieceObject>();
        pieceObject.SetInfo(PieceIndex, this, Piece.Position);

        _pieces.Add(PieceIndex, Piece);
        _pieceIndex++;
    }

    public void SelectPiece(ushort PieceIndex, Vector2 PieceObjectPosition, GameObject SelectedPieceGameObject)
    {
        if(!_shouldLerpPiece)
        {
            _newPiecePosition = PieceObjectPosition;

            //If the selected piece is clicked again, remove all "moves" and unset the selected piece
            if (_selectedPiece != null && _selectedPieceGameObject != null && _previousSelectedIndex == PieceIndex)
            {
                _selectedPiece = null;
                _selectedPieceGameObject = null;

                foreach (GameObject move in _moves)
                {
                    Destroy(move);
                }
                _moves.Clear();
            }

            else
            {
                //Make sure that we only move the white or black pieces when it's white's or black's turn respectively.
                if(_pieces[PieceIndex].GetColor() == _currentPlayerColor)
                {
                    _selectedPiece = _pieces[PieceIndex];
                    _previousSelectedIndex = PieceIndex;
                    _selectedPieceGameObject = SelectedPieceGameObject;
                    PieceType selectedPieceType = _selectedPiece.GetPieceType();

                    //Remove old "moves" if there are any
                    foreach (GameObject move in _moves)
                    {
                        Destroy(move);
                    }

                    _moves.Clear();


                    //Display all possible moves
                    foreach (Vector2 move in _selectedPiece.GetDefaultMoves())
                    {
                        int result = 0; //Used to determine if there is an obstruction along a path. See AddMove for more details


                            if (selectedPieceType != PieceType.Bishop && selectedPieceType != PieceType.Queen && selectedPieceType != PieceType.Rook)
                            {
                                //Special case for pawns
                                if (selectedPieceType == PieceType.Pawn)
                                {
                                    //If the pawn has not moved at all
                                    if (!_selectedPiece.HasMoved && result == 0)
                                    {
                                        result = AddMove(PieceObjectPosition, move);
                                    }

                                    //If the pawn has been moved at least once
                                    else
                                    {
                                        if (Mathf.Abs(move.y) == 1 && result == 0) //Only add moves for the pawn that will move it one square
                                        {
                                            result = AddMove(PieceObjectPosition, move);
                                        }
                                    }
                                }

                                else
                                {
                                    if (result == 0)
                                    {
                                        result = AddMove(PieceObjectPosition, move);
                                    }
                                }
                            }

                            //Special case for Bishop, Rook and Queen who can move "infinitely" across the bord
                            else
                            {
                                for (int i = 1; i < 8; i++)
                                {
                                    if (result == 0)
                                    {
                                        result = AddMove(PieceObjectPosition, new Vector2(move.x * i, move.y * i));
                                    }
                                }
                            }

                    }
                }

            }
        }
    }

}
