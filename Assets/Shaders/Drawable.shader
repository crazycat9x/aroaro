Shader "Hidden/Drawable"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PenCoord ("Pen Coordinate", Vector) = (0,0,0,0)
        _PenColor ("Pen Color", Color) = (1,1,1,1)
        _PenWidth ("Pen Width", float) = 3
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float2 _PenCoord;
            float _PenWidth;
            float4 _PenColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float halfPen = _PenWidth/2;
                fixed4 col = tex2D(_MainTex, i.uv);
                if (i.uv.x >= _PenCoord.x - halfPen &&
                    i.uv.x <= _PenCoord.x + halfPen &&
                    i.uv.y >= _PenCoord.y - halfPen &&
                    i.uv.y <= _PenCoord.y + halfPen)
                {
                    col.rgba = _PenColor;
                }
                
                return col;
            }
            ENDCG
        }
    }
}
