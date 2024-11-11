using UnityEngine;

public static class Logger
{
    public static string Colourize(this string str, uint colour) =>
        $"<color=#{colour:X6}>{str}</color>";

    public static void Info(string message) =>
        Debug.Log($"{"[".Colourize(0x202020)}{"INFO".Colourize(0x8080F0)}{"]: ".Colourize(0x202020)}{message}");
       
    public static void Warn(string message) =>
        Debug.Log($"{"[".Colourize(0x202020)}{"WARN".Colourize(0xF0F080)}{"]: ".Colourize(0x202020)}{message}");

    public static void Error(string message) =>
        Debug.Log($"{"[".Colourize(0x202020)}{"OOPS".Colourize(0xF08080)}{"]: ".Colourize(0x202020)}{message}");
}
