using System.Collections.Generic;
using UnityEngine;

public class CharactersPlayer : MonoBehaviour
{
    [SerializeField] private GameObject baseButton;
    [SerializeField] private GameObject rootUI;
    [SerializeField] private GameObject rootCharacter;

    [SerializeField] private List<GameObject> characters;

    [SerializeField] private Texture2D _texture2D;
    private CharacterLoader loader;

    private Vector3 offset = new Vector3(2, 0, 0);

    private void Start()
    {
        loader = Resources.Load<CharacterLoader>("CharacterLoader");
        var names = loader.Characters;
        foreach (var objName in names)
        {
            var btn = Instantiate(baseButton, rootUI.transform);
            btn.GetComponent<CharactersButton>().Setup(objName, OnSpawnCharacters,
                OnColorChange, OnTextureHairChange);
        }
    }

    private void OnSpawnCharacters(string id)
    {
        var asset = loader.GetCharacter(id);
        var obj = Instantiate(asset, offset, Quaternion.identity);
        obj.name = id;
        characters.Add(obj);
        offset += offset;
        _texture2D = loader.GetTexture(id);
    }

    private void OnColorChange(string id, float value)
    {
        var f = characters.Find(x => x.name == id);
        if (f != null)
        {
            var custom = f.gameObject.GetComponent<CharacterCustom>();
            custom.SetClothesColor(new Color(value, value, value, 1f));
        }
    }

    private void OnTextureHairChange(string id)
    {
        var f = characters.Find(x => x.name == id);
        if (f != null)
        {
            var custom = f.gameObject.GetComponent<CharacterCustom>();
            custom.SetHairTexture(loader.GetTexture(id));
        }
    }
}