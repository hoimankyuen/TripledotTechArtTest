Shader "Custom/Unlit/UILoopingBackground"
{
    Properties
    {
        _Pattern ("Pattern", 2D) = "white" {}
        _PatternColor ("Pattern Color", Color) = (1, 1, 1, 1)
        _PatternMovement ("Pattern Movement", Vector, 2) = (0, 0, 0, 0)
        _BackgroundColor1 ("Background Color 1", Color) = (1, 1, 1, 1)
        _BackgroundColor2 ("Background Color 2", Color) = (1, 1, 1, 1)

    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 patternUV : TEXCOORD1;
                float4 color : COLOR0;
            };

            sampler2D _Pattern;
            float4 _Pattern_ST;
            float4 _Pattern_TexelSize;
            fixed4 _PatternColor;
            float4 _PatternMovement;

            fixed4 _BackgroundColor1;
            fixed4 _BackgroundColor2;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                float2 patternUV = (v.vertex + _PatternMovement * _Time.y) * _Pattern_TexelSize.xy;
                o.patternUV = TRANSFORM_TEX(patternUV, _Pattern);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 backgroundColor = lerp(_BackgroundColor2, _BackgroundColor1, clamp(i.uv.y * 2 - 1, 0, 1));
                fixed patternAlpha = tex2D(_Pattern, i.patternUV).a * _PatternColor.a;
                fixed4 color = fixed4(_PatternColor.rgb, 1) * patternAlpha + backgroundColor * (1 - patternAlpha);
                color = color * i.color;
                return color;
            }
            ENDCG
        }
    }
}
