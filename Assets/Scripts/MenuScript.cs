using UnityEditor;
using UnityEngine;

public class MenuScript{
    /*
     * Mass assign of materials to tile objects
     */
    [MenuItem("Tools/Assign Tile Material")]
    public static void AssignMaterial()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        Material material = Resources.Load<Material>("Tile");

        foreach(GameObject t in tiles)
        {
            t.GetComponent<Renderer>().material = material;
        } 
    }
    /*
     * Mass assign of scripts to tile objects
     */
    [MenuItem("Tools/Assign Tile Script")]
    public static void AssignTileScript()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject t in tiles)
        {
            t.AddComponent<Tile>();
        }
    }
}
