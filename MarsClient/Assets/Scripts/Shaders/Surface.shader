Shader "Hit" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,0)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Illum ("Illumin (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_EmissionLM ("Emission (Lightmapper)", Float) = 0
}

SubShader {
	LOD 300
	Tags { "RenderType"="Opaque" }
	
	Pass {
		Name "BASE"
		Tags {"LightMode" = "Vertex"}
		Material {
			Diffuse (1, 1, 1, 1)
			Ambient (1, 1, 1, 1)
		}
		Lighting On
		SetTexture [_MainTex] { Combine texture * primary Double, texture alpha * primary alpha }
		SetTexture [_MainTex] { constantColor [_Color] combine constant lerp (constant) previous }
		//Vector 23 [_Illum_ST]
	}
}

Fallback "VertexLit"
}
