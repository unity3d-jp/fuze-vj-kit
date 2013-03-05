// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Solid Color" {

    Properties {
        _Color ("Main Color", COLOR) = (1,1,1,1)
    }
    
    SubShader {
        Pass { Color [_Color] }
    }
} 