using UnityEngine;

public enum PieceColor
{
    White,
    Black
}

public enum PieceType
{
    Pawn,
    Knight,
    King,
    Queen,
    Rook,
    Bishop
}

//Attached on each "Piece game object". Holds an index that refers to a piece inside a dictionary in GameManager.
[RequireComponent(typeof(BoxCollider2D))]
public class PieceObject : MonoBehaviour
{
    private ushort _index;
    private GameManager _gameManagerReference;
    private Vector2 _piecePosition;

    public void SetInfo(ushort Index, GameManager GameManagerReference, Vector2 PiecePosition)
    {
        _index = Index;
        _gameManagerReference = GameManagerReference;
        _piecePosition = PiecePosition;
    }

    public void SetPosition(Vector2 NewPosition)
    {
        _piecePosition = NewPosition;
    }

    public ushort Index
    {
        get
        {
            return _index;
        }

        set
        {
            _index = value;
        }
    }

    public void OnMouseDown()
    {
        _gameManagerReference.SelectPiece(_index, _piecePosition, this.gameObject);
    }
}

public class Piece
{
    
    private PieceColor _color;
    private PieceType _type;
    private string _name;
    private Vector2[] _defaultMoves;
    private Sprite _sprite;
    private Vector2 _position;
    private bool _hasMoved = false;

    public Piece(PieceColor Color, PieceType Type, string Name, Vector2[] DefaultMoves, string SpritePath)
    {       
        _color = Color;
        _type = Type;
        _name = Name;
        _defaultMoves = DefaultMoves;
        _sprite = Resources.Load<Sprite>(SpritePath);
    }

    //Used only for the pawns.
    public bool HasMoved
    {
        get
        {
            return _hasMoved;
        }

        set
        {
            _hasMoved = value;
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }
        
        set
        {
            _name = value;
        }
    }

    public Vector2[] GetDefaultMoves()
    {
        return _defaultMoves;
    }

    public PieceColor GetColor()
    {
        return _color;
    }

    public Vector2 Position
    {
        get
        {
            return _position;
        }

        set
        {
            _position = value;
        }
    }

    public Sprite GetSprite()
    {
        return _sprite;
    }

    public PieceType GetPieceType()
    {
        return _type;
    }
}
