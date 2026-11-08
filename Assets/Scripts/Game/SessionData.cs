using UnityEngine;
// Static so the player name survives scene changes.
public static class SessionData
{
    public static string PlayerName { get; set; } = "";
}
