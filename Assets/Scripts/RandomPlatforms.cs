using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    public Mesh[] availableMeshes;
    public Material lavaMaterial;

    [Header("Масштаб модели (X, Y, Z)")]
    public Vector3 visualScale = new Vector3(100f, 100f, 100f);

    void Start()
    {
        if (GetComponent<MeshRenderer>()) GetComponent<MeshRenderer>().enabled = false;
        GameObject visual = new GameObject("VisualStone");
        visual.transform.SetParent(this.transform);
        visual.transform.localPosition = Vector3.zero;

        visual.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        visual.transform.localScale = new Vector3(
            visualScale.x / transform.localScale.x,
            visualScale.y / transform.localScale.y,
            visualScale.z / transform.localScale.z
        );
        MeshFilter mf = visual.AddComponent<MeshFilter>();
        MeshRenderer mr = visual.AddComponent<MeshRenderer>();

        if (availableMeshes.Length > 0)
            mf.mesh = availableMeshes[Random.Range(0, availableMeshes.Length)];

        mr.material = lavaMaterial;
    }
}