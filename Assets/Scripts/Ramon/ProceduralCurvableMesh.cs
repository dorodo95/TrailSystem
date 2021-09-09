using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ProceduralCurvableMesh : MonoBehaviour
{
    [SerializeField] float faceWidth = 1f;
    public AnimationCurve widthCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] float faceHeight = 1f;
    [Range (3,10)]
    [SerializeField] int numberOfFaces = 3;
    [Range (-1,1)]
    [SerializeField] float curveMesh = 0;
    int lastNumberOfFaces;
    float lastFaceHeight;
    float lastFaceWidth;
    
    void Awake ()
    {  
        lastNumberOfFaces = numberOfFaces;
        lastFaceHeight = faceHeight;
        lastFaceWidth = faceWidth;
        generateMesh ();
    }
    void generateMesh ()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Curvable Mesh";
        GetComponent<MeshFilter>().sharedMesh = mesh;
        mesh.Clear();

        Vector3 verticePositionRootForward;
        Vector3 verticePositionNextForward;
        Vector3 verticePositionRootLeft;
        Vector3 verticePositionNextLeft;

        float TAU = 6.28318530718f;
        
        List<Vector3> position = new List<Vector3>();
        for (int i = 0; i < numberOfFaces+1; i++) {

        float c = i /(float)numberOfFaces;
        
        float t = i /(float)numberOfFaces+1;
        float angRag = t * TAU;

        Vector3 dir = new Vector3(
            Mathf.Cos(angRag),
            0,
            Mathf.Sin(angRag) 
        );

        
        verticePositionRootLeft =  dir * faceWidth;
        verticePositionRootLeft.x -=  (faceWidth/2) + (faceHeight/2);
        verticePositionNextLeft =  dir * faceHeight;
        verticePositionNextLeft.x -=  (faceWidth/2) + (faceHeight/2);
        
        
        verticePositionRootForward = new Vector3(  widthCurve.Evaluate(c) *  faceWidth/2, 0, - faceHeight * i );
        verticePositionNextForward = new Vector3(  widthCurve.Evaluate(c) * -faceWidth/2, 0, - faceHeight * i );

        Vector3 verticePositionRootFinal = Vector3.Lerp(verticePositionRootForward,verticePositionRootLeft,curveMesh);
        Vector3 verticePositionNextFinal = Vector3.Lerp(verticePositionNextForward,verticePositionNextLeft,curveMesh);
        

        position.Add( verticePositionRootFinal );
        position.Add( verticePositionNextFinal );
            
        };


        List<int> faceIndices = new List<int>();
        for (int i = 0; i < numberOfFaces; i++) {
        
        int rootIndex = i * 2;
        int indexInnerRoot = rootIndex + 1;
        int indexOuterNext = rootIndex + 2;
        int indexInnerNext = rootIndex + 3;

        faceIndices.Add( rootIndex );
        faceIndices.Add( indexOuterNext );
        faceIndices.Add( indexInnerNext );

        faceIndices.Add( rootIndex );
        faceIndices.Add( indexInnerNext );
        faceIndices.Add( indexInnerRoot );

        }

        
        mesh.SetVertices ( position );
        mesh.SetTriangles ( faceIndices, 0 );

    }
    void Update ()
    {
        generateMesh ();
        /*if(lastNumberOfFaces != numberOfFaces || lastFaceHeight != faceHeight || lastFaceWidth != faceWidth ){
            
            lastNumberOfFaces = numberOfFaces;
            lastFaceHeight = faceHeight;
            lastFaceWidth = faceWidth;  
            generateMesh ();
            

        };*/
        
    
    

    }


}
