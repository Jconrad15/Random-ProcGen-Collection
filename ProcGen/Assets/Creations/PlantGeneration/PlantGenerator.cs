using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform[] gameObjects;

    // G = (V = variables, w = axiom, P = rules)
    public enum Symbols { A, B, C, D };

    private List<Symbols> currentVariables = new List<Symbols>();
    private List<Symbols> nextVariables = new List<Symbols>();

    private List<Symbols> axiom = new List<Symbols>();

    private List<Transform> gameObjectPositions = new List<Transform>();

    private bool isReadytoProcessNextN = false;

    private int nLevel = 0;

    private int timer = 0;
    private int timeLimit = 45;

    // Start is called before the first frame update
    void Start()
    {
        axiom.Add(Symbols.A);

        // The first set of variables is the axiom
        currentVariables = axiom;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer >= timeLimit)
        {
            if (isReadytoProcessNextN == true)
            {
                ProcessStructureRules();
            }
            else//(isUpdated == false)
            {
                UpdateGraphics();
            }
            timer = 0;
        }
        timer += 1;
    }

    private void UpdateGraphics()
    {
        int numInstantiated = 0;
        Vector3 editNextPosition = new Vector3(0, 0, 0);
        Quaternion editNextRotation = new Quaternion(1, 1, 1, 1);

        bool isNextToBeEdited = false;

        // Create new gameobjects
        for (int i = 0; i < currentVariables.Count; i++)
        {
            Vector3 position;
            Quaternion rotation;

            if (currentVariables[i] == Symbols.A)
            {
                RuleA.DrawingRule(numInstantiated, nLevel, out position, out rotation);
            }
            else if (currentVariables[i] == Symbols.B)
            {
                RuleB.DrawingRule(numInstantiated, nLevel, out position, out rotation);
            }
            else if (currentVariables[i] == Symbols.C)
            {
                RuleC.DrawingRule(numInstantiated, nLevel, out position, out rotation);
            }
            else if (currentVariables[i] == Symbols.D)
            {
                RuleD.DrawingRule(numInstantiated, nLevel, out position, out rotation);
                RuleD.EditNextRule(numInstantiated, nLevel, out editNextPosition, out editNextRotation);
                isNextToBeEdited = true;

                Debug.Log("The next is to be edited. The index is: " + i);
            }
            else // this shouldn't happen
            {
                position = new Vector3(0, 0, 0);
                rotation = new Quaternion(1, 1, 1, 1);
            }

            // Instantiate the gameObject if the drawing rule does not return 999 as position
            if (position.x != 999)
            {
                if (isNextToBeEdited == false)
                {
                    Transform prefab = gameObjects[(int)currentVariables[i]];

                    gameObjectPositions.Add(Instantiate(prefab, position, rotation));
                    numInstantiated += 1;
                }
                else //isNextToBeEdited == true
                {
                    Transform prefab = gameObjects[(int)currentVariables[i]];

                    gameObjectPositions.Add(Instantiate(prefab, position + editNextPosition, rotation * editNextRotation));
                    numInstantiated += 1;

                    isNextToBeEdited = false;
                    editNextPosition = new Vector3(0, 0, 0);
                    editNextRotation = new Quaternion(1, 1, 1, 1);
                }
            }

        }
        isReadytoProcessNextN = true;
    }

    private void ProcessStructureRules()
    {
        nextVariables.Clear();

        // Determine the next variables for each current variable
        for (int i = 0; i < currentVariables.Count; i++)
        {
            // Rule A
            if (currentVariables[i] == Symbols.A)
            {
                List<Symbols> outVariables = new List<Symbols>();
                RuleA.GetNextN(out outVariables);
                foreach (Symbols symbol in outVariables)
                {
                    nextVariables.Add(symbol);
                }
            }
            // Rule B
            else if (currentVariables[i] == Symbols.B)
            {
                List<Symbols> outVariables = new List<Symbols>();
                RuleB.GetNextN(out outVariables);
                foreach (Symbols symbol in outVariables)
                {
                    nextVariables.Add(symbol);
                }
            }
            // Rule C
            else if (currentVariables[i] == Symbols.C)
            {
                List<Symbols> outVariables = new List<Symbols>();
                RuleC.GetNextN(out outVariables);
                foreach (Symbols symbol in outVariables)
                {
                    nextVariables.Add(symbol);
                }
            }
            // 
            else if (currentVariables[i] == Symbols.D)
            {
                List<Symbols> outVariables = new List<Symbols>();
                RuleD.GetNextN(out outVariables);
                foreach (Symbols symbol in outVariables)
                {
                    nextVariables.Add(symbol);
                }
            }
        }

        // clear the current variables and replace them with the next variables
        currentVariables.Clear();
        foreach (var v in nextVariables) { currentVariables.Add(v); }

        nLevel += 1;
        isReadytoProcessNextN = false;
    }

    public class RuleA
    {
        public static void GetNextN(out List<Symbols> outVariables)
        {
            List<Symbols> nV = new List<Symbols>();
            nV.Add(Symbols.A);
            nV.Add(Symbols.B);

            outVariables = nV;
        }

        public static void DrawingRule(int index, int nLevel, out Vector3 outPosition, out Quaternion outRotation)
        {
            Vector3 position;
            Quaternion rotation;
            position = new Vector3(index, nLevel, 0);
            rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

            outPosition = position;
            outRotation = rotation;
        }
    }

    public class RuleB
    {
        public static void GetNextN(out List<Symbols> outVariables)
        {
            List<Symbols> nV = new List<Symbols>();
            nV.Add(Symbols.C);
            nV.Add(Symbols.A);

            outVariables = nV;
        }

        public static void DrawingRule(int index, int nLevel, out Vector3 outPosition, out Quaternion outRotation)
        {
            Vector3 position;
            Quaternion rotation;
            position = new Vector3(index, nLevel, 0);
            rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);

            outPosition = position;
            outRotation = rotation;
        }
    }

    public class RuleC
    {
        public static void GetNextN(out List<Symbols> outVariables)
        {
            List<Symbols> nV = new List<Symbols>();
            nV.Add(Symbols.B);
            nV.Add(Symbols.D);

            outVariables = nV;
        }

        public static void DrawingRule(int index, int nLevel, out Vector3 outPosition, out Quaternion outRotation)
        {
            Vector3 position;
            Quaternion rotation;
            position = new Vector3(index, nLevel, 0);
            rotation = Quaternion.LookRotation((Vector3.up + Vector3.right).normalized);

            outPosition = position;
            outRotation = rotation;
        }
    }

    public class RuleD
    {
        public static void GetNextN(out List<Symbols> outVariables)
        {
            // D is a constant

            // return empty list
            List<Symbols> nV = new List<Symbols>();
            outVariables = nV;
        }

        public static void DrawingRule(int index, int nLevel, out Vector3 outPosition, out Quaternion outRotation)
        {
            // This drawing rule isn't used.  
            outPosition = new Vector3(999, 999, 999);
            outRotation = new Quaternion(999, 999, 999, 999);
        }

        public static void EditNextRule(int index, int nLevel, out Vector3 outEditPosition, out Quaternion outEditRotation)
        {
            outEditPosition = new Vector3(-1, 0.5f, 0);
            outEditRotation = Quaternion.LookRotation((Vector3.up + Vector3.left).normalized);
        }

    }



}
