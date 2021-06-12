Shader "Custom/MaskingShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _ColorStrength("Color Strength", Range(1,4)) = 1
        _EmissionColor("Emission Color", Color) = (1,1,1,1)
        _EmissionTex("Emission", 2D) = "white" {}
        _EmissionStrength("Emission Strength", Range(1,10)) = 1
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex, _EmissionTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissionTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color, _EmissionColor;
        half _ColorStrength, _EmissionStrength;

        uniform fixed4 GLOBAL_WorldPos;
        uniform half GLOBAL_Radius;
        uniform half GLOBAL_FallOff;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            half grayscale = (c.r + c.g + c.b) * 0.25;
            fixed3 c_g = fixed3(grayscale, grayscale, grayscale);

            fixed4 e = tex2D(_EmissionTex, IN.uv_EmissionTex) * _EmissionColor * _EmissionStrength;

            half d = distance(GLOBAL_WorldPos, IN.worldPos);
            half sum = saturate((d - GLOBAL_Radius) / -GLOBAL_FallOff);
            fixed4 lerpCol = lerp(fixed4(c_g, 1), c * _ColorStrength,sum);
            fixed4 lerpEmission = lerp(fixed4(0, 0, 0, 0), e, sum);


            o.Albedo = lerpCol.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Emission = lerpEmission.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
