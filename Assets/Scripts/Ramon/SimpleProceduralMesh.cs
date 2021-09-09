using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleProceduralMesh : MonoBehaviour
{

    //public Vector3[] positions;
    List<Vector3> finalPos;
    Mesh mesh;
    public Transform[] positions;
    public int[] faces;

    void Awake ()
    {
        mesh = new Mesh();
        mesh.name = "Procedural Mesh";
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
    void SetVertices ()
    {
        finalPos = new List<Vector3>();
        for(var i = 0; i < positions.Length; i++){
            finalPos.Add(positions[i].position);
        }
    }
    void Update ()
    {
        mesh.Clear();
        
        if(finalPos.Count != positions.Length)
        {
          SetVertices ();
        }
        
 
        for(var i = 0; i < positions.Length; i++){
           
           finalPos[i] = positions[i].position;
           
        }

        mesh.SetVertices ( finalPos );
        mesh.triangles = faces;
    

    }


}
