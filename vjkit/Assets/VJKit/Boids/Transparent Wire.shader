Shader "Custom/Transparent Wire" {
   Properties {
      _Color ("Color", Color) = (1, 1, 1, 1)
   }
   SubShader {
      Tags {"Queue" = "Transparent"}
      Pass {    
         Cull Off
         ZTest Always
         ZWrite Off
         Blend SrcAlpha OneMinusSrcAlpha
         Fog { Mode off }      

         GLSLPROGRAM
 
         uniform vec4 _Color;

         #ifdef VERTEX

         void main() {
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main() {
            gl_FragColor = _Color;
         }
 
         #endif
 
         ENDGLSL
      }
   }
}