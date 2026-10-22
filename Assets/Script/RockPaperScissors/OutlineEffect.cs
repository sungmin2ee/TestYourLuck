using UnityEngine;

public class OutlineToggle : MonoBehaviour
{
    private MeshRenderer rend;
    private Material[] originalMaterials;
    public Material outlineMaterial; // Shader Graph로 만든 Outline Material
    private bool isOutlined = false;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        originalMaterials = rend.materials;
        OutlineManager.Register(this); // 전역 리스트에 등록
    }

    void OnDestroy()
    {
        OutlineManager.Unregister(this); // 제거
    }

    public void ToggleOutline()
    {
        if (isOutlined)
            ToggleOutlineOff();
        else
        {
            rend.materials = new Material[] { originalMaterials[0], outlineMaterial };
            isOutlined = true;
        }
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
    }

    public void ToggleOutlineOff()
    {
        rend.materials = new Material[] { originalMaterials[0] };
        isOutlined = false;
        VolumeSettings.Instance.PlaySfx(VolumeSettings.Sfx.Button);
    }

    public bool IsOutlined() => isOutlined;
}
