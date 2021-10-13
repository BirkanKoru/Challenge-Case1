using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private SpriteRenderer square;
    [SerializeField] private SpriteRenderer cross;

    private bool selected = false;
    
    public void Select()
    {
        selected = true;
        cross.enabled = true;
    }

    public void DeSelect()
    {
        selected = false;
        cross.enabled = false;
    }

    public bool GetSelected()
    {
        return selected;
    }
}
