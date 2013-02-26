Shader "Custom/Trail Particle" {
   SubShader {
      Pass {
         GLSLPROGRAM
 
         varying vec4 color;

         #ifdef VERTEX

         void main() {
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
            color = gl_Color * max(0.2, (gl_NormalMatrix * gl_Normal).z);
         }
 
         #endif
 
         #ifdef FRAGMENT
 
         void main() {
            gl_FragColor = color;
         }
 
         #endif
 
         ENDGLSL
      }
   }
}