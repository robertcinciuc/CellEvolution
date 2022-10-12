using UnityEngine;

public class MeshGenerator : MonoBehaviour{
  
  void Start(){
    gameObject.AddComponent<MeshFilter>();
    gameObject.AddComponent<MeshRenderer>();
    Mesh mesh = GetComponent<MeshFilter>().mesh();
	  
	  mesh.Clear();
    
    mesh.vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0)};
    mesh.uv = new Vector2[] {new Vector(0, 0), new Vector(0, 1), new Vector(1, 1)};
    mesh.triangles = new int[] {0, 1, 2};
  }
  
}