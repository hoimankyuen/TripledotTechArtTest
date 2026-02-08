Shader "Custom/Unlit/UIReflection"
{
    Properties
    {
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        
        _ReflectionColor ("Reflection Color", Color) = (1, 1, 1, 1)
        _ReflectionDirection ("Reflection Direction", Float) = 0
        _ReflectionWidth ("Reflection Width", Float) = 10
        _ReflectionSpeed ("Reflection Speed", Float) = 1
        _ReflectionInterval ("Reflection Interval", Float) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            fixed4 _ReflectionColor;
            float _ReflectionDirection;
            float _ReflectionWidth;
            float _ReflectionSpeed;
            float _ReflectionInterval;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(o.worldPosition);
                o.color = v.color;
                return o;
            }

            float mod(float a, float b)
            {
                return (a % b + b) % b;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = i.color;

                float cosDir = cos(radians(_ReflectionDirection));
                float sinDir = sin(radians(_ReflectionDirection));
                float2x2 rot = float2x2(cosDir, -sinDir, sinDir, cosDir);
                float length = _ReflectionInterval * _ReflectionSpeed;
                float distance = mod(mul(i.worldPosition, rot).x + _ReflectionSpeed * _Time.y, length);
                color.a *= step(distance, _ReflectionWidth);
                
                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}