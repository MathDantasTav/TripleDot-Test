Shader "UI/ScrollingTiledImage"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _ScrollSpeed ("Scroll Speed (XY)", Vector) = (0.1, 0.0, 0, 0)
        _RotationSpeed ("Rotation Speed (deg/sec)", Float) = 10
        _RotationPivot ("Rotation Pivot (XY)", Vector) = (0.5, 0.5, 0, 0)
        _Tiling ("Tiling (XY)", Vector) = (1, 1, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _Color;
            float4 _ScrollSpeed;
            float4 _Tiling;
            float2 _RotationPivot;
            float _RotationSpeed;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            float2 RotateUV(float2 uv, float angle, float2 pivot)
            {
                float s = sin(angle);
                float c = cos(angle);

                uv -= pivot;

                float2 rotated;
                rotated.x = uv.x * c - uv.y * s;
                rotated.y = uv.x * s + uv.y * c;

                return rotated + pivot;
            }

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv * _Tiling.xy;
                o.color = v.color;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float time = _Time.y;

                float2 uv = i.uv + time * _ScrollSpeed.xy;

                uv = RotateUV(
                    uv,
                    time * _RotationSpeed * 0.0174533,
                    _RotationPivot
                );

                uv = frac(uv);

                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

                return tex * _Color * i.color;
            }

            ENDHLSL
        }
    }
}