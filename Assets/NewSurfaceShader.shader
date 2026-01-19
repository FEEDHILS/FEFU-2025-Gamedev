Shader "Custom/Skybox/FogBlendCubemap"
{
    Properties
    {
        _CubeTex ("Sky Cubemap", Cube) = "" {}
        _Tint ("Tint", Color) = (1,1,1,1)
        _Exposure ("Exposure", Range(0,4)) = 1.0

        _FogColor ("Fog Color", Color) = (0.8,0.85,0.9,1)
        _FogStrength ("Fog Strength", Range(0,1)) = 0.65
        _FogPower ("Fog Power (how fast fog increases toward horizon)", Range(0.1,8)) = 2.0

        _NoiseTex ("Noise (optional)", 2D) = "gray" {}
        _NoiseScale ("Noise Scale", Range(0.1,10)) = 2.5
        _NoiseStrength ("Noise Strength", Range(0,1)) = 0.25

        _HorizonOffset ("Horizon Offset", Range(-0.5,0.5)) = 0.0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" }
        Cull Off
        ZWrite Off
        Lighting Off
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            samplerCUBE _CubeTex;
            sampler2D _NoiseTex;
            float4 _Tint;
            float _Exposure;

            float4 _FogColor;
            float _FogStrength;
            float _FogPower;

            float _NoiseScale;
            float _NoiseStrength;
            float _HorizonOffset;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 viewDir : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = worldPos - _WorldSpaceCameraPos;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 dir = normalize(i.viewDir);

                // берем цвет из кубической карты
                fixed4 sky = texCUBE(_CubeTex, dir) * _Tint;
                sky.rgb = pow(sky.rgb * _Exposure, float3(1.0,1.0,1.0));

                // фактор горизонта для тумана
                float horizonFactor = saturate(1.0 - dir.y + _HorizonOffset);
                float fogCurve = pow(horizonFactor, _FogPower);

                // шум-маска
                float2 noiseUV = float2(dir.x, dir.z) * 0.5 + 0.5; // проецируем на XZ
                noiseUV *= _NoiseScale;
                float noise = tex2D(_NoiseTex, noiseUV).r;
                float noiseMask = lerp(1.0, noise, _NoiseStrength);

                float totalMix = saturate(fogCurve * _FogStrength * noiseMask);

                fixed4 finalCol = lerp(sky, _FogColor, totalMix);
                finalCol.a = 1.0;

                return finalCol;
            }
            ENDCG
        }
    }
    FallBack Off
}
