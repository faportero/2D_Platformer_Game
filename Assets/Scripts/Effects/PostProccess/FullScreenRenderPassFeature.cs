using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FullScreenRenderPassFeature : ScriptableRendererFeature
{
    public Material postProcessMaterial; // Material con el shader de postproceso

    private FullScreenRenderPass renderPass;

    public override void Create()
    {
        renderPass = new FullScreenRenderPass(postProcessMaterial);
        renderPass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderPass.Setup(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(renderPass);
    }

    public void SetEnabled(bool enabled)
    {
        this.SetActive(enabled);
    }
}

public class FullScreenRenderPass : ScriptableRenderPass
{
    private Material material;
    private RenderTargetIdentifier source;

    public FullScreenRenderPass(Material material)
    {
        this.material = material;
    }

    public void Setup(RenderTargetIdentifier source)
    {
        this.source = source;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("FullScreenRenderPass");
        cmd.Blit(source, BuiltinRenderTextureType.CameraTarget, material);
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
