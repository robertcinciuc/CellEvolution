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
        planeSize = TerrainRenderer.planeSize;
        planes = new Dictionary<LocalPlanes, Vector3>();
        planes.Add(LocalPlanes.CURRENT_PLANE, TerrainRenderer.currPlane.transform.position);
        planes.Add(LocalPlanes.X_PLANE, TerrainRenderer.xPlane.transform.position);
        planes.Add(LocalPlanes.Z_PLANE, TerrainRenderer.zPlane.transform.position);
        planes.Add(LocalPlanes.XZ_PLANE, TerrainRenderer.xzPlane.transform.position);
    }

    void Update(){
    }

    public void setLocalPlane(LocalPlanes localPlane, Vector3 localPlaneCoord) {
        this.localPlane = localPlane;
        this.localPlaneCoord = localPlaneCoord;
    }

    private void FixedUpdate() {
        planes[LocalPlanes.CURRENT_PLANE] = TerrainRenderer.currPlane.transform.position;
        planes[LocalPlanes.X_PLANE] = TerrainRenderer.xPlane.transform.position;
        planes[LocalPlanes.Z_PLANE] = TerrainRenderer.zPlane.transform.position;
        planes[LocalPlanes.XZ_PLANE] = TerrainRenderer.xzPlane.transform.position;

        updateLocalPlane();

        updateEnemyOwnership();
    }

    private Positions getPosAgainstPlane(Vector3 objToLocate, Vector3 planePos, Vector3 planeSize) {
        if(isInsidePlane(objToLocate, planePos, planeSize) == Positions.INSIDE) {
            return Positions.INSIDE;
        }

        if (objToLocate.x < planePos.x - planeSize.x / 2) {
            if (objToLocate.z < planePos.z - planeSize.z / 2) {
                return Positions.LOWER_LEFT;
            }
            if (objToLocate.z > planePos.z + planeSize.z / 2) {
                return Positions.UPPER_LEFT;
            }
            return Positions.LEFT;
        }

        if (objToLocate.x > planePos.x + planeSize.x / 2) {
            if (objToLocate.z < planePos.z - planeSize.z / 2) {
                return Positions.LOWER_RIGHT;
            }
            if (objToLocate.z > planePos.z + planeSize.z / 2) {
                return Positions.UPPER_RIGHT;
            }
            return Positions.RIGHT;
        }

        if (objToLocate.z < planePos.z - planeSize.z / 2) {
            return Positions.LOWER;
        }

        return Positions.UPPER;
    }

    private Positions isInsidePlane(Vector3 objectToLocate, Vector3 planePos, Vector3 planeSize) {
        if(objectToLocate.x > planePos.x + planeSize.x / 2 ||
           objectToLocate.x < planePos.x - planeSize.x / 2 ||
           objectToLocate.z > planePos.z + planeSize.z / 2 ||
           objectToLocate.z < planePos.z - planeSize.z / 2) {
            return Positions.OUTSIDE;
        }

        return Positions.INSIDE;
    }

    private void updateEnemyOwnership() {
        //Get middle point of total plane
        Vector3 totalPlaneSize = new Vector3(planeSize.x * 2, 0, planeSize.z * 2);
        Vector3 totalPlaneMidPoint = new Vector3(
            Mathf.Min(planes[LocalPlanes.CURRENT_PLANE].x, planes[LocalPlanes.X_PLANE].x) + planeSize.x / 2,
            0,
            Mathf.Min(planes[LocalPlanes.CURRENT_PLANE].z, planes[LocalPlanes.Z_PLANE].z) + planeSize.z / 2);

        //Destroy enemy if outside the total plane or switch enemy between planes when crossing their border
        if (isInsidePlane(parent.transform.position, totalPlaneMidPoint, totalPlaneSize) == Positions.OUTSIDE) {
            EnemySpawner.deleteEnemy(parent.GetInstanceID());
        } else {
            Positions objAgainstLocalPlane = getPosAgainstPlane(parent.transform.position, localPlaneCoord, planeSize);

            if (objAgainstLocalPlane == Positions.INSIDE) {
                return;
            }

            bool isInsideXPlane = Positions.INSIDE == isInsidePlane(parent.transform.position, planes[LocalPlanes.X_PLANE], planeSize);
            bool isInsideZPlane = Positions.INSIDE == isInsidePlane(parent.transform.position, planes[LocalPlanes.Z_PLANE], planeSize);
            bool isInsideXZPlane = Positions.INSIDE == isInsidePlane(parent.transform.position, planes[LocalPlanes.XZ_PLANE], planeSize);
            LocalPlanes targetPlane = LocalPlanes.CURRENT_PLANE;
            if (isInsideXPlane) {
                targetPlane = LocalPlanes.X_PLANE;
            } else if (isInsideZPlane) {
                targetPlane = LocalPlanes.Z_PLANE;
            } else if (isInsideXZPlane) {
                targetPlane = LocalPlanes.XZ_PLANE;
            }

            EnemySpawner.moveEnemyToPlane(parent.GetInstanceID(), localPlane, targetPlane);
            setLocalPlane(targetPlane, planes[targetPlane]);
        }
    }

    private void updateLocalPlane() {
        foreach (KeyValuePair<LocalPlanes, Dictionary<int, GameObject>> planeEntry in EnemySpawner.planeEnemies) {
            if (planeEntry.Value.ContainsKey(parent.GetInstanceID())) {
                localPlane = planeEntry.Key;
                localPlaneCoord = TerrainRenderer.getPlaneObject(localPlane).transform.position;
            }
        }
    }
}
