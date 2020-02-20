Shader "Custom/MobileBG"
{
	Properties
	{
	    _MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags {"Queue" = "Background" }

        Lighting off
        Zwrite off
        Fog { Mode off }
        
	    Pass
	    {
	    	SetTexture [_MainTex] { Combine texture }
	    }
	}
}