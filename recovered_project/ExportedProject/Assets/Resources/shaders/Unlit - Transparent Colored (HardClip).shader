Shader "Unlit/Transparent Colored (HardClip)" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
}
SubShader {
 LOD 100
 Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
 Pass {
  ZWrite Off
  Cull Off
  Lighting Off
  Blend SrcAlpha OneMinusSrcAlpha

  CGPROGRAM
  #pragma vertex vert
  #pragma fragment frag
  #include "UnityCG.cginc"

  sampler2D _MainTex;
  float4 _MainTex_ST;

  struct appdata { float4 vertex : POSITION; float2 texcoord : TEXCOORD0; fixed4 color : COLOR; };
  struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; fixed4 color : COLOR; float2 clipUV : TEXCOORD1; };

  v2f vert(appdata v)
  {
   v2f o;
   o.pos = UnityObjectToClipPos(v.vertex);
   o.uv = v.texcoord;
   o.color = v.color;
   o.clipUV = v.vertex.xy * _MainTex_ST.xy + _MainTex_ST.zw;
   return o;
  }

  fixed4 frag(v2f i) : SV_Target
  {
   fixed4 col = tex2D(_MainTex, i.uv) * i.color;
   col.a *= step(abs(i.clipUV.x), 1.0) * step(abs(i.clipUV.y), 1.0);
   clip(col.a - 0.01);
   return col;
  }
  ENDCG
 }
}
}
