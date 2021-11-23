using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneRoomGenerator : MonoBehaviour
{
    // General room data
    private int xSize;
    private int zSize;
    private float roomHeight;

    // Floor Data
    [SerializeField]
    private Material floorMaterial;
    [SerializeField]
    private Gradient gradient;

    // Wall Data
    [SerializeField]
    private Color[] colorList;
    [SerializeField]
    private Material[] wallMaterial;
    private Color wallColor;

    // Column Data
    private GameObject[] columnList;
    [SerializeField]
    private Material columnMaterial;

    private enum OddEvenColumns { ODD, EVEN };
    private OddEvenColumns oddEvenColumns;

    private enum WhichWall { BACK, LEFT, FRONT, RIGHT};
    private WhichWall whichWall;

    // Throne Data
    [SerializeField]
    private Material throneMaterial;

    // Rug Data
    [SerializeField]
    private Material rugMaterial;

    // WallDecore Data
    [SerializeField]
    private Material[] wallDecorMaterial;

    // Decoration Data
    [SerializeField]
    private GameObject[] decorationPrefabs;

    [SerializeField]
    private GameObject[] swordPrefabs;
    [SerializeField]
    private Material swordBoxMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // generate common variables
        xSize = Random.Range(9, 20);
        zSize = Random.Range(9, 20);
        roomHeight = Random.Range(4, 6);

        // Object Generation
        CreateFloor();
        CreateWalls();
        CreateColumns();
        CreateThrone();
        CreateRug();

        CreateWallDecor();

        PlaceDecorations();

    }

    private void CreateFloor()
    {
        // General Shader Variables
        float minDensity = 3;
        float maxDensity = 5;
        float minAngleOffset = 5;
        float maxAngleOffset = 15;
        
        // Create floor object
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);

        floor.name = "Floor";

        floor.transform.position = new Vector3((float)xSize / 2, 0, (float)zSize / 2);
        floor.transform.localScale = new Vector3(xSize, zSize, 1);
        floor.transform.Rotate(90, 0, 0);


        MeshRenderer MR = floor.GetComponent<MeshRenderer>();
        MR.material = floorMaterial;
        MR.material.SetFloat("_density", Random.Range(minDensity, maxDensity));
        MR.material.SetFloat("_angleOffset", Random.Range(minAngleOffset, maxAngleOffset));
    }

    private void CreateWalls()
    {
        // General Shader Variables
        float minDensity = 1;
        float maxDensity = 4;
        float minAngleOffset = 1;
        float maxAngleOffset = 10;
        
        // Wall Data
        GameObject wallBack;
        GameObject wallRight;
        GameObject wallLeft;
        GameObject wallFront;

        // Create back wall
        wallBack = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wallBack.transform.position = new Vector3((float)xSize / 2, roomHeight / 2, zSize);
        wallBack.transform.localScale = new Vector3(xSize, roomHeight, 1);

        // Create front wall
        wallFront = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wallFront.transform.position = new Vector3((float)xSize / 2, roomHeight / 2, 0);
        wallFront.transform.localScale = new Vector3(xSize, roomHeight, 1);
        wallFront.transform.Rotate(0, -180, 0);

        // Create left wall
        wallLeft = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wallLeft.transform.position = new Vector3(0, roomHeight / 2, (float)zSize / 2);
        wallLeft.transform.localScale = new Vector3(zSize, roomHeight, 1);
        wallLeft.transform.Rotate(0, -90, 0);

        // Create right wall
        wallRight = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wallRight.transform.position = new Vector3(xSize, roomHeight / 2, (float)zSize / 2);
        wallRight.transform.localScale = new Vector3(zSize, roomHeight, 1);
        wallRight.transform.Rotate(0, 90, 0);

        GameObject[] wallGameObjects = new GameObject[4];
        wallGameObjects[0] = wallBack;
        wallGameObjects[1] = wallFront;
        wallGameObjects[2] = wallLeft;
        wallGameObjects[3] = wallRight;

        int selectedWallMaterial = Random.Range(0, wallMaterial.Length);
        wallColor = colorList[Random.Range(0, colorList.Length)];
        foreach (GameObject GO in wallGameObjects)
        {
            MeshRenderer MR = GO.GetComponent<MeshRenderer>();
            MR.material = wallMaterial[selectedWallMaterial];
            MR.material.SetFloat("_density", Random.Range(minDensity, maxDensity));
            MR.material.SetFloat("_angleOffset", Random.Range(minAngleOffset, maxAngleOffset));
            MR.material.SetColor("_color", wallColor);
        }
    }

    private void CreateColumns()
    {
        int columnCount = (int)Random.Range(0, (xSize * zSize) * 0.08f);
        if (columnCount == 1) { columnCount = 2; }
        Debug.Log("There are " + columnCount + " columns.");
        float columnWidth = Random.Range(0.5f, 1.1f);
        
        columnList = new GameObject[columnCount];
        Vector3[] positions = new Vector3[columnCount];

        // determine column positions
        if (columnCount % 2 == 0)
        {
            oddEvenColumns = OddEvenColumns.EVEN;

            float offset = Random.Range(0, ((float)xSize / columnCount) - columnWidth);
            for (int i = 0; i < positions.Length; i += 2)
            {
                positions[i] = new Vector3((float)xSize / columnCount - offset,
                                            roomHeight / 2,
                                            (float)zSize / columnCount * (i + 1));

                positions[i + 1] = new Vector3((float)xSize - ((float)xSize / columnCount) + offset,
                                                roomHeight / 2,
                                                (float)zSize / columnCount * (i + 1));
            }
        }
        else
        {
            oddEvenColumns = OddEvenColumns.ODD;

            float offset = Random.Range(0, Mathf.PI * 2f);
            float radius = ((xSize + zSize) / 2) / 3;
            for (int i = 0; i < positions.Length; i++)
            {
                float angle = (i * Mathf.PI * 2f / positions.Length) + offset;
                positions[i] = new Vector3((Mathf.Cos(angle) * radius) + ((float)xSize / 2), 
                                           roomHeight / 2, 
                                           (Mathf.Sin(angle) * radius) + ((float)zSize / 2));
            }
        }

        // Create the column gameobjects and set position
        Color columnColor;
        int selectedColor = Random.Range(0, 2);
        if(selectedColor == 0)
        {
            columnColor = colorList[Random.Range(0, colorList.Length)];
        }
        else
        {
            columnColor = wallColor;
        }
        for (int i = 0; i < columnList.Length; i++)
        {
            GameObject columnGameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            columnGameObject.transform.position = positions[i];
            columnGameObject.transform.localScale = new Vector3(columnWidth, roomHeight / 2, columnWidth);

            // set columnGameObject materials
            columnGameObject.GetComponent<MeshRenderer>().material = columnMaterial;
            columnGameObject.GetComponent<MeshRenderer>().material.color = columnColor;

            columnList[i] = columnGameObject;
        }
    }

    private void CreateThrone()
    {
        Color throneColor = RandomColor();
        
        Vector3 throneBasePosition;
        float throneBaseHeight = Random.Range(0.2f, 0.5f);
        float throneBaseXSize = Random.Range(0.8f, 1.5f);
        float throneBaseZSize = throneBaseXSize;

        Vector3 throneBackPosition;
        float throneBackHeight = Random.Range(1.5f, 3f);
        float throneBackXSize = throneBaseXSize;
        float throneBackZSize = Random.Range(0.2f, 0.5f);

        Vector3 throneRightArmPosition;
        float throneRightArmHeight = Random.Range(0.2f, 1f);
        float throneRightArmXSize = Random.Range(0.1f, 0.2f);
        float throneRightArmZSize = throneBaseZSize;

        Vector3 throneLeftArmPosition;
        float throneLeftArmHeight = throneRightArmHeight;
        float throneLeftArmXSize = throneRightArmXSize;
        float throneLeftArmZSize = throneRightArmZSize;


        if (oddEvenColumns == OddEvenColumns.EVEN)
        {
            throneBasePosition = new Vector3((float)xSize / 2, 
                                             throneBaseHeight / 2, 
                                             zSize - throneBaseZSize);

        }
        else if (oddEvenColumns == OddEvenColumns.ODD)
        {
            throneBasePosition = new Vector3((float)xSize / 2, 
                                             throneBaseHeight / 2, 
                                             (float)zSize / 2);

        }
        else
        {
            // Default, this shouldn't happen
            throneBasePosition = new Vector3(0, 0, 0);
        }

        // Throne Base
        GameObject throneBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        throneBase.name = "ThroneBase";
        throneBase.transform.position = throneBasePosition;
        throneBase.transform.localScale = new Vector3(throneBaseXSize, throneBaseHeight, throneBaseZSize);
        throneBase.GetComponent<MeshRenderer>().material = throneMaterial;
        throneBase.GetComponent<MeshRenderer>().material.color = throneColor;

        // Throne Back
        throneBackPosition = throneBasePosition;
        throneBackPosition.z += (throneBaseZSize - throneBackZSize) / 2;
        throneBackPosition.y += throneBaseHeight;

        GameObject throneBack = GameObject.CreatePrimitive(PrimitiveType.Cube);
        throneBack.name = "ThroneBack";
        throneBack.transform.position = throneBackPosition;
        throneBack.transform.localScale = new Vector3(throneBackXSize, throneBackHeight, throneBackZSize);
        throneBack.GetComponent<MeshRenderer>().material = throneMaterial;
        throneBack.GetComponent<MeshRenderer>().material.color = throneColor;

        // Throne Right Arm
        throneRightArmPosition = throneBasePosition;
        throneRightArmPosition.x -= (throneBaseXSize - throneRightArmXSize) / 2;
        throneRightArmPosition.y += throneBaseHeight;

        GameObject throneRightArm = GameObject.CreatePrimitive(PrimitiveType.Cube);
        throneRightArm.name = "ThroneRightArm";
        throneRightArm.transform.position = throneRightArmPosition;
        throneRightArm.transform.localScale = new Vector3(throneRightArmXSize, throneRightArmHeight, throneRightArmZSize);
        throneRightArm.GetComponent<MeshRenderer>().material = throneMaterial;
        throneRightArm.GetComponent<MeshRenderer>().material.color = throneColor;

        // Throne Left Arm
        throneLeftArmPosition = throneBasePosition;
        throneLeftArmPosition.x += (throneBaseXSize - throneLeftArmXSize) / 2;
        throneLeftArmPosition.y += throneBaseHeight;

        GameObject throneLeftArm = GameObject.CreatePrimitive(PrimitiveType.Cube);
        throneLeftArm.name = "ThroneLeftArm";
        throneLeftArm.transform.position = throneLeftArmPosition;
        throneLeftArm.transform.localScale = new Vector3(throneLeftArmXSize, throneLeftArmHeight, throneLeftArmZSize);
        throneLeftArm.GetComponent<MeshRenderer>().material = throneMaterial;
        throneLeftArm.GetComponent<MeshRenderer>().material.color = throneColor;

    }

    private void CreateRug()
    {
        Vector3 rugPosition;
        Vector3 rugScale;
        float rugWidth = Random.Range(0.5f, 4);
        float rugHeight = 0.1f;
        if (oddEvenColumns == OddEvenColumns.EVEN)
        {
            rugPosition = new Vector3((float)xSize / 2,
                                      0,
                                      (float)zSize / 2);

            rugScale = new Vector3(rugWidth, rugHeight, (float)zSize * 0.8f);

        }
        else if (oddEvenColumns == OddEvenColumns.ODD)
        {
            rugPosition = new Vector3((float)xSize / 2,
                                      0,
                                      (float)zSize / 2);

            rugScale = new Vector3(rugWidth * 2, rugHeight, rugWidth * 2);
        }
        else
        {
            // Default, this shouldn't happen
            rugPosition = new Vector3(0, 0, 0);
            rugScale = new Vector3(1, rugHeight, 1);
        }

        GameObject rug = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rug.name = "Rug";
        rug.transform.position = rugPosition;
        rug.transform.localScale = rugScale;
        rug.GetComponent<MeshRenderer>().material = rugMaterial;
        rug.GetComponent<MeshRenderer>().material.SetColor("_color", RandomColor());

    }

    private void CreateWallDecor()
    {
        int wallDecorAmount;
        Vector3[] wallDecorPositions;
        Vector3[] wallDecorRotations;
        GameObject[] wallDecor;

        float wallDecorOffset = 0.01f;
        float wallDecorHeight = 1;

        // Get position and rotation of the wall decor based on even odd column count
        // Even column count
        if (oddEvenColumns == OddEvenColumns.EVEN)
        {
            int caseSwitch = Random.Range(0, 2);
            if (columnList.Length == 0) { caseSwitch = 2; }

            switch (caseSwitch)
            {
                case 1:  // Place the wall decor on the side walls between the columns
                    wallDecorAmount = columnList.Length - 2;
                    wallDecorPositions = new Vector3[wallDecorAmount];
                    wallDecorRotations = new Vector3[wallDecorAmount];
                    wallDecor = new GameObject[wallDecorAmount];

                    int index = 1;
                    for (int i = 0; i < wallDecorAmount; i += 2)
                    {
                        wallDecorPositions[i] = new Vector3(0 + wallDecorOffset,
                                                            roomHeight / 2,
                                                            (float)zSize / ((float)(wallDecorAmount/2) + 1) * index);

                        wallDecorPositions[i + 1] = new Vector3(xSize - wallDecorOffset,
                                                                roomHeight / 2,
                                                                (float)zSize / ((float)(wallDecorAmount / 2) + 1) * index);

                        wallDecorRotations[i] = new Vector3(0, -90, 0);

                        wallDecorRotations[i + 1] = new Vector3(0, 90, 0);
                        index += 1;
                    }
                    break;

                case 2: // Randomly place the decor on the walls
                    wallDecorAmount = Random.Range(0, 6);
                    wallDecorPositions = new Vector3[wallDecorAmount];
                    wallDecorRotations = new Vector3[wallDecorAmount];
                    wallDecor = new GameObject[wallDecorAmount];

                    for (int i = 0; i < wallDecorAmount; i++)
                    {
                        whichWall = (WhichWall)Random.Range(0, 3);
                        float y = Random.Range(0 + (wallDecorHeight / 2), roomHeight - (wallDecorHeight / 2));


                        if (whichWall == WhichWall.BACK)
                        {
                            float x = Random.Range(0 + wallDecorHeight, xSize - wallDecorHeight);

                            wallDecorPositions[i] = new Vector3(x, y, zSize - wallDecorOffset);
                            wallDecorRotations[i] = new Vector3(0, 0, 0);
                        }
                        else if (whichWall == WhichWall.RIGHT)
                        {
                            float z = Random.Range(0, zSize);

                            wallDecorPositions[i] = new Vector3(xSize - wallDecorOffset, y, z);
                            wallDecorRotations[i] = new Vector3(0, 90, 0);
                        }
                        else if (whichWall == WhichWall.FRONT)
                        {
                            float x = Random.Range(0, xSize);

                            wallDecorPositions[i] = new Vector3(x, y, 0 + wallDecorOffset);
                            wallDecorRotations[i] = new Vector3(0, -180, 0);
                        }
                        else //(whichWall == WhichWall.LEFT)
                        {
                            float z = Random.Range(0, zSize);

                            wallDecorPositions[i] = new Vector3(0 + wallDecorOffset, y, z);
                            wallDecorRotations[i] = new Vector3(0, -90, 0);
                        }
                    }
                    break;

                default:
                    wallDecorAmount = 0;
                    wallDecorPositions = new Vector3[wallDecorAmount];
                    wallDecorRotations = new Vector3[wallDecorAmount];
                    wallDecor = new GameObject[wallDecorAmount];
                    break;
            }
        }
        // Odd column count
        else //(oddEvenColumns == OddEvenColumns.ODD)
        {
            wallDecorAmount = Random.Range(0, 6);
            wallDecorPositions = new Vector3[wallDecorAmount];
            wallDecorRotations = new Vector3[wallDecorAmount];
            wallDecor = new GameObject[wallDecorAmount];

            for (int i = 0; i < wallDecorAmount; i++)
            {
                whichWall = (WhichWall)Random.Range(0, 3);
                float y = Random.Range(0 + (wallDecorHeight / 2), roomHeight - (wallDecorHeight / 2));


                if (whichWall == WhichWall.BACK)
                {
                    float x = Random.Range(0 + wallDecorHeight, xSize - wallDecorHeight);

                    wallDecorPositions[i] = new Vector3(x, y, zSize - wallDecorOffset);
                    wallDecorRotations[i] = new Vector3(0, 0, 0);
                }
                else if (whichWall == WhichWall.RIGHT)
                {
                    float z = Random.Range(0, zSize);

                    wallDecorPositions[i] = new Vector3(xSize - wallDecorOffset, y, z);
                    wallDecorRotations[i] = new Vector3(0, 90, 0);
                }
                else if (whichWall == WhichWall.FRONT)
                {
                    float x = Random.Range(0, xSize);

                    wallDecorPositions[i] = new Vector3(x, y, 0 + wallDecorOffset);
                    wallDecorRotations[i] = new Vector3(0, -180, 0);
                }
                else //(whichWall == WhichWall.LEFT)
                {
                    float z = Random.Range(0, zSize);

                    wallDecorPositions[i] = new Vector3(0 + wallDecorOffset, y, z);
                    wallDecorRotations[i] = new Vector3(0, -90, 0);
                }
            }
        }
        
        Debug.Log("There are " + wallDecorAmount + " wall decor objects.");
        
        // Generate the decor
        if (wallDecorAmount > 0)
        {
            for (int i = 0; i < wallDecorAmount; i++)
            {
                int randomMaterial = Random.Range(0, wallDecorMaterial.Length);
                Vector2 randomVector = new Vector2(Random.Range(0, 50), Random.Range(0, 50));
                int randomBool = Random.Range(0, 2);

                wallDecor[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                wallDecor[i].transform.position = wallDecorPositions[i];
                wallDecor[i].transform.localScale = new Vector3(1, wallDecorHeight, 1);
                wallDecor[i].transform.Rotate(wallDecorRotations[i]);

                MeshRenderer MR = wallDecor[i].GetComponent<MeshRenderer>();
                MR.material = wallDecorMaterial[randomMaterial];
                if(MR.material.name == "CoatofArmsMaterial (Instance)")
                {
                    MR.material.SetColor("_checkerColor1", RandomColor());
                    MR.material.SetColor("_checkerColor2", RandomColor());
                    MR.material.SetColor("_shapeColor", colorList[Random.Range(0, colorList.Length)]);
                    MR.material.SetVector("_seed", randomVector);
                    MR.material.SetInt("_booleanSwitch", randomBool);
                }

            }
        }

    }
    
    private void PlaceDecorations()
    {
        // determine number of torches
        int numTorches = Random.Range(1, 6) * 2;

        // determine locations of torches
        Vector3[] positionsTorches = new Vector3[numTorches];
        for (int i = 0; i < numTorches; i += 2)
        {
            positionsTorches[i] = new Vector3((float)xSize / 3, 0.05f, (float)zSize / numTorches * (i+1));
            positionsTorches[i + 1] = new Vector3((float)xSize * 2 / 3, 0.05f, (float)zSize / numTorches * (i + 1));
            Instantiate(decorationPrefabs[0], positionsTorches[i], Quaternion.identity);
            Instantiate(decorationPrefabs[0], positionsTorches[i + 1], Quaternion.identity);
        }


        // determine number of barrels
        int numBarrels = Random.Range(0, 7);
        Debug.Log("There are " + numBarrels + " barrels.");

        // determine locations of barrels
        if (numBarrels > 0)
        {
            Vector3[] positionsBarrels = new Vector3[numBarrels];
            float offset = Random.Range(0, Mathf.PI * 2f);
            float radius = Random.Range(0.5f, 1);

            float border = xSize / 4;
            float firstBarrelPosX = Random.Range(xSize - border * 2, xSize - 1);
            if(firstBarrelPosX < xSize - border)
            {
                firstBarrelPosX -= (border * 2);
            }
            if(firstBarrelPosX < 2)
            {
                firstBarrelPosX += 2;
            }
            //Random.Range(xSize - 2, xSize - 1), 
            positionsBarrels[0] = new Vector3(firstBarrelPosX, 0.5f, Random.Range(2, zSize - 2));
            Instantiate(decorationPrefabs[1], positionsBarrels[0], Quaternion.identity);

            for (int i = 1; i < numBarrels; i++)
            {
                float angle = (i * Mathf.PI * 2f / (positionsBarrels.Length + Random.Range(0, 1))) + offset;
                positionsBarrels[i] = new Vector3((Mathf.Cos(angle) * radius) + positionsBarrels[0].x,
                                           0.5f,
                                           (Mathf.Sin(angle) * radius) + positionsBarrels[0].z);


                Instantiate(decorationPrefabs[1], positionsBarrels[i], Quaternion.identity);
            }
        }


        // Create Sword box display
        Vector3 positionSwordBox;
        float swordBoxHeight = Random.Range(0.2f, 0.5f);
        float swordBoxXSize = Random.Range(0.8f, 1.5f);
        float swordBoxZSize = swordBoxXSize;

        if (oddEvenColumns == OddEvenColumns.EVEN)
        {
            positionSwordBox = new Vector3((float)xSize / 2 + Random.Range(swordBoxXSize + 0.2f, swordBoxXSize + 2f),
                                     swordBoxHeight / 2,
                                     zSize - swordBoxZSize);

        }
        else //if (oddEvenColumns == OddEvenColumns.ODD)
        {
            positionSwordBox = new Vector3((float)xSize / 2 + Random.Range(swordBoxXSize + 0.2f, swordBoxXSize + 2f),
                                             swordBoxHeight / 2,
                                             (float)zSize / 2);

        }

        // sword box display
        GameObject throneBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        throneBase.name = "SwordBox";
        throneBase.transform.position = positionSwordBox;
        throneBase.transform.localScale = new Vector3(swordBoxXSize, swordBoxHeight, swordBoxZSize);
        throneBase.GetComponent<MeshRenderer>().material = swordBoxMaterial;
        throneBase.GetComponent<MeshRenderer>().material.SetColor("_color", RandomColor());

        // Determine number of swords
        int numSwords = 2;//Random.Range(1, 2);
        // determine locations of swords
        Vector3[] positionsSwords = new Vector3[numSwords];
        for (int i = 0; i < numSwords; i ++)
        {
            float xPos = (positionSwordBox.x - (swordBoxXSize / 2)) + ((float)swordBoxXSize * (i + 1) / (numSwords + 1));
            float yPos = positionSwordBox.y + 0.27f;
            float zPos = positionSwordBox.z;
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, 0, 90);

            positionsSwords[i] = new Vector3(xPos, yPos, zPos);
            Instantiate(swordPrefabs[Random.Range(0, swordPrefabs.Length)], positionsSwords[i], rotation);
        }

    }

    private Color RandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color randomcolor = new Color(r, g, b);
        return randomcolor;
    }
    
}
