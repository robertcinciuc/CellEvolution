using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoundChecker : MonoBehaviour
{

    private Vector3 planeSize;
    private Dictionary<LocalPlanes, Vector3> planes;
    private GameObject parent;
    private LocalPlanes localPlane;
    private Vector3 localPlaneCoord;

    void Start(){
        parent = gameObject;
        planeSize = WorldPlaneRenderer.planeSize;
        planes = new Dictionary<LocalPlanes, Vector3>();
        planes.Add(LocalPlanes.CURRENT_PLANE, WorldPlaneRenderer.currPlane.transform.position);
        planes.Add(LocalPlanes.X_PLANE, WorldPlaneRenderer.xPlane.transform.position);
        planes.Add(LocalPlanes.Z_PLANE, WorldPlaneRenderer.zPlane.transform.position);
        planes.Add(LocalPlanes.XZ_PLANE, WorldPlaneRenderer.xzPlane.transform.position);
    }

    void Update(){
    }

    public void setLocalPlane(LocalPlanes localPlane, Vector3 localPlaneCoord) {
        this.localPlane = localPlane;
        this.localPlaneCoord = localPlaneCoord;
    }

    private void FixedUpdate() {
        planes[LocalPlanes.CURRENT_PLANE] = WorldPlaneRenderer.currPlane.transform.position;
        planes[LocalPlanes.X_PLANE] = WorldPlaneRenderer.xPlane.transform.position;
        planes[LocalPlanes.Z_PLANE] = WorldPlaneRenderer.zPlane.transform.position;
        planes[LocalPlanes.XZ_PLANE] = WorldPlaneRenderer.xzPlane.transform.position;

        LocalPlanes botLeftPlane = LocalPlanes.XZ_PLANE;
        Vector3 botLeftCorner = getGlobalPlaneBottomLeftCoord(ref botLeftPlane);

        //bool parentXBiggerPlaneX = false;
        //bool parentXSmallerPlaneX = false;
        //bool parentZBiggerPlaneZ = false;
        //bool parentZSmallerPlaneZ = false;

        //bool localPlaneXBiggerCornerPlaneX = true;
        //bool localPlaneZBiggerCornerPlaneZ = true;

        //if (parent.transform.position.x > localPlaneCoord.x + planeSize.x / 2) {
        //    parentXBiggerPlaneX = true;
        //}
        //if (parent.transform.position.x < localPlaneCoord.x - planeSize.x / 2) {
        //    parentXSmallerPlaneX = true;
        //}
        //if (parent.transform.position.z > localPlaneCoord.z + planeSize.z / 2) {
        //    parentZBiggerPlaneZ = true;
        //}
        //if (parent.transform.position.z < localPlaneCoord.z - planeSize.z / 2) {
        //    parentZSmallerPlaneZ = true;
        //}

        //if(parent.transform.position.z < localPlaneCoord.z) {
        //    parentZBiggerPlaneZ = false;
        //}
        //if (planes[localPlane].x < botLeftCorner.x) {
        //    localPlaneXBiggerCornerPlaneX = false;
        //}
        //if (planes[localPlane].z < botLeftCorner.z) {
        //    localPlaneZBiggerCornerPlaneZ = false;
        //}

        //Based on the 4 booleans either destroy the enemy or switch it between planes

        //Switch enemy between planes
        //Destroy enemy if it's beyond the total plane 

        if(parent.transform.position.x < botLeftCorner.x ||
           parent.transform.position.x > botLeftCorner.x + planeSize.x * 2 ||
           parent.transform.position.z < botLeftCorner.z ||
           parent.transform.position.z > botLeftCorner.z + planeSize.z * 2) {
            EnemySpawner.planeEnemies[localPlane].Remove(parent.GetInstanceID());
            Destroy(parent);
        } else {

        }
    }

    private Vector3 getGlobalPlaneBottomLeftCoord(ref LocalPlanes botLeftPlane) {
        float xExtremity = planes[LocalPlanes.CURRENT_PLANE].x - planeSize.x /2;
        float zExtremity = planes[LocalPlanes.CURRENT_PLANE].z - planeSize.z /2;
        botLeftPlane = LocalPlanes.CURRENT_PLANE;

        if(planes[LocalPlanes.CURRENT_PLANE].x > planes[LocalPlanes.X_PLANE].x) {
            xExtremity = planes[LocalPlanes.X_PLANE].x - planeSize.x / 2;
            botLeftPlane = LocalPlanes.X_PLANE;
        }
        if(planes[LocalPlanes.CURRENT_PLANE].z > planes[LocalPlanes.Z_PLANE].z) {
            zExtremity = planes[LocalPlanes.Z_PLANE].z - planeSize.z / 2;

            if (botLeftPlane == LocalPlanes.X_PLANE) {
                botLeftPlane = LocalPlanes.XZ_PLANE;
            }
        }

        return new Vector3(xExtremity, 0, zExtremity);
    }
}
