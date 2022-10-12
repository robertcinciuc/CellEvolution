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
  
}
