using UnityEditor;
using UnityEngine;

namespace CartoonHeroes
{
    [CustomEditor(typeof(SetCharacter))]
    [CanEditMultipleObjects]
    public class SetCharacterEditor : Editor
    {
        private const int defaultSpace = 8;

        public override void OnInspectorGUI()
        {
            var setCharacter = (SetCharacter) target;

            serializedObject.Update();

            GUILayout.Space(defaultSpace);

            setCharacter.characterRoot =
                EditorGUILayout.ObjectField("Character Root", setCharacter.characterRoot, typeof(Transform), true) as
                    Transform;

            GUILayout.Space(defaultSpace);

            if (GUILayout.Button("Add Item Group"))
            {
                if (setCharacter.itemGroups == null)
                {
                    setCharacter.itemGroups = new SetCharacter.ItemGroup[1];
                }
                else
                {
                    var itemGroups_Temp = new SetCharacter.ItemGroup[setCharacter.itemGroups.Length];
                    itemGroups_Temp = setCharacter.itemGroups.Clone() as SetCharacter.ItemGroup[];

                    setCharacter.itemGroups = new SetCharacter.ItemGroup[setCharacter.itemGroups.Length + 1];
                    for (var i = 0; i < itemGroups_Temp.Length; i++) setCharacter.itemGroups[i] = itemGroups_Temp[i];
                }

                var newItemGroup = setCharacter.itemGroups[setCharacter.itemGroups.Length - 1] =
                    new SetCharacter.ItemGroup();
                newItemGroup.name = "New Item Group " + (setCharacter.itemGroups.Length - 1);
                newItemGroup.items = new SetCharacter.Item[1];
                newItemGroup.slots = 1;
            }

            if (setCharacter.itemGroups != null && setCharacter.itemGroups.Length > 0)
            {
                GUILayout.Space(defaultSpace);

                for (var i = 0; i < setCharacter.itemGroups.Length; i++)
                {
                    //GUILayout.Label(itemGroups[i].name);
                    setCharacter.itemGroups[i].name =
                        EditorGUILayout.TextField("Group Name: ", setCharacter.itemGroups[i].name);

                    setCharacter.itemGroups[i].slots =
                        EditorGUILayout.IntField("Slots", setCharacter.itemGroups[i].slots);

                    setCharacter.itemGroups[i].slots = Mathf.Clamp(setCharacter.itemGroups[i].slots, 1, 100);

                    if (setCharacter.itemGroups[i].slots != setCharacter.itemGroups[i].items.Length)
                    {
                        var items_Temp = new SetCharacter.Item[setCharacter.itemGroups[i].items.Length];
                        items_Temp = setCharacter.itemGroups[i].items.Clone() as SetCharacter.Item[];
                        setCharacter.itemGroups[i].items = new SetCharacter.Item[setCharacter.itemGroups[i].slots];
                        for (var n = 0; n < setCharacter.itemGroups[i].items.Length && n < items_Temp.Length; n++)
                            setCharacter.itemGroups[i].items[n] = items_Temp[n];
                    }

                    for (var n = 0; n < setCharacter.itemGroups[i].items.Length; n++)
                    {
                        EditorGUILayout.BeginHorizontal();

                        if (!setCharacter.HasItem(setCharacter.itemGroups[i], n))
                        {
                            if (GUILayout.Button("Add Item"))
                            {
                                var addedObj = setCharacter.AddItem(setCharacter.itemGroups[i], n);
                                Undo.RegisterCreatedObjectUndo(addedObj, "Added Item");
                            }
                        }
                        else
                        {
                            if (GUILayout.Button("Remove Item"))
                            {
                                var removedObjs = setCharacter.GetRemoveObjList(setCharacter.itemGroups[i], n);
                                Undo.SetCurrentGroupName("Removed Item");
                                for (var m = 0; m < removedObjs.Count; m++)
                                    if (removedObjs[m] != null)
                                        Undo.DestroyObjectImmediate(removedObjs[m]);
                                Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
                            }
                        }

                        if (setCharacter.itemGroups[i] != null && setCharacter.itemGroups[i].items[n] != null)
                            setCharacter.itemGroups[i].items[n].prefab =
                                EditorGUILayout.ObjectField(setCharacter.itemGroups[i].items[n].prefab,
                                    typeof(GameObject), true) as GameObject;

                        EditorGUILayout.EndHorizontal();
                    }

                    if (GUILayout.Button("Delete Group: " + setCharacter.itemGroups[i].name))
                    {
                        var itemGroups_Temp = new SetCharacter.ItemGroup[setCharacter.itemGroups.Length - 1];
                        for (var n = 0; n < i; n++) itemGroups_Temp[n] = setCharacter.itemGroups[n];
                        for (var n = i + 1; n < setCharacter.itemGroups.Length; n++)
                            itemGroups_Temp[n - 1] = setCharacter.itemGroups[n];
                        setCharacter.itemGroups = itemGroups_Temp;
                    }

                    GUILayout.Space(defaultSpace);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}