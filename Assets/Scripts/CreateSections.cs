using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSections : MonoBehaviour
{
    public GameObject mesh;

    float cubeWidth;
    float cubeHeight;
    float cubeDepth;

    public float cubeScaleX;
    public float cubeScaleY;
    public float cubeScaleZ;

    private GameObject topple;

    private void Start()
    {
        // setting dimensions
        cubeWidth = transform.localScale.z;
        cubeHeight = transform.localScale.y;
        cubeDepth = transform.localScale.x;

        cubeScaleX = mesh.gameObject.transform.localScale.x;
        cubeScaleY = mesh.gameObject.transform.localScale.y;
        cubeScaleZ = mesh.gameObject.transform.localScale.z;
        // setting the size of the voxel cube to scale;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        mesh.gameObject.GetComponent<Transform>().localScale = new Vector3(cubeScaleX, cubeScaleY, cubeScaleZ);

        topple = GameObject.Find("Structure");
        CreateCube();
    }
    
    void CreateCube()
    {
        //this.gameObject.GetComponent<BoxCollider>().enabled = false;

        //this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        GameObject[] placeHolders = GameObject.FindGameObjectsWithTag("placeholder"); 
        for(int i = 0; i < placeHolders.Length; i++)
        {
            placeHolders[i].SetActive(false);
        }

        if (gameObject.CompareTag("Box"))
        {
            for (float x = 0; x < cubeWidth; x += cubeScaleX)
            {
                for (float y = 0; y < cubeHeight; y += cubeScaleY)
                {
                    for (float z = 0; z < cubeDepth; z += cubeScaleZ)
                    {
                        Vector3 vector = transform.position;
                        GameObject cubes = (GameObject)Instantiate(mesh, vector + new Vector3(x, y, z), Quaternion.identity);
                        cubes.gameObject.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
                        cubes.AddComponent<Split>();
                        cubes.transform.parent = topple.transform;
                        //cubes.AddComponent<CollapseDetection>();
                        cubes.AddComponent<FixedJoint>();
                        cubes.AddComponent<ArticulationBody>();
                    }
                }
            }
        }
    }
}
