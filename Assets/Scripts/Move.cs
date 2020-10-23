using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private GameManager _gameManagerReference;
    private SpriteRenderer[] _spriteRenderers;

    private void Awake()
    {
        _spriteRenderers = new SpriteRenderer[2] 
        {
            GetComponent<SpriteRenderer>(),
            transform.Find("center").GetComponent<SpriteRenderer>()
        };        
    }

    public void SetInfo(GameManager GameManagerReference)
    {
        _gameManagerReference = GameManagerReference;
    }

    public void SetColor(Color32 NewColor)
    {
        foreach(SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            Color32 oldColor = spriteRenderer.color;
            spriteRenderer.color = new Color32(NewColor.r, NewColor.g, NewColor.b, oldColor.a);
        }
    }

    private void OnMouseDown()
    {
        if(_gameManagerReference != null)
        {
            _gameManagerReference.MovePiece((Vector2)transform.position);
        }
    }
}
