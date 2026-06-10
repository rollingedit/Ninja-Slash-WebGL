Shader "Custom/CurvedZombient" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _QOffset ("Offset", Vector) = (0,0,0,0)
 _Dist ("Distance", Float) = 100
 _ZombientColor ("ZombientColor", Color) = (0,0,0,0)
}
SubShader {
 Tags { "RenderType"="Opaque" }
 LOD 200
 CGPROGRAM
 #pragma surface surf Lambert
 #pragma target 3.0
 sampler2D _MainTex;
 struct Input { float2 uv_MainTex; };
 void surf(Input IN, inout SurfaceOutput o) {
  fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
  o.Albedo = c.rgb;
 }
 ENDCG
}
}
