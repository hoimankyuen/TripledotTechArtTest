Shader "Custom/Unlit/UIRoundedSquareFader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _GridSize ("Grid Size", Vector, 2) = (1000, 1400, 0, 0)
        _RoundedSize ("Rounded Size", Float) = 100
        
        _FadeFrom ("Fade From", Float) = 100
        _FadeTo ("Fade To", Float) = 600
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 worldPosition : TEXCOORD1;
                float4 color : COLOR0;
            };

            fixed4 _Color;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                o.color = v.color;
                return o;
            }
            
            float mod(float a, float b)
            {
                return (a % b + b) % b;
            }

            float2 mod(float2 a, float2 b)
            {
                return float2(mod(a.x, b.x), mod(a.y, b.y));
            }

            float4 _GridSize;
            float _RoundedSize;

            float _FadeFrom;
            float _FadeTo;
            
            static const float _Corners[10] = {0, 0, 1, 0, -1, 0, 0, 1, 0, -1}; // actually 5 float2s
            static const float _Edges[10] = {0, 0, 1, 1, -1, 1, 1, -1, -1, -1}; // actually 5 float2s

            float GetRoundedSquare(float2 worldPosition, float2 offset, float size)
            {
                float squareSize = max(size - _RoundedSize, 0);
                float roundedSize = max(min(size, _RoundedSize), 0);
                
                float alpha = 0;
                float2 pos = mod(worldPosition + offset, _GridSize) - _GridSize / 2;
                for (int j = 0; j < 10; j+=2)
                {
                    float2 corner = float2(_Corners[j], _Corners[j + 1]) * squareSize;
                    alpha = max(alpha, step(length(pos - corner), roundedSize));
                    float2 edges = float2(_Edges[j], _Edges[j + 1]) * roundedSize * sin(radians(45));
                    alpha = max(alpha, step(abs(pos.x - edges.x) + abs(pos.y - edges.y), squareSize));
                }
                return alpha;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float size = clamp((i.worldPosition.y - _FadeFrom) / (_FadeTo - _FadeFrom), 0, 1) * max(_GridSize.x / 2, _GridSize.y / 2);
                fixed4 color = i.color;
                color.a = 0;
                color.a = max(color.a, GetRoundedSquare(i.worldPosition.xy, float2(0, 0), size));
                color.a = max(color.a, GetRoundedSquare(i.worldPosition.xy, _GridSize / 2, size));
                return color;
            }
            ENDCG
        }
    }
}
