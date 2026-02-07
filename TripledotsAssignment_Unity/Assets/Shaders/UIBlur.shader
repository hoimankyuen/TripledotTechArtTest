Shader "Custom/UIBlur"
{
    Properties
    {
        _BlurRadius ("Blur Radius", Range(0, 5)) = 1
    }
    
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        // Horizontal Partial Gaussian
        GrabPass
        {                    
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
          
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord: TEXCOORD0;
                float4 color : COLOR;
            };
          
            struct v2f
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 color : COLOR0;
            };
          
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uv.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                o.uv.zw = o.vertex.zw;
                o.color = v.color;
                return o;
            }

            static const float _Kernel[6] = {0.160427, 0.148699, 0.118414, 0.081013, 0.047617, 0.024044};
            
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _BlurRadius;
            
            fixed4 RetrieveFromGrab(float4 uv, int offset)
            {
                return tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv.x + _GrabTexture_TexelSize.x * offset * _BlurRadius, uv.y, uv.z, uv.w)));
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 blurred = fixed4(0,0,0,0);
                for (int offset = -5; offset <= 5; offset++)
                {
                    blurred += RetrieveFromGrab(i.uv, offset) * _Kernel[abs(offset)];
                }
                fixed4 unblurred = RetrieveFromGrab(i.uv, 0);
                return blurred * i.color.a + unblurred * (1 - i.color.a);
            }
            ENDCG
        }

        // Vertical Partial Gaussian
        GrabPass
        {                        
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
          
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord: TEXCOORD0;
                float4 color : COLOR;
            };
          
            struct v2f
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 color : COLOR0;
            };
          
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uv.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                o.uv.zw = o.vertex.zw;
                o.color = v.color;
                return o;
            }

            static const float _Kernel[6] = {0.160427, 0.148699, 0.118414, 0.081013, 0.047617, 0.024044};
            
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _BlurRadius;

            fixed4 RetrieveFromGrab(float4 uv, float offset)
            {
                return tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(uv.x, uv.y + _GrabTexture_TexelSize.y * offset * _BlurRadius, uv.z, uv.w)));
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 blurred = fixed4(0,0,0,0);
                for (int offset = -5; offset <= 5; offset++)
                {
                    blurred += RetrieveFromGrab(i.uv, offset) * _Kernel[abs(offset)];
                }
                fixed4 unblurred = RetrieveFromGrab(i.uv, 0);
                return blurred * i.color.a + unblurred * (1 - i.color.a);
            }
            ENDCG
        }
       
        // UI Colouring
        GrabPass
        {                        
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord: TEXCOORD0;
                float4 color : COLOR;
            };
          
            struct v2f
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 color : COLOR0;
            };
            
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uv.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                o.uv.zw = o.vertex.zw;
                o.color = v.color;
                return o;
            }
            
            sampler2D _GrabTexture;
          
            fixed4 frag( v2f i ) : SV_Target
            {
                fixed4 colour = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uv));
                return colour * i.color;
            }
            ENDCG
        }
    }
}