using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GameInputManager
{
    static Dictionary<string, KeyCode> keyMapping;
    public enum keyMaps { Left, Up, Right, Down };
    public static KeyCode[] defaults = new KeyCode[4]
    {
        KeyCode.A,
        KeyCode.W,
        KeyCode.D,
        KeyCode.S
    };

    static GameInputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < 4; ++i)
        {
            keyMapping.Add(((keyMaps)i).ToString(), defaults[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);

        keyMapping[keyMap] = key;

        // Use Enum.TryParse to convert string to enum value
        if (Enum.TryParse(keyMap, out keyMaps direction))
        {
            // Use the enum value as index
            int index = (int)direction;
            defaults[index] = key;
            Debug.Log($"{keyMap} key set to: {key}");
        }
    }

    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }

    public static bool GetKey(string keyMap)
    {
        return Input.GetKey(keyMapping[keyMap]);
    }

    public static KeyCode GetButton(string button)
    {
        return keyMapping[button];
    }

    public static string[] GetKeyMaps()
    {
        return Enum.GetNames(typeof(keyMaps));
    }
}