using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WordCollectionData", menuName = "ScriptableObjects/WordCollectionScriptableObject", order = 1)]
public class WordCollectionScriptableObject : ScriptableObject
{
    public List<string> throneRoomNameWords = new List<string>();
    public List<string> throneRoomTypeWords = new List<string>();

}
