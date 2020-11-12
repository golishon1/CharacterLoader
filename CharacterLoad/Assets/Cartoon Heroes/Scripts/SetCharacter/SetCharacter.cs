﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace CartoonHeroes
{
    public class SetCharacter : MonoBehaviour
    {
        private const string namePrefix = "Set Character_";
        private const string hideString = "(Hide)";

        public Transform characterRoot;
        public ItemGroup[] itemGroups;

        public GameObject disabledGraySkeleton;

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }


        public GameObject AddItem(ItemGroup itemGroup, int itemSlot)
        {
            var item = itemGroup.items[itemSlot];
            var itemInstance = Instantiate(item.prefab);
            itemInstance.name = itemInstance.name.Substring(0, itemInstance.name.Length - "(Clone)".Length);
            RemoveAnimator(itemInstance);
            ParentObjectAndBones(itemInstance);

            SetGraySkeletonVisibility(!VisibleItems());

            return itemInstance;
        }

        public bool VisibleItems()
        {
            for (var i = 0; i < itemGroups.Length; i++)
            for (var n = 0; n < itemGroups[i].items.Length; n++)
                if (HasItem(itemGroups[i], n))
                    return true;

            return false;
        }

        public void SetGraySkeletonVisibility(bool set)
        {
            if (!set)
            {
                var allCharacterChildren = GetAllCharacterChildren();
                for (var i = 0; i < allCharacterChildren.Length; i++)
                    if (allCharacterChildren[i].name.Contains(hideString))
                    {
                        disabledGraySkeleton = allCharacterChildren[i].gameObject;
                        allCharacterChildren[i].gameObject.SetActive(false);
                        break;
                    }
            }
            else
            {
                if (disabledGraySkeleton != null) disabledGraySkeleton.SetActive(true);
            }
        }

        public bool HasItem(ItemGroup itemGroup, int itemSlot)
        {
            if (itemGroup.items[itemSlot] != null && itemGroup.items[itemSlot].prefab != null)
            {
                var root = GetRoot();
                var prefab = itemGroup.items[itemSlot].prefab.transform;
                for (var i = 0; i < root.childCount; i++)
                {
                    var child = root.GetChild(i);
                    if (child.name.Contains(prefab.name) && child.name.Contains(namePrefix)) return true;
                }
            }

            return false;
        }

        public void ParentObjectAndBones(GameObject itemInstance)
        {
            var allCharacterChildren = GetAllCharacterChildren();
            var allItemChildren = itemInstance.GetComponentsInChildren<Transform>();
            itemInstance.transform.position = transform.position;
            itemInstance.transform.parent = transform;

            var allItemChildren_NewNames = new string[allItemChildren.Length];

            for (var i = 0; i < allItemChildren.Length; i++)
            {
                //Match and parent bones
                for (var n = 0; n < allCharacterChildren.Length; n++)
                    if (allItemChildren[i].name == allCharacterChildren[n].name)
                    {
                        MatchTransform(allItemChildren[i], allCharacterChildren[n]);
                        allItemChildren[i].parent = allCharacterChildren[n];
                    }

                //Rename
                allItemChildren_NewNames[i] = allItemChildren[i].name;

                if (!allItemChildren[i].name.Contains(namePrefix))
                    allItemChildren_NewNames[i] = namePrefix + allItemChildren[i].name;

                if (!allItemChildren[i].name.Contains(itemInstance.name))
                    allItemChildren_NewNames[i] += "_" + itemInstance.name;
            }

            for (var i = 0; i < allItemChildren.Length; i++) allItemChildren[i].name = allItemChildren_NewNames[i];
        }

        public Transform GetRoot()
        {
            Transform root;
            if (characterRoot == null)
                root = transform;
            else
                root = characterRoot;
            return root;
        }

        public Transform[] GetAllCharacterChildren()
        {
            var root = GetRoot();
            var allCharacterChildren = root.GetComponentsInChildren<Transform>();

            /*List<Transform> allCharacterChildren_List = new List<Transform>();
            
            for(int i = 0; i < allCharacterChildren.Length; i++){
                if(allCharacterChildren[i].GetComponent<SkinnedMeshRenderer>() != null || allCharacterChildren[i].GetComponent<Animator>() != null)
                {
                    continue;
                }
                allCharacterChildren_List.Add(allCharacterChildren[i]);
            }

            allCharacterChildren = allCharacterChildren_List.ToArray();*/

            return allCharacterChildren;
        }

        public bool BelongsToItem(Transform obj, ItemGroup itemGroup, int itemSlot)
        {
            if (obj == null || itemGroup.items[itemSlot].prefab == null) return false;
            return obj.name.Contains(namePrefix) && obj.name.Contains(itemGroup.items[itemSlot].prefab.name);
        }

        public void RemoveAnimator(GameObject item)
        {
            var animator = item.GetComponent<Animator>();
            if (animator != null) DestroyImmediate(animator);
        }

        public void MatchTransform(Transform obj, Transform target)
        {
            obj.position = target.position;
            obj.rotation = target.rotation;
        }

        public List<GameObject> GetRemoveObjList(ItemGroup itemGroup, int itemSlot)
        {
            var allChildren = GetAllCharacterChildren();

            var removeList = new List<GameObject>();

            for (var i = 0; i < allChildren.Length; i++)
                if (BelongsToItem(allChildren[i], itemGroup, itemSlot))
                    //DestroyImmediate(allChildren[i].gameObject);
                    removeList.Add(allChildren[i].gameObject);

            SetGraySkeletonVisibility(!VisibleItems());
            return removeList;
        }

        [Serializable]
        public class ItemGroup
        {
            public string name;
            public Item[] items;
            public int slots;
        }

        [Serializable]
        public class Item
        {
            public GameObject prefab;
        }
    }
}