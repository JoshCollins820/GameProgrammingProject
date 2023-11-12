Shader "Amazing Assets/Lowpoly Mesh Generator/Vertex Color (Lit)"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.0
        _Metallic ("Metallic", Range(0,1)) = 0.0
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


        fixed4 _Color;
        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;

        struct Input
        {
            float2 uv_MainTex;
            fixed4 color : COLOR;
        };                


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            //Texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            //Albedo comes from vertex color
            o.Albedo = c.rgb * IN.color.rgb;

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = IN.color.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
