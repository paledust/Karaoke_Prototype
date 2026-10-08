using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if NEW_PREFAB_SYSTEM
	[ExecuteAlways]
#else
	[ExecuteInEditMode]
#endif
public class SpineMeshMatch : MonoBehaviour
{
    [SerializeField] private MeshFilter referenceMeshFilter;
    MeshFilter ownMeshFilter;
    private void Reset(){
		if (referenceMeshFilter == null) {
			referenceMeshFilter = this.transform.parent.GetComponentInParent<MeshFilter>();
			if (!referenceMeshFilter)
				return;
		}
    }
	void Awake () {
		ownMeshFilter = this.GetComponent<MeshFilter>();
	}
    void OnEnable(){
        Awake();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ownMeshFilter.sharedMesh = referenceMeshFilter.sharedMesh;
    }
}
