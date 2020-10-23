using System.Collections.Generic;
using UnityEngine;

public static class PieceDefinitions
{
    //This class was intended to act as a sort of collection of "templates" adding pieces to the board
    //but when I tried it it seemed to work initially but there are problems regarding identity that come from this.
    //All of the "white pawns" would have the same identity in memory and so when you change _hasMoved for one of them you would unintentonally
    //change it for all of them. I am leaving this here for now, just in case I need it again.
    static readonly Dictionary<string, Piece> Pieces = new Dictionary<string, Piece>() 
    {
        {"White Pawn", new Piece(PieceColor.White, PieceType.Pawn, "White Pawn", new Vector2[] { new Vector2(0, -1), new Vector2(0, -2) }, "Sprites/pawn_white") },
        {"Black Pawn", new Piece(PieceColor.Black, PieceType.Pawn, "Black Pawn", new Vector2[] { new Vector2(0, 1), new Vector2(0, 2) }, "Sprites/pawn_black") },
        {"White King", new Piece(PieceColor.White, PieceType.King, "White King", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, 1), new Vector2(1, -1) }, "Sprites/king_white") },
        {"Black King", new Piece(PieceColor.Black, PieceType.King, "Black King", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, 1), new Vector2(1, -1) }, "Sprites/king_black") },
        {"White Rook", new Piece(PieceColor.White, PieceType.Rook, "White Rook", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0) }, "Sprites/rook_white") },
        {"Black Rook", new Piece(PieceColor.Black, PieceType.Rook, "Black Rook", new Vector2[] { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0) }, "Sprites/rook_black") },
        {"White Knight", new Piece(PieceColor.White, PieceType.Knight, "White Knight", new Vector2[] { new Vector2(2, 1), new Vector2(1, 2), new Vector2(-2, 1), new Vector2(-1, 2), new Vector2(2, -1), new Vector2(1, -2), new Vector2(-2, -1), new Vector2(-1, -2) }, "Sprites/knight_white") },
        {"Black Knight", new Piece(PieceColor.Black, PieceType.Knight, "Black Knight", new Vector2[] { new Vector2(2, 1), new Vector2(1, 2), new Vector2(-2, 1), new Vector2(-1, 2), new Vector2(2, -1), new Vector2(1, -2), new Vector2(-2, -1), new Vector2(-1, -2) }, "Sprites/knight_black") }
    };
}