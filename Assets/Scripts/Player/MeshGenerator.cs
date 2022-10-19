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
    
    public void drawCylinder(GameObject segment1, GameObject segment2){
        Vector3[] vertices1 = segment1.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] vertices2 = segment2.GetComponent<MeshFilter>().mesh.vertices;
        
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        
        if(vertices1.Length != vertices2.Length){
            Debug.LogError("Slice vertices are of different sizes " + vertices1.Length + " " + vertices2.Length);    
        }
        
        List<Vector3> concatVertices = new List<Vector3>();
        concatVertices.AddRange(vertices1);
        concatVertices.AddRange(vertices2);

        //Rotate vertices according to their slices
        for (int i = 0; i < concatVertices.Count; i++) {
            if (i < vertices1.Length) {
                concatVertices[i] = segment1.transform.TransformPoint(concatVertices[i]);
            } else {
                concatVertices[i] = segment2.transform.TransformPoint(concatVertices[i]);
            }
        }
         mesh.vertices = concatVertices.ToArray();

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

        // Connect vertices into triangles
        int vertexIndex = 0;
        int[] triangleIndices = new int[(2 * vertices1.Length - 2) * 3];
        for (int i = 0; i < vertices1.Length; i += 3) {
            triangleIndices[i] = vertexIndex;
            triangleIndices[i + 1] = vertices1.Length + vertexIndex;
            triangleIndices[i + 2] = vertexIndex + 1;
            vertexIndex++;
        }

        vertexIndex = 0;
        for (int i = vertices1.Length; i < vertices1.Length + vertices2.Length; i += 3) {
            triangleIndices[i] = vertices1.Length + vertexIndex;
            triangleIndices[i + 1] = vertexIndex;
            triangleIndices[i + 2] = vertices1.Length + vertexIndex + 1;
            vertexIndex++;
        }
        mesh.triangles = triangleIndices;
    }
}
