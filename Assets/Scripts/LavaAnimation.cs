using UnityEngine;

public class LavaAnimation : MonoBehaviour
{
    public float scrollSpeedX = 0.05f;
    public float scrollSpeedY = 0.05f;
    private Material lavaMaterial;

    void Start()
    {
        lavaMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        lavaMaterial.SetTextureOffset("_BaseMap", new Vector2(offsetX, offsetY));
    }
}