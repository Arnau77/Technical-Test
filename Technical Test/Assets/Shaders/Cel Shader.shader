Shader "Unlit/Cel Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadingBands("ShadingBands", Range(0,1)) = 1
        _Brightness("Brightness", Range(0,1)) = 0.7
        _Strength("Strength", Range(0,1)) = 0.5
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
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ShadingBands;
            float _Strength;
            float _Brightness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Normalize the normal vector
                float3 normalNormalized = normalize(i.worldNormal);
                //Do the dot product between the normal and the light vector
                float dotProduct = dot(_WorldSpaceLightPos0, normalNormalized);
                //Make the dot product superior than 0 (delete the negative values)
                dotProduct = max(0.0, dotProduct);
                //Get the integer truncated of the division between the dot product and the shading bands property (the greater the number the less shading bands)
                dotProduct = floor(dotProduct / _ShadingBands);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                //Retunr the multiplication of the dot product and its strength property, added the brigthness property, and finally, multiplicated again by the color got by the texture
                return col * (dotProduct*_Strength + _Brightness);
            }
            ENDCG
        }
    }
}
