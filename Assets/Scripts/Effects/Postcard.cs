using UnityEngine;

public class Postcard : MonoBehaviour
{
    [SerializeField] private MeshRenderer imageRenderer;
    [SerializeField] private RenderTexture texture;
    [SerializeField] private Material posteriseMaterial;
    
    private static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");

    public void Rasterise()
    {
        if (Application.isEditor && !Application.isPlaying)
            return;
        
        RenderTexture existing = (RenderTexture) imageRenderer.material.GetTexture(BaseMapId);
        texture = new RenderTexture(existing.width, existing.height, existing.depth);
        texture.Create();

        Graphics.Blit(existing, texture, posteriseMaterial);

        imageRenderer.material.SetTexture(BaseMapId, texture);
    }
}
