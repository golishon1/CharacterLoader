﻿using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Character")]
public class CharacterLoader : ScriptableObject
{
    [SerializeField] private string[] characters;

    public string[] Characters => characters;

    private void Reset()
    {
        var objects = Resources.LoadAll<GameObject>("Characters");

        characters = new string[objects.Length];

        for (var i = 0; i < characters.Length; i++) characters[i] = objects[i].name;
    }

    public GameObject GetCharacter(string characterName)
    {
        var objName = characters.FirstOrDefault(x => x == characterName);
        return string.IsNullOrEmpty(objName) ? null : LoadObject(characterName);
    }

    private static GameObject LoadObject(string characterName)
    {
        return Resources.Load<GameObject>($"Characters/{characterName}");
    }

    public Texture2D GetTexture(string name)
    {
        var directoryInfo = new DirectoryInfo(Path.Combine(Application.streamingAssetsPath, name));

        var allFiles = directoryInfo.GetFiles("*.tga");
        var file = allFiles.FirstOrDefault();
        Debug.Log(file.Name);
        var bytes = File.ReadAllBytes(file.FullName);
        var texture2d = new Texture2D(1, 1);
        texture2d.LoadImage(bytes);
        return texture2d;
    }
}