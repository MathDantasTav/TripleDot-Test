Shader "UI/UIBlur"
{
    Properties
    {
        _MainTex ("Render Texture", 2D) = "white" {}
        _BlurRadius ("Blur Radius", Float) = 1.0
        _Dim ("Global Dim", Range(0,1)) = 0.3
        _Vignette ("Vignette", Range(0,1)) = 0.4
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

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

            // Reference height used to normalize blur radius, keeping the effect consistent across different render resolutions
            static const float BlurResolutionReference = 1920.0;
            
            float4 _MainTex_TexelSize;
            float _BlurRadius;

            float _Dim;
            float _Vignette;

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

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float2 uv = i.uv;

                float resolutionAdjustedBlurRadius = _BlurRadius / _MainTex_TexelSize.y / BlurResolutionReference;
                float2 texel = _MainTex_TexelSize.xy * resolutionAdjustedBlurRadius;

                half4 col = 0;

                // Kawase-style blur
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2( texel.x,  texel.y));
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texel.x,  texel.y));
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2( texel.x, -texel.y));
                col += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(-texel.x, -texel.y));

                col *= 0.25;

                // slight center stability
                col = lerp(col, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv), 0.25);

                // dim
                col.rgb *= (1.0 - _Dim);

                // vignette
                float2 c = uv * 2.0 - 1.0;
                float v = dot(c, c);
                col.rgb *= (1.0 - v * _Vignette);

                // UI FADE CONTROL (RawImage alpha)
                col.a *= i.color.a;

                return col;
            }

            ENDHLSL
        }
    }
}