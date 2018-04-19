using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    // Holds all adjacent tiles to current tile.
    public List<Tile> adjacencyList = new List<Tile>();

    [Header("Tile Attributes")]
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    [Header("Tile Colors")]
    //public Color cwalkable;
    public Color ccurrent;
    public Color ctarget;
    public Color cselectable;
    public Color hover;
    public Color startColor;

    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        cselectable = Color.blue;
        hover = Color.yellow;
    }

    private void Update()
    {
        if (current)
        {
            rend.material.color = ccurrent;
        }
        else if (target)
        {
            rend.material.color = ctarget;
        }
        else if (selectable)
        {
            rend.material.color = cselectable;
        }
        else
        {
            rend.material.color = startColor;
        }
    }


    public void OnMouseEnter()
    {
        rend.material.color = hover;
    }

    public void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
