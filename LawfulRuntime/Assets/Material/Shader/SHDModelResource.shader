Shader "Lawful/ModelResource"
{
    Properties
    {
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float3 positionA   : POSITION;      // Stream #0 - Position
                float3 normalA     : NORMAL;        // Stream #0 - Normal

                float3 positionB   : TEXCOORD4;     // Stream #1 - Position
                float3 normalB     : TEXCOORD5;     // Stream #1 - Position

                float2 texcoord    : TEXCOORD0;     // Stream #2 - Texture Coordinates
                float4 colour      : TEXCOORD1;     // Stream #2 - Colour
                uint4  boneIndices : TEXCOORD6;     // Stream #2 - Bone Indices
                float4 boneWeights : TEXCOORD7;     // Stream #2 - Bone Weights
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float3 normal   : NORMAL;
                float4 colour   : COLOR;
                float2 uv       : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.position   = UnityObjectToClipPos(v.positionA);
                o.normal     = normalize(v.normalA);
                o.uv         = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.colour     = v.colour;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return i.colour * tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
