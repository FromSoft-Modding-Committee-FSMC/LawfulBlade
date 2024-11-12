Shader "Lawful/Sprite (Black 2 Alpha)"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex VertexPassthrough
            #pragma fragment GreyscaleFragment

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            //
            // VERTEX PASSTHROUGH SHADER
            //
            v2f VertexPassthrough (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //
            // GREYSCALE SPRITE SHADER
            //
            fixed4 GreyscaleFragment (v2f i) : SV_Target
            {
                fixed4 colour = tex2D(_MainTex, i.uv);
                colour.a = all(colour.rgb);

                return colour;
            }
            ENDCG
        }
    }
}
