using System.Collections;
using UnityEngine;

public class Postcard : MonoBehaviour
{
    public bool posterise;
    
    [SerializeField] private MeshRenderer imageRenderer;
    [SerializeField] private UIController ui;
    [SerializeField] private RenderTexture originalTexture;
    [SerializeField] private RenderTexture posterisedTexture;
    [SerializeField] private Material posteriseMaterial;
    
    [SerializeField, Range(0f, 1f)] private float flashPeakTime;
    
    [SerializeField] private float scaleDuration;
    [SerializeField] private AnimationCurve scaleCurve;
    
    [SerializeField] private float flashDuration;
    [SerializeField] private AnimationCurve flashCurve;
    
    private static readonly int MainTexId = Shader.PropertyToID("_MainTex");
    private static readonly int OriginalTexId = Shader.PropertyToID("_OriginalTex");
    private static readonly int FlashAmountId = Shader.PropertyToID("_FlashAmount");
    private static readonly int FlashLerpId = Shader.PropertyToID("_FlashLerp");

    private Coroutine _appearRoutine;
    private Coroutine _flashRoutine;

    
    public void Rasterise(bool withMaterial = true)
    {
        RenderTexture existing = (RenderTexture) imageRenderer.material.GetTexture(MainTexId);
        
        originalTexture = new RenderTexture(existing.descriptor);
        originalTexture.Create();

        posterisedTexture = new RenderTexture(existing.descriptor);
        originalTexture.Create();
        
        Graphics.Blit(existing, originalTexture);
        Graphics.Blit(existing, posterisedTexture, posteriseMaterial);

        imageRenderer.material.SetTexture(MainTexId, originalTexture);
        imageRenderer.material.SetTexture(OriginalTexId, posterisedTexture);

        Flash();
    }

    public void Destroy()
    {
        if (originalTexture)
            originalTexture.Release();
        
        DestroyImmediate(gameObject);
    }

    public void Scale()
    {
        if (!(_appearRoutine is null))
            StopCoroutine(_appearRoutine);
        
        _appearRoutine = StartCoroutine(ScaleRoutine());
    }

    private void Flash()
    {
        if (!(_flashRoutine is null))
            StopCoroutine(_flashRoutine);

        _flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float start = Time.time;

        while (Time.time - start < flashDuration)
        {
            float t = (Time.time - start) / flashDuration;
            if (t >= flashPeakTime)
            {
                ShowUI(UIController.UIType.Menu);
                imageRenderer.material.SetFloat(FlashLerpId, posterise ? 1f : 0f);
            }
            
            float amount = flashCurve.Evaluate(t);
            imageRenderer.material.SetFloat(FlashAmountId, amount * (posterise ? 1f : 0f));

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ScaleRoutine()
    {
        Vector3 target = transform.localScale;
        transform.localScale = Vector3.zero;
        float start = Time.time;

        while (!Mathf.Approximately(0f, Vector3.Distance(target, transform.localScale)))
        {
            float t = (Time.time - start) / scaleDuration;

            Vector3 scale = Vector3.Lerp(Vector3.zero, target, scaleCurve.Evaluate(t));
            transform.localScale = scale;

            yield return new WaitForEndOfFrame();
        }
    }
    

    public void ShowUI(UIController.UIType uiType) => ui.Show(uiType);

    public void HideUI() => ui.Hide();

    public void DisableUI() => ui.Disable();
}
