using UnityEngine;

public class ComputerOutline : MonoBehaviour
{
    private MeshRenderer rend;
    private Material[] originalMaterials;
    public Material outlineMaterial; // Shader Graph·Î ¸¸µç Outline Material
    private bool isOutlined = false;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        originalMaterials = rend.materials;
    }

    // Update is called once per frame
    public void ToggleOutline()
    {
        if (isOutlined)
            ToggleOutlineOff();
        else
        {
            rend.materials = new Material[] { originalMaterials[0], outlineMaterial };
            isOutlined = true;
        }
    }

    public void ToggleOutlineOff()
    {
        rend.materials = new Material[] { originalMaterials[0] };
        isOutlined = false;
    }

    public bool IsOutlined() => isOutlined;
}
