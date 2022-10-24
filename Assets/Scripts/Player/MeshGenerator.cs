using System.Collections.Generic;
using UnityEngine;


public class MeshGenerator : MonoBehaviour {
    
    public Texture mainTexture;
    
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    void Start(){
        gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
        
        meshRenderer.material.EnableKeyword ("_NORMALMAP");
        meshRenderer.material.EnableKeyword ("_METALLICGLOSSMAP");
        // TODO: verify whether texture needs to be applied on each update
        meshRenderer.material.SetTexture("_MainTex", mainTexture);
        
        mesh.Clear();
    }

    void Update() {
    }
    
    public void drawCylinder(GameObject segment1, GameObject segment2){
        Vector3[] vertices1 = segment1.GetComponent<MeshFilter>().mesh.vertices;
        Vector3[] vertices2 = segment2.GetComponent<MeshFilter>().mesh.vertices;

        //Get ordered vertices
        Vector3[] orderedVertices1 = getVerticesInOrder(vertices1);
        Vector3[] orderedVertices2 = getVerticesInOrder(vertices2);
        
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        
        if(orderedVertices1.Length != orderedVertices2.Length){
            Debug.LogError("Slice vertices are of different sizes " + orderedVertices1.Length + " " + orderedVertices2.Length);    
        }
        
        List<Vector3> concatVertices = new List<Vector3>();
        concatVertices.AddRange(orderedVertices1);
        concatVertices.AddRange(orderedVertices2);

        //Rotate vertices according to their slices
        for (int i = 0; i < concatVertices.Count; i++) {
            if (i < orderedVertices1.Length) {
                concatVertices[i] = segment1.transform.TransformPoint(concatVertices[i]);
            } else {
                concatVertices[i] = segment2.transform.TransformPoint(concatVertices[i]);
            }
        }
         mesh.vertices = concatVertices.ToArray();

        // // Calculate normals for UVs
        // int nbPoints = orderedVertices1.Length + orderedVertices2.Length;
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

        // Connect vertices into triangles with base on slice 1
        int vertexIndex = 0;
        int[] triangleIndices = new int[(2 * orderedVertices1.Length - 2) * 3];
        for (int i = 0; i < orderedVertices1.Length; i += 3) {
            triangleIndices[i] = vertexIndex;
            triangleIndices[i + 1] = orderedVertices1.Length + vertexIndex;
            triangleIndices[i + 2] = vertexIndex + 1;
            vertexIndex++;
        }

        //Connect vertices into triangles with base on slice 2
        vertexIndex = 0;
        for (int i = orderedVertices1.Length; i < orderedVertices1.Length + orderedVertices2.Length; i += 3) {
            triangleIndices[i] = orderedVertices1.Length + vertexIndex;
            triangleIndices[i + 1] = vertexIndex;
            triangleIndices[i + 2] = orderedVertices1.Length + vertexIndex + 1;
            vertexIndex++;
        }
        mesh.triangles = triangleIndices;
    }


    public Vector3[] getVerticesInOrder(Vector3[] vertices) {

        AngleIntervalList<Vector3> inOrderVertices = new AngleIntervalList<Vector3>(vertices.Length);
        for (int i = 0; i < vertices.Length; ++i) {
            float angle = Vector3.Angle(Vector3.right, vertices[i]);
            inOrderVertices.add(vertices[i], angle);
        }

        return inOrderVertices.getOrderedElements();
    }

    public class AngleIntervalList<T> {
        private T[] elements;
        private int size;

        public AngleIntervalList(int size) {
            this.size = size;
            this.elements = new T[size];
        }

        public void add(T elem, float angle) {
            //Transform angle to positive value
            float newAngle = angle;
            if(angle < 0) {
                newAngle = 360 - angle;
            }

            float angleUnit = 360f / size;
            int index = Mathf.FloorToInt(newAngle / angleUnit);
            elements[index] = elem;
        }

        public T[] getOrderedElements() {
            return elements;
        }
    }
}
