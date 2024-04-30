using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public static int wins;
    public static int maxHealth = 3;
    public static int health = maxHealth;
    public static List<Collectable.CollectableType> inventory = new List<Collectable.CollectableType>{Collectable.CollectableType.Axe}; // default inventory state
    public static bool canSeePath = false;
    public static string playerName = "Anon"; // default player name
    public const string API_KEY = ""; //replace with api key when the leaderboard server is made
    public const string leaderboardAPI = ""; //replace with url for leaderboard server when its made
}
