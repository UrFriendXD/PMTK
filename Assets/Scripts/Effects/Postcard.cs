using System.Collections;
using UnityEngine;

public class Postcard : MonoBehaviour
{
    [SerializeField] private MeshRenderer imageRenderer;
    [SerializeField] private UIController ui;
    [SerializeField] private RenderTexture texture;
    [SerializeField] private Material posteriseMaterial;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private AnimationCurve scaleCurve;
    
    private static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");
    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    public void Rasterise(bool withMaterial = true)
    {
        RenderTexture existing = (RenderTexture) imageRenderer.material.GetTexture(BaseMapId);
        texture = new RenderTexture(existing.width, existing.height, existing.depth);
        texture.Create();

        if (withMaterial)
            Graphics.Blit(existing, texture, posteriseMaterial);
        else
            Graphics.Blit(existing, texture);

        imageRenderer.material.SetTexture(BaseMapId, texture);
        
        // TODO: Play flash animation, and ShowUI during that.
        ShowUI(UIController.UIType.Menu);
    }

    public void Destroy()
    {
        if (texture)
            texture.Release();
        
        DestroyImmediate(gameObject);
    }

    public void Appear()
    {
        StopAllCoroutines();
        StartCoroutine(ScaleRoutine());
    }

    private IEnumerator ScaleRoutine()
    {
        Vector3 target = transform.localScale;
        transform.localScale = Vector3.zero;
        float start = Time.time;

        while (!Mathf.Approximately(0f, Vector3.Distance(target, transform.localScale)))
        {
            float t = (Time.time - start) / scaleSpeed;

            Vector3 scale = Vector3.Lerp(Vector3.zero, target, scaleCurve.Evaluate(t));
            transform.localScale = scale;

            yield return new WaitForEndOfFrame();
        }
    }
    

    public void ShowUI(UIController.UIType uiType) => ui.Show(uiType);

    public void HideUI() => ui.Hide();

    public void DisableUI() => ui.Disable();
}
