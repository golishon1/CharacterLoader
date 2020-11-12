using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustom : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRendererHair;
    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRendererClothes;

    public void SetHairColor(Color color)
    {
        _skinnedMeshRendererHair.material.color = color;
    }

    public void SetClothesColor(Color color)
    {
        for (int i = 0; i < _skinnedMeshRendererClothes.Length; i++)
        {
            _skinnedMeshRendererClothes[i].material.color = color;
        }
    }

    public void SetHairTexture(Texture2D texture2D)
    {
        _skinnedMeshRendererHair.material.mainTexture = texture2D;
    }
}
