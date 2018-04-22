Shader "Custom/CycleColor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_xSpeed ("X Cycling Speed", Range(-50,50)) = 1.0
		_xLength ("X Length", Range(0.001, 10)) = 1.0
		_ySpeed ("Y Cycling Speed", Range(-50,50)) = 1.0
		_yLength ("Y Length", Range(0.001, 10)) = 1.0
		_zSpeed ("Z Cycling Speed", Range(-50,50)) = 1.0
		_zLength ("Z Length", Range(0.001, 10)) = 1.0
		_xMax ("X Max", Range( 1, 10)) = 1.0
		_yMax ("Y Max", Range( 1, 10)) = 1.0
		_zMax ("Z Max", Range( 1, 10)) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float3 worldPos;
			float3 objPos;
		};

		fixed4 _Color;
		fixed4 _OverlayColor;
		half _xSpeed;
		half _xLength;
		half _ySpeed;
		half _yLength;
		half _zSpeed;
		half _zLength;
		half _xMax;
		half _yMax;
		half _zMax;

		void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.objPos = v.vertex;
        }

		void surf (Input IN, inout SurfaceOutput o) {

			float x = _SinTime.x * _xSpeed;
			float y = _SinTime.y * _ySpeed;
			float z = _SinTime.z * _zSpeed;

			fixed4 newColor = fixed4( 
				fmod( abs(IN.objPos.x + x), _xLength), 
				fmod( abs(IN.worldPos.y + y), _yLength), 
				fmod( abs(IN.objPos.z + z), _zLength), 1);

			if( newColor.x >= _xMax ) 
				newColor.x = 0.1;
			if( newColor.y >= _yMax ) 
				newColor.y = 0.1;
			if( newColor.z >= _zMax ) 
				newColor.z = 0.1;

			o.Albedo = newColor / _Color;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
