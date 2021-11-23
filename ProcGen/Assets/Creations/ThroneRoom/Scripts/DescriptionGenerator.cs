using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptionGenerator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameField;
    string throneRoomName;

    [SerializeField]
    private WordCollectionScriptableObject wordCollection;

    // Start is called before the first frame update
    public void Start()
    {
        nameField.SetText(GetThroneRoomName());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("n pressed");
            nameField.SetText(GetThroneRoomName());
        }
    }

    private string GetThroneRoomName()
    {
        throneRoomName = null;
        
        int number = Random.Range(2, 4);
        string[] nameParts = new string[number];

        if (number == 2)
        {
            nameParts[0] = wordCollection.throneRoomNameWords[Random.Range(0, wordCollection.throneRoomNameWords.Count)] + " ";
            nameParts[1] = wordCollection.throneRoomTypeWords[Random.Range(0, wordCollection.throneRoomTypeWords.Count)];
        }
        else // number == 3
        {
            nameParts[0] = "The ";
            nameParts[1] = wordCollection.throneRoomNameWords[Random.Range(0, wordCollection.throneRoomNameWords.Count)] + " ";
            nameParts[2] = wordCollection.throneRoomTypeWords[Random.Range(0, wordCollection.throneRoomTypeWords.Count)];
        }

        for (int i = 0; i < nameParts.Length; i++)
        {
            throneRoomName += nameParts[i];
        }


        return throneRoomName;
    }


}
