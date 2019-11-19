using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scene1 {
    public class MeshManager : MonoBehaviour {
        public ComputeShader shader;
        public bool showDebug;
        public Vector3 gridSize;
        public int gridCount;
        public float isolevel = 5;
        Dictionary<Vector3, int> dic;
        List<Grid> chunks;
        int indexDebug;

        Vector4[] positions;


        private void Start() {
            dic = new Dictionary<Vector3, int>();
            chunks = new List<Grid>();
        }
        private void Update() {
            Vector3 pos = transform.position;
            Vector3Int n_pos = new Vector3Int(Mathf.FloorToInt(pos.x / gridSize.x), Mathf.FloorToInt(pos.y / gridSize.y), Mathf.FloorToInt(pos.z / gridSize.z));
            int value = 0;
            if (dic.TryGetValue(n_pos, out value)) {
            } else {

                Grid grid = new Grid(gridCount);
                grid.CreateGrid(shader, new Vector3(n_pos.x * gridSize.x, n_pos.y * gridSize.y, n_pos.z * gridSize.z), gridSize);
                positions = grid.GetPoints();

                grid.GenerateObject(isolevel);

                dic.Add(n_pos, chunks.Count);
                chunks.Add(grid);
            }
            indexDebug = value;
        }

        private void OnDrawGizmos() {
            if (!showDebug)
                return;

            foreach (var item in positions) {
                float value = Mathf.InverseLerp(-16, 16, item.w);
                float color = 256 * value;
                Color rgb = new Color(color, color, color);
                Gizmos.color = rgb;
                Gizmos.DrawSphere(item, 0.1f);
            }
        }
    }
}