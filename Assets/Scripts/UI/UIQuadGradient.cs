using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class UIQuadGradient : BaseMeshEffect
{
    public Color topLeft = Color.white;
    public Color topRight = Color.white;
    public Color bottomLeft = Color.white;
    public Color bottomRight = Color.white;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        UIVertex vertex = new UIVertex();

        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);

            Vector2 pos = vertex.position;

            // normalize to 0–1 space
            float tX = pos.x > 0 ? 1f : 0f;
            float tY = pos.y > 0 ? 1f : 0f;

            Color top = Color.Lerp(topLeft, topRight, tX);
            Color bottom = Color.Lerp(bottomLeft, bottomRight, tX);

            vertex.color *= Color.Lerp(bottom, top, tY);

            vh.SetUIVertex(vertex, i);
        }
    }
}