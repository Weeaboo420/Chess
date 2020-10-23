using UnityEngine;

public static class Globals
{
    public const int BoardSize = 8;


    public static readonly Color32 WhiteTileColor = new Color32(255, 255, 255, 255);
    //public static readonly Color32 BlackTileColor = new Color32(29, 29, 29, 255);
    public static readonly Color32 BlackTileColor = new Color32(59, 93, 140, 255);

    public static readonly Color32 WhitePieceColor = new Color32(255, 255, 255, 255);
    public static readonly Color32 BlackPieceColor = new Color32(255, 255, 255, 255);

    public static readonly Color32 MoveColor = new Color32(125, 232, 157, 255);
    public static readonly Color32 CaptureColor = new Color32(173, 71, 71, 255);
}