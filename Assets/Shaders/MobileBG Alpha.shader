Shader "Custom/MobileBG Alpha"
{
	Properties
	{
	    _MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		//Tags { "Queue" = "Background+1" }
		Tags { "Queue" = "Background+1" "IgnoreProjector"="True" "RenderType" = "Transparent" }

        Lighting off
        Ztest Off
        Zwrite off
        Fog { Mode off }
        
        Blend SrcAlpha OneMinusSrcAlpha // Alpha blending
		//Blend DstAlpha SrcAlpha
		//Blend One One                       // Additive
		//Blend OneMinusDstColor One          // Soft Additive
		//Blend DstColor Zero                 // Multiplicative
		//Blend DstColor SrcColor             // 2x Multiplicative
        
	    Pass
	    {
	    	SetTexture [_MainTex] { Combine texture }
	    }
	}
}