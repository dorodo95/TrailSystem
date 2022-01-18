using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]
public class ProceduralCurvableMesh : MonoBehaviour
{
    [SerializeField] float faceWidth = 1f;
    public AnimationCurve widthCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] float faceHeight = 1f;
    [Range (3,32)]
    [SerializeField] int numberOfFaces = 3;
    [Range (0,1)]
    [SerializeField] float curveMesh = 0;
    int lastNumberOfFaces;
    float lastFaceHeight;
    float lastFaceWidth;

    public enum MyComponentType {
        FilterMesh, 
        Particle
        };
    [SerializeField] MyComponentType myComponentType = MyComponentType.FilterMesh;
    
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

        
        

        switch (myComponentType)
        {
            case MyComponentType.Particle:

            List<Vector4> particleValue = new List<Vector4>();
        for (int i = 0; i <  GetComponent<ParticleSystem>().particleCount; i++)
        {
            GetComponent<ParticleSystem>().GetCustomParticleData(particleValue,ParticleSystemCustomData.Custom1);
        }
            GetComponent<ParticleSystemRenderer>().mesh = mesh;
            

            break;

            case MyComponentType.FilterMesh:
            GetComponent<MeshFilter>().sharedMesh = mesh;
            break;

        }
        

        mesh.Clear();

        Vector3 verticePositionRootForward;
        Vector3 verticePositionNextForward;
        Vector3 verticePositionRootLeft;
        Vector3 verticePositionNextLeft;

        float TAU = 6.28318530718f * curveMesh;
        
        List<Vector3> position = new List<Vector3>();
        for (int i = 0; i < numberOfFaces + 1  ; i++) {

        float t = i /(float)numberOfFaces;
        float angRag = t * TAU;

        Vector3 dir = new Vector3(
            Mathf.Cos(angRag),
            0,
            Mathf.Sin(angRag)
            
        );

      float x = dir.x, y = dir.y, z = dir.z;
	  float sint, cost;


	    sint = Mathf.Sin(angRag);
	    cost = Mathf.Cos(angRag);
	
        /*
        dir.x = -(y - 1.0f / curveMesh) * sint;
        dir.y = y * cost + (1.0f - cost) / curveMesh;
        dir.z = z;
        
        dir.x += cost * faceHeight;
        dir.y += sint * faceHeight;
        dir.z += faceWidth;
        

        /*
        dir.x = x * cost + (1.0f - cost) / curveMesh;
	    dir.x = y;
	    dir.y = -(x - 1.0f / curveMesh) * sint;

	      
	    dir.x += sint * faceHeight;
	    dir.y += faceWidth;
	    dir.z += cost * faceHeight;
        */

        /*
        Proportional
        verticePositionRootLeft =  dir * (faceWidth/2);
        verticePositionNextLeft =  dir * faceWidth;
        */

        //Inversely Proportional
        float faceWidthPlusAnimationCurve = (faceWidth * widthCurve.Evaluate(t)) + 1;

        verticePositionRootLeft =  dir / faceWidthPlusAnimationCurve;
        verticePositionNextLeft =  dir * (faceWidthPlusAnimationCurve);
        verticePositionRootLeft.x -= ((1/faceWidthPlusAnimationCurve) + faceWidthPlusAnimationCurve)/2;
        verticePositionNextLeft.x -= ((1/faceWidthPlusAnimationCurve) + faceWidthPlusAnimationCurve)/2;

        //verticePositionRootLeft.z += (faceHeight * i) * Mathf.Lerp(1,0, curveMesh);
        //verticePositionNextLeft.z += (faceHeight * i) * Mathf.Lerp(1,0, curveMesh);
        
        verticePositionRootLeft *= -1;
        verticePositionNextLeft *= -1;
        
        
        verticePositionRootForward = new Vector3(  widthCurve.Evaluate(t) *   faceWidth/2, 0, - faceHeight * i );
        verticePositionNextForward = new Vector3(  widthCurve.Evaluate(t) * - faceWidth/2, 0, - faceHeight * i );

        Vector3 verticePositionRootFinal = Vector3.Lerp(verticePositionRootForward,verticePositionRootLeft, curveMesh);
        Vector3 verticePositionNextFinal = Vector3.Lerp(verticePositionNextForward,verticePositionNextLeft, curveMesh);



        position.Add( verticePositionRootLeft );
        position.Add( verticePositionNextLeft );
            
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
