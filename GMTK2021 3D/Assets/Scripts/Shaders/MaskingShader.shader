Shader "Custom/MaskingShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        //_WorldPos("World Space", Vector) = (0,0,0,0)
        //_Radius("Radius", Range(0, 100)) = 0
        //_FallOff("FallOff", Range(0, 100)) = 0
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

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

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

            half d = distance(GLOBAL_WorldPos, IN.worldPos);
            half sum = saturate((d - GLOBAL_Radius) / GLOBAL_FallOff);
            fixed4 lerpCol = lerp(c, fixed4(c_g, 1),sum);



            o.Albedo = lerpCol.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
