using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static List<GameObject> playerTeam = new List<GameObject>();
    public static List<GameObject> enemyTeam = new List<GameObject>();
    
    public static void InitializeTeams()
    {

    }
    
    /*static Dictionary<string, List<Movement>> units = new Dictionary<string, List<Movement>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<Movement> turnTeam = new Queue<Movement>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (turnTeam.Count == 0)
        {
            InitTeamTurnQueue();
        }
		
	}

    static void InitTeamTurnQueue()
    {
        List<Movement> teamList = units[turnKey.Peek()];

        foreach(Movement unit in teamList)
        {
            turnTeam.Enqueue(unit);
        }
        StartTurn();
    }

    public static void StartTurn()
    {
        if (turnTeam.Count > 0)
        {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn()
    {
        Movement unit = turnTeam.Dequeue();
        unit.EndTurn();

        if(turnTeam.Count > 0)
        {
            StartTurn();
        }
        else
        {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(Movement unit)
    {
        List<Movement> list;

        if (!units.ContainsKey(unit.tag))
        {
            list = new List<Movement>();
            units[unit.tag] = list;

            if (!turnKey.Contains(unit.tag))
            {
                turnKey.Enqueue(unit.tag);
            }
        }
        else
        {
            list = units[unit.tag];
        }

        list.Add(unit);
    }*/
}
