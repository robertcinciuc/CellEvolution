using System.Collections.Generic;
using UnityEngine;


public class MeshGenerator : MonoBehaviour {
  
    private Mesh mesh;

    void Start(){
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
    }

    void Update() {
    }
    
    public void drawCylinder(Vector3[] vertices1, Vector3[] vertices2){
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        
        // Add vertices 1 from segm1, then 1 from segm2
        if(vertices1.Length != vertices2.Length){
            Debug.LogError("Slice vertices are of different sizes " + vertices1.Length + " " + vertices2.Length);    
        }
        
        List<Vector3> concatVertices = new List<Vector3>();
        // concatVertices.AddRange(vertices1);
        // concatVertices.AddRange(vertices2);
        // mesh.vertices = concatVertices.ToArray();
        concatVertices.add(vertices1[0]);
        concatVertices.add(vertices2[0]);
        concatVertices.add(vertices1[1]);

        // // Calculate normals for UVs
        // int nbPoints = vertices1.Length + vertices2.Length;
        // Vector2[] uv = new Vector2[nbPoints];
        // float increment = 1f / Mathf.Sqrt(nbPoints) + 1;
        // int vertexIndex = 0;
        // for (float i = 0; i < 1; i += increment) {
        //     for (float j = 0; j < 1; j += increment) {
        //         uv[vertexIndex] = new Vector2(i, j);
        //         vertexIndex++;
        //     }
        // }
        // mesh.uv = uv;

        // // Connect vertices into triangles
        // vertexIndex = 0;
        // int[] triangleIndices = new int[(2 * vertices1.Length - 2) * 3];
        // for (int i = 0; i < vertices1.Length; i+=3){
        //     triangleIndices[i] = vertexIndex;
        //     triangleIndices[i + 1] = vertices1.Length + vertexIndex;
        //     triangleIndices[i + 2] = vertexIndex;
        //     vertexIndex++;
        // }

        // vertexIndex = 0;
        // for(int i = vertices1.Length; i < vertices1.Length + vertices2.Length; i+=3){
        //     triangleIndices[i] = vertices1.Length + vertexIndex;
        //     triangleIndices[i + 1] = vertexIndex;
        //     triangleIndices[i + 2] = vertices1.Length + vertexIndex;
        //     vertexIndex++;
        // }
        // mesh.triangles = triangleIndices;
        
        mesh.triangles = new int[3] {0, 1,2};
    }
}
