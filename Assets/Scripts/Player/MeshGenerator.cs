using UnityEngine;


public class MeshGenerator : MonoBehaviour {
  
    private Mesh mesh;

    void Start(){
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
	  
        mesh.Clear();
    
        mesh.vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0)};
        mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        mesh.triangles = new int[] {0, 1, 2};
    }

    void Update() {
        Vector3[] oldVertices = mesh.vertices;
        Vector3[] newVertices = new Vector3[] { oldVertices[0] * 1.001f, oldVertices[1] * 1.001f, oldVertices[2] * 1.001f };
        mesh.vertices = newVertices;
    }
    
    public void drawCylinder(Vector3[] vertices1, Vector3[] vertices2){
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        
        // Add vertices 1 from segm1, then 1 from segm2
        if(vertices1.Length != vertices2.Length){
            Debug.LogError("Slice vertices are of different sizes " + vertices1.Length + " " + vertices2.Length);    
        }
        List<int> concatVertices = new List<int>();
        concatVertices.AddRange(vertices1);
        concatVertices.AddRange(vertices2);
        mesh.vertices = concatVertices.toArray();
        
        // Calculate normals for UVs
        // mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1)};
        
        // Connect vertices into triangles
        int[] triangleIndices = new int[(2 * vertices1.Length - 2) * 3];
        for(int i = 0; i < vertices1.Length; i+=3){
            triangleIndices[i] = concatVertices[i];
            triangleIndices[i+1] = concatVertices[i + vertices1.Length - 1];
            // triangleIndices[i+2]
        }
        // mesh.triangles = new int[] {0, 1, 2};
    }
}
