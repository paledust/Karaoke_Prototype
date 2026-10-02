// Made with Amplify Shader Editor v1.9.9.9
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaders/Particles RadiusWave"
{
	Properties
	{
		_MainTex( "MainTex", 2D ) = "white" {}
		_EmissionShimering( "EmissionShimering", Range( 0, 1 ) ) = 0
		_ShimeringFreq( "ShimeringFreq", Float ) = 5
		_EmissionScale( "EmissionScale", Float ) = 1
		_NoiseStrength( "NoiseStrength", Float ) = 1
		_NoiseTiling( "NoiseTiling", Float ) = 1
		_NoiseSpeed( "NoiseSpeed", Vector ) = ( 1, 1, 0, 0 )
		_NoiseMin( "NoiseMin", Float ) = -1.25
		_NoiseMax( "NoiseMax", Float ) = 1
		_NoiseTex( "NoiseTex", 2D ) = "white" {}
		_OutterRingRadius( "OutterRingRadius", Float ) = 0.2
		_OutterRingWidth( "OutterRingWidth", Float ) = 0.03
		_InnerRingRadius( "InnerRingRadius", Float ) = 0.2
		_InnerRingWidth( "InnerRingWidth", Float ) = 0.2
		_Max( "Max", Float ) = 1
		_Min( "Min", Float ) = 0
		_FadeNoiseTex( "FadeNoiseTex", 2D ) = "white" {}
		_FadeRadius( "FadeRadius", Float ) = 0
		_FadeLength( "FadeLength", Float ) = 0.1
		_FadeSmooth( "FadeSmooth", Float ) = 1
		_FadeStrength( "FadeStrength", Float ) = 1
		_FadePatternScale( "FadePatternScale", Float ) = 1
		[KeywordEnum( UV_W,Value )] _InverseLifeTimeSource( "InverseLifeTimeSource", Float ) = 0
		_LifeTime( "LifeTime", Range( -1, 0 ) ) = 0
		[Toggle( _WHISPERONLY_ON )] _WhisperOnly( "WhisperOnly", Float ) = 0
		[Toggle( _USEVORONOID_ON )] _UseVoronoid( "UseVoronoid", Float ) = 0
		_VoronoidScale( "VoronoidScale", Float ) = 1
		_VoronoidStrength( "VoronoidStrength", Float ) = 0
		_DirectionalFade( "DirectionalFade", Float ) = 0
		_DirectionalFadeSmooth( "DirectionalFadeSmooth", Float ) = 1
		[KeywordEnum( Static,Dynamic )] _NoiseSpeedControl( "NoiseSpeedControl", Float ) = 0
		_ExternalNoiseOffset( "ExternalNoiseOffset", Vector ) = ( 0, 0, 0, 0 )


		[HideInInspector][NoScaleOffset] unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset] unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
	}

	SubShader
	{
		PackageRequirements
		{
			"com.unity.render-pipelines.universal": "[17.0,18.0]"
		}

		

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" "UniversalMaterialType"="Unlit" "ShaderGraphShader"="true" }

	LOD 0

		Cull Off

		HLSLINCLUDE
		#pragma target 2.0
		#pragma prefer_hlslcc gles
		// ensure rendering platforms toggle list is visible

		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
		#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"

		ENDHLSL

		
		Pass
		{
			Name "Sprite Unlit"
            Tags { "LightMode"="Universal2D" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _DISABLE_COLOR_TINT
			#define ASE_VERSION 19909
			#define ASE_SRP_VERSION 170300


			#if ( UNITY_VERSION >= 60010000 )
			#pragma multi_compile_instancing
			#endif

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ DEBUG_DISPLAY SKINNED_SPRITE

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX_NORMAL_OUTPUT
            #define FEATURES_GRAPH_VERTEX_TANGENT_OUTPUT
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX

			#define SHADERPASS SHADERPASS_SPRITEUNLIT

			#if ( UNITY_VERSION >= 60010000 )
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
			#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_WORLD_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_TEXTURE_COORDINATES0
			#define ASE_NEEDS_FRAG_TEXTURE_COORDINATES0
			#pragma shader_feature_local _NOISESPEEDCONTROL_STATIC _NOISESPEEDCONTROL_DYNAMIC
			#pragma shader_feature_local _WHISPERONLY_ON
			#pragma shader_feature_local _INVERSELIFETIMESOURCE_UV_W _INVERSELIFETIMESOURCE_VALUE
			#pragma shader_feature_local _USEVORONOID_ON


			half4 _RendererColor;

			sampler2D _NoiseTex;
			sampler2D _MainTex;
			sampler2D _FadeNoiseTex;
			float WHISPER_VALUE;
			float WHISPER_ON;
			UNITY_INSTANCING_BUFFER_START(AmplifyShadersParticlesRadiusWave)
				UNITY_DEFINE_INSTANCED_PROP(float2, _ExternalNoiseOffset)
				UNITY_DEFINE_INSTANCED_PROP(float, _LifeTime)
				UNITY_DEFINE_INSTANCED_PROP(float, _NoiseStrength)
				UNITY_DEFINE_INSTANCED_PROP(float, _OutterRingRadius)
				UNITY_DEFINE_INSTANCED_PROP(float, _InnerRingRadius)
			UNITY_INSTANCING_BUFFER_END(AmplifyShadersParticlesRadiusWave)
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			float2 _NoiseSpeed;
			float _EmissionScale;
			float _FadeRadius;
			float _FadePatternScale;
			float _FadeSmooth;
			float _InnerRingWidth;
			float _OutterRingWidth;
			float _DirectionalFadeSmooth;
			float _DirectionalFade;
			float _VoronoidStrength;
			float _Max;
			float _FadeLength;
			float _Min;
			float _NoiseTiling;
			float _NoiseMax;
			float _NoiseMin;
			float _EmissionShimering;
			float _ShimeringFreq;
			float _VoronoidScale;
			float _FadeStrength;
			CBUFFER_END


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_SKINNED_VERTEX_INPUTS
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D(_AlphaTex); SAMPLER(sampler_AlphaTex);
				float _EnableAlphaTexture;
			#endif

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash166( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi166( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0; int i, j;
						for ( j = -1; j <= 1; j++ )
						{
							for ( i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash166( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_SKINNED_VERTEX_COMPUTE(v);

				SetUpSpriteInstanceProperties();
				v.positionOS = UnityFlipSprite( v.positionOS, unity_SpriteProps.xy );

				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS = vertexValue;
				#else
					v.positionOS += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS);

				o.positionCS = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS;
				o.texCoord0 = v.uv0;
				o.color = v.color * _RendererColor * unity_SpriteColor;
				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float4 positionCS = IN.positionCS;
				float3 positionWS = IN.positionWS;

				float mulTime146 = _TimeParameters.x * _ShimeringFreq;
				float2 temp_cast_0 = (mulTime146).xx;
				float simplePerlin2D145 = snoise( temp_cast_0 );
				simplePerlin2D145 = simplePerlin2D145*0.5 + 0.5;
				float lerpResult147 = lerp( 1.0 , simplePerlin2D145 , _EmissionShimering);
				float2 speedDir209 = _NoiseSpeed;
				float2 appendResult19 = (float2(positionWS.x , positionWS.y));
				float2 temp_output_21_0 = ( appendResult19 * _NoiseTiling );
				float2 panner22 = ( 1.0 * _Time.y * speedDir209 + temp_output_21_0);
				float2 _ExternalNoiseOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_ExternalNoiseOffset);
				#if defined( _NOISESPEEDCONTROL_STATIC )
				float2 staticSwitch258 = panner22;
				#elif defined( _NOISESPEEDCONTROL_DYNAMIC )
				float2 staticSwitch258 = ( temp_output_21_0 + _ExternalNoiseOffset_Instance );
				#else
				float2 staticSwitch258 = panner22;
				#endif
				float2 noiseTiling136 = staticSwitch258;
				float smoothstepResult25 = smoothstep( _NoiseMin , _NoiseMax , tex2D( _NoiseTex, noiseTiling136 ).r);
				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 break12 = tex2D( _MainTex, uv_MainTex );
				float3 appendResult13 = (float3(break12.r , break12.g , break12.b));
				float2 uv_NoiseTex = IN.texCoord0.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float _LifeTime_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_LifeTime);
				#if defined( _INVERSELIFETIMESOURCE_UV_W )
				float staticSwitch152 = IN.texCoord0.xyz.z;
				#elif defined( _INVERSELIFETIMESOURCE_VALUE )
				float staticSwitch152 = _LifeTime_Instance;
				#else
				float staticSwitch152 = IN.texCoord0.xyz.z;
				#endif
				float _NoiseStrength_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_NoiseStrength);
				float2 texCoord35 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 radiusUV139 = ( texCoord35 - float2( 0.5,0.5 ) );
				float time166 = 1.0;
				float2 voronoiSmoothId166 = 0;
				float2 coords166 = noiseTiling136 * _VoronoidScale;
				float2 id166 = 0;
				float2 uv166 = 0;
				float voroi166 = voronoi166( coords166, time166, id166, uv166, 0, voronoiSmoothId166 );
				float smoothstepResult174 = smoothstep( 0.0 , 1.0 , voroi166);
				float2 texCoord208 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 break231 = speedDir209;
				float cos226 = cos( atan2( break231.y , break231.x ) );
				float sin226 = sin( atan2( break231.y , break231.x ) );
				float2 rotator226 = mul( texCoord208 - float2( 0.5,0.5 ) , float2x2( cos226 , -sin226 , sin226 , cos226 )) + float2( 0.5,0.5 );
				float smoothstepResult236 = smoothstep( _DirectionalFade , ( _DirectionalFade + _DirectionalFadeSmooth ) , (rotator226).x);
				#ifdef _USEVORONOID_ON
				float staticSwitch244 = smoothstepResult236;
				#else
				float staticSwitch244 = 0.0;
				#endif
				float directionalFade240 = staticSwitch244;
				float lerpResult250 = lerp( ( smoothstepResult174 * _VoronoidStrength ) , 0.0 , directionalFade240);
				#ifdef _USEVORONOID_ON
				float staticSwitch162 = lerpResult250;
				#else
				float staticSwitch162 = 0.0;
				#endif
				float voronoidSplit197 = staticSwitch162;
				float temp_output_205_0 = ( length( radiusUV139 ) + voronoidSplit197 );
				float _OutterRingRadius_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_OutterRingRadius);
				float temp_output_21_0_g24 = _OutterRingWidth;
				float temp_output_5_0_g24 = ( ( temp_output_205_0 - ( _OutterRingRadius_Instance - temp_output_21_0_g24 ) ) / temp_output_21_0_g24 );
				float temp_output_183_0 = saturate( ( ( tex2D( _NoiseTex, ( 1.0 * noiseTiling136 ) ).r * _NoiseStrength_Instance ) + -temp_output_5_0_g24 ) );
				float _InnerRingRadius_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_InnerRingRadius);
				float temp_output_21_0_g25 = _InnerRingWidth;
				float temp_output_5_0_g25 = ( ( temp_output_205_0 - ( _InnerRingRadius_Instance - temp_output_21_0_g25 ) ) / temp_output_21_0_g25 );
				float smoothstepResult47 = smoothstep( _Min , _Max , ( temp_output_183_0 - saturate( ( ( tex2D( _NoiseTex, ( 1.0 * noiseTiling136 ) ).r * _NoiseStrength_Instance ) + -temp_output_5_0_g25 ) ) ));
				float smoothstepResult143 = smoothstep( _Min , _Max , temp_output_183_0);
				float2 appendResult108 = (float2(positionWS.x , positionWS.y));
				float temp_output_21_0_g23 = _FadeLength;
				float temp_output_5_0_g23 = ( ( length( radiusUV139 ) - ( _FadeRadius - temp_output_21_0_g23 ) ) / temp_output_21_0_g23 );
				float smoothstepResult89 = smoothstep( 0.0 , -_FadeSmooth , ( ( tex2D( _FadeNoiseTex, ( _FadePatternScale * appendResult108 ) ).r * 1.0 ) + -temp_output_5_0_g23 ));
				float fadeNoise164 = ( saturate( ( smoothstepResult143 * smoothstepResult89 ) ) * _FadeStrength );
				float temp_output_53_0 = ( break12.a * saturate( ( tex2D( _NoiseTex, uv_NoiseTex ).r + staticSwitch152 ) ) * saturate( ( smoothstepResult47 + fadeNoise164 ) ) );
				float temp_output_10_0_g26 = 1.0;
				float lerpResult8_g26 = lerp( ( ( WHISPER_VALUE * temp_output_10_0_g26 ) + ( 1.0 - temp_output_10_0_g26 ) ) , ( WHISPER_VALUE * temp_output_10_0_g26 ) , WHISPER_ON);
				#ifdef _WHISPERONLY_ON
				float staticSwitch153 = ( temp_output_53_0 * saturate( lerpResult8_g26 ) );
				#else
				float staticSwitch153 = temp_output_53_0;
				#endif
				float4 appendResult155 = (float4(( ( ( _EmissionScale * lerpResult147 ) * smoothstepResult25 ) * appendResult13 ) , staticSwitch153));
				

				float4 Color = ( IN.color * appendResult155 );
				float AlphaClipThreshold = 0.5;

			#if defined( ALPHA_CLIP_THRESHOLD )
				clip( Color.a - AlphaClipThreshold );
			#endif

			#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, positionWS, positionCS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
			#endif

			#if ETC1_EXTERNAL_ALPHA
				float4 alpha = SAMPLE_TEXTURE2D(_AlphaTex, sampler_AlphaTex, IN.texCoord0.xy);
				Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture);
			#endif

			#if !defined( _DISABLE_COLOR_TINT )
				Color *= IN.color;
			#endif

				return Color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
            Name "Sprite Unlit Forward"
            Tags { "LightMode"="UniversalForward" }

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA

			

			HLSLPROGRAM

			#define _DISABLE_COLOR_TINT
			#define ASE_VERSION 19909
			#define ASE_SRP_VERSION 170300


			#if ( UNITY_VERSION >= 60010000 )
			#pragma multi_compile_instancing
			#endif

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ SKINNED_SPRITE
			#pragma multi_compile_fragment _ DEBUG_DISPLAY

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX_NORMAL_OUTPUT
            #define FEATURES_GRAPH_VERTEX_TANGENT_OUTPUT
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX

			#define SHADERPASS SHADERPASS_SPRITEFORWARD

			#if ( UNITY_VERSION >= 60010000 )
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
			#endif
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
			#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"

			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_WORLD_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_TEXTURE_COORDINATES0
			#define ASE_NEEDS_FRAG_TEXTURE_COORDINATES0
			#pragma shader_feature_local _NOISESPEEDCONTROL_STATIC _NOISESPEEDCONTROL_DYNAMIC
			#pragma shader_feature_local _WHISPERONLY_ON
			#pragma shader_feature_local _INVERSELIFETIMESOURCE_UV_W _INVERSELIFETIMESOURCE_VALUE
			#pragma shader_feature_local _USEVORONOID_ON


			half4 _RendererColor;

			sampler2D _NoiseTex;
			sampler2D _MainTex;
			sampler2D _FadeNoiseTex;
			float WHISPER_VALUE;
			float WHISPER_ON;
			UNITY_INSTANCING_BUFFER_START(AmplifyShadersParticlesRadiusWave)
				UNITY_DEFINE_INSTANCED_PROP(float2, _ExternalNoiseOffset)
				UNITY_DEFINE_INSTANCED_PROP(float, _LifeTime)
				UNITY_DEFINE_INSTANCED_PROP(float, _NoiseStrength)
				UNITY_DEFINE_INSTANCED_PROP(float, _OutterRingRadius)
				UNITY_DEFINE_INSTANCED_PROP(float, _InnerRingRadius)
			UNITY_INSTANCING_BUFFER_END(AmplifyShadersParticlesRadiusWave)
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			float2 _NoiseSpeed;
			float _EmissionScale;
			float _FadeRadius;
			float _FadePatternScale;
			float _FadeSmooth;
			float _InnerRingWidth;
			float _OutterRingWidth;
			float _DirectionalFadeSmooth;
			float _DirectionalFade;
			float _VoronoidStrength;
			float _Max;
			float _FadeLength;
			float _Min;
			float _NoiseTiling;
			float _NoiseMax;
			float _NoiseMin;
			float _EmissionShimering;
			float _ShimeringFreq;
			float _VoronoidScale;
			float _FadeStrength;
			CBUFFER_END


			struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_SKINNED_VERTEX_INPUTS
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash166( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi166( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0; int i, j;
						for ( j = -1; j <= 1; j++ )
						{
							for ( i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash166( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_SKINNED_VERTEX_COMPUTE(v);

				SetUpSpriteInstanceProperties();
				v.positionOS = UnityFlipSprite( v.positionOS, unity_SpriteProps.xy );

				

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS = vertexValue;
				#else
					v.positionOS += vertexValue;
				#endif
				v.normal = v.normal;
				v.tangent.xyz = v.tangent.xyz;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS);

				o.positionCS = vertexInput.positionCS;
				o.positionWS = vertexInput.positionWS;
				o.texCoord0 = v.uv0;
				o.color = v.color * _RendererColor * unity_SpriteColor;
				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float4 positionCS = IN.positionCS;
				float3 positionWS = IN.positionWS;

				float mulTime146 = _TimeParameters.x * _ShimeringFreq;
				float2 temp_cast_0 = (mulTime146).xx;
				float simplePerlin2D145 = snoise( temp_cast_0 );
				simplePerlin2D145 = simplePerlin2D145*0.5 + 0.5;
				float lerpResult147 = lerp( 1.0 , simplePerlin2D145 , _EmissionShimering);
				float2 speedDir209 = _NoiseSpeed;
				float2 appendResult19 = (float2(positionWS.x , positionWS.y));
				float2 temp_output_21_0 = ( appendResult19 * _NoiseTiling );
				float2 panner22 = ( 1.0 * _Time.y * speedDir209 + temp_output_21_0);
				float2 _ExternalNoiseOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_ExternalNoiseOffset);
				#if defined( _NOISESPEEDCONTROL_STATIC )
				float2 staticSwitch258 = panner22;
				#elif defined( _NOISESPEEDCONTROL_DYNAMIC )
				float2 staticSwitch258 = ( temp_output_21_0 + _ExternalNoiseOffset_Instance );
				#else
				float2 staticSwitch258 = panner22;
				#endif
				float2 noiseTiling136 = staticSwitch258;
				float smoothstepResult25 = smoothstep( _NoiseMin , _NoiseMax , tex2D( _NoiseTex, noiseTiling136 ).r);
				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 break12 = tex2D( _MainTex, uv_MainTex );
				float3 appendResult13 = (float3(break12.r , break12.g , break12.b));
				float2 uv_NoiseTex = IN.texCoord0.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float _LifeTime_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_LifeTime);
				#if defined( _INVERSELIFETIMESOURCE_UV_W )
				float staticSwitch152 = IN.texCoord0.xyz.z;
				#elif defined( _INVERSELIFETIMESOURCE_VALUE )
				float staticSwitch152 = _LifeTime_Instance;
				#else
				float staticSwitch152 = IN.texCoord0.xyz.z;
				#endif
				float _NoiseStrength_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_NoiseStrength);
				float2 texCoord35 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 radiusUV139 = ( texCoord35 - float2( 0.5,0.5 ) );
				float time166 = 1.0;
				float2 voronoiSmoothId166 = 0;
				float2 coords166 = noiseTiling136 * _VoronoidScale;
				float2 id166 = 0;
				float2 uv166 = 0;
				float voroi166 = voronoi166( coords166, time166, id166, uv166, 0, voronoiSmoothId166 );
				float smoothstepResult174 = smoothstep( 0.0 , 1.0 , voroi166);
				float2 texCoord208 = IN.texCoord0.xy * float2( 1,1 ) + float2( 0,0 );
				float2 break231 = speedDir209;
				float cos226 = cos( atan2( break231.y , break231.x ) );
				float sin226 = sin( atan2( break231.y , break231.x ) );
				float2 rotator226 = mul( texCoord208 - float2( 0.5,0.5 ) , float2x2( cos226 , -sin226 , sin226 , cos226 )) + float2( 0.5,0.5 );
				float smoothstepResult236 = smoothstep( _DirectionalFade , ( _DirectionalFade + _DirectionalFadeSmooth ) , (rotator226).x);
				#ifdef _USEVORONOID_ON
				float staticSwitch244 = smoothstepResult236;
				#else
				float staticSwitch244 = 0.0;
				#endif
				float directionalFade240 = staticSwitch244;
				float lerpResult250 = lerp( ( smoothstepResult174 * _VoronoidStrength ) , 0.0 , directionalFade240);
				#ifdef _USEVORONOID_ON
				float staticSwitch162 = lerpResult250;
				#else
				float staticSwitch162 = 0.0;
				#endif
				float voronoidSplit197 = staticSwitch162;
				float temp_output_205_0 = ( length( radiusUV139 ) + voronoidSplit197 );
				float _OutterRingRadius_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_OutterRingRadius);
				float temp_output_21_0_g24 = _OutterRingWidth;
				float temp_output_5_0_g24 = ( ( temp_output_205_0 - ( _OutterRingRadius_Instance - temp_output_21_0_g24 ) ) / temp_output_21_0_g24 );
				float temp_output_183_0 = saturate( ( ( tex2D( _NoiseTex, ( 1.0 * noiseTiling136 ) ).r * _NoiseStrength_Instance ) + -temp_output_5_0_g24 ) );
				float _InnerRingRadius_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_InnerRingRadius);
				float temp_output_21_0_g25 = _InnerRingWidth;
				float temp_output_5_0_g25 = ( ( temp_output_205_0 - ( _InnerRingRadius_Instance - temp_output_21_0_g25 ) ) / temp_output_21_0_g25 );
				float smoothstepResult47 = smoothstep( _Min , _Max , ( temp_output_183_0 - saturate( ( ( tex2D( _NoiseTex, ( 1.0 * noiseTiling136 ) ).r * _NoiseStrength_Instance ) + -temp_output_5_0_g25 ) ) ));
				float smoothstepResult143 = smoothstep( _Min , _Max , temp_output_183_0);
				float2 appendResult108 = (float2(positionWS.x , positionWS.y));
				float temp_output_21_0_g23 = _FadeLength;
				float temp_output_5_0_g23 = ( ( length( radiusUV139 ) - ( _FadeRadius - temp_output_21_0_g23 ) ) / temp_output_21_0_g23 );
				float smoothstepResult89 = smoothstep( 0.0 , -_FadeSmooth , ( ( tex2D( _FadeNoiseTex, ( _FadePatternScale * appendResult108 ) ).r * 1.0 ) + -temp_output_5_0_g23 ));
				float fadeNoise164 = ( saturate( ( smoothstepResult143 * smoothstepResult89 ) ) * _FadeStrength );
				float temp_output_53_0 = ( break12.a * saturate( ( tex2D( _NoiseTex, uv_NoiseTex ).r + staticSwitch152 ) ) * saturate( ( smoothstepResult47 + fadeNoise164 ) ) );
				float temp_output_10_0_g26 = 1.0;
				float lerpResult8_g26 = lerp( ( ( WHISPER_VALUE * temp_output_10_0_g26 ) + ( 1.0 - temp_output_10_0_g26 ) ) , ( WHISPER_VALUE * temp_output_10_0_g26 ) , WHISPER_ON);
				#ifdef _WHISPERONLY_ON
				float staticSwitch153 = ( temp_output_53_0 * saturate( lerpResult8_g26 ) );
				#else
				float staticSwitch153 = temp_output_53_0;
				#endif
				float4 appendResult155 = (float4(( ( ( _EmissionScale * lerpResult147 ) * smoothstepResult25 ) * appendResult13 ) , staticSwitch153));
				

				float4 Color = ( IN.color * appendResult155 );
				float AlphaClipThreshold = 0.5;

			#if defined( ALPHA_CLIP_THRESHOLD )
				clip( Color.a - AlphaClipThreshold );
			#endif

			#if defined(DEBUG_DISPLAY)
				SurfaceData2D surfaceData;
				InitializeSurfaceData(Color.rgb, Color.a, surfaceData);
				InputData2D inputData;
				InitializeInputData(positionWS.xy, half2(IN.texCoord0.xy), inputData);
				half4 debugColor = 0;

				SETUP_DEBUG_DATA_2D(inputData, positionWS, positionCS);

				if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
				{
					return debugColor;
				}
			#endif

			#if ETC1_EXTERNAL_ALPHA
				float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
				Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
			#endif

			#if !defined( _DISABLE_COLOR_TINT )
				Color *= IN.color;
			#endif

				return Color;
			}

			ENDHLSL
		}

		
        Pass
        {
			
            Name "SceneSelectionPass"
            Tags { "LightMode"="SceneSelectionPass" }

            Cull Off

            HLSLPROGRAM

			#define _DISABLE_COLOR_TINT
			#define ASE_VERSION 19909
			#define ASE_SRP_VERSION 170300


			#if ( UNITY_VERSION >= 60010000 )
			#pragma multi_compile_instancing
			#endif

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ DEBUG_DISPLAY SKINNED_SPRITE

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX_NORMAL_OUTPUT
            #define FEATURES_GRAPH_VERTEX_TANGENT_OUTPUT
            #define FEATURES_GRAPH_VERTEX

            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENESELECTIONPASS 1

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
			#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

			#define ASE_NEEDS_TEXTURE_COORDINATES0
			#define ASE_NEEDS_FRAG_TEXTURE_COORDINATES0
			#pragma shader_feature_local _NOISESPEEDCONTROL_STATIC _NOISESPEEDCONTROL_DYNAMIC
			#pragma shader_feature_local _WHISPERONLY_ON
			#pragma shader_feature_local _INVERSELIFETIMESOURCE_UV_W _INVERSELIFETIMESOURCE_VALUE
			#pragma shader_feature_local _USEVORONOID_ON


			sampler2D _NoiseTex;
			sampler2D _MainTex;
			sampler2D _FadeNoiseTex;
			float WHISPER_VALUE;
			float WHISPER_ON;
			UNITY_INSTANCING_BUFFER_START(AmplifyShadersParticlesRadiusWave)
				UNITY_DEFINE_INSTANCED_PROP(float2, _ExternalNoiseOffset)
				UNITY_DEFINE_INSTANCED_PROP(float, _LifeTime)
				UNITY_DEFINE_INSTANCED_PROP(float, _NoiseStrength)
				UNITY_DEFINE_INSTANCED_PROP(float, _OutterRingRadius)
				UNITY_DEFINE_INSTANCED_PROP(float, _InnerRingRadius)
			UNITY_INSTANCING_BUFFER_END(AmplifyShadersParticlesRadiusWave)
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			float2 _NoiseSpeed;
			float _EmissionScale;
			float _FadeRadius;
			float _FadePatternScale;
			float _FadeSmooth;
			float _InnerRingWidth;
			float _OutterRingWidth;
			float _DirectionalFadeSmooth;
			float _DirectionalFade;
			float _VoronoidStrength;
			float _Max;
			float _FadeLength;
			float _Min;
			float _NoiseTiling;
			float _NoiseMax;
			float _NoiseMin;
			float _EmissionShimering;
			float _ShimeringFreq;
			float _VoronoidScale;
			float _FadeStrength;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_SKINNED_VERTEX_INPUTS
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

            int _ObjectId;
            int _PassValue;

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash166( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi166( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0; int i, j;
						for ( j = -1; j <= 1; j++ )
						{
							for ( i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash166( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			VertexOutput vert(VertexInput v )
			{
				VertexOutput o = (VertexOutput)0;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_SKINNED_VERTEX_COMPUTE(v);

				SetUpSpriteInstanceProperties();
				v.positionOS = UnityFlipSprite( v.positionOS, unity_SpriteProps.xy );

				float3 ase_positionWS = TransformObjectToWorld( ( v.positionOS ).xyz );
				o.ase_texcoord.xyz = ase_positionWS;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord1.xyz = v.ase_texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS = vertexValue;
				#else
					v.positionOS += vertexValue;
				#endif

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS);

				o.positionCS = vertexInput.positionCS;
				return o;
			}

			half4 frag(VertexOutput IN) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float mulTime146 = _TimeParameters.x * _ShimeringFreq;
				float2 temp_cast_0 = (mulTime146).xx;
				float simplePerlin2D145 = snoise( temp_cast_0 );
				simplePerlin2D145 = simplePerlin2D145*0.5 + 0.5;
				float lerpResult147 = lerp( 1.0 , simplePerlin2D145 , _EmissionShimering);
				float2 speedDir209 = _NoiseSpeed;
				float3 ase_positionWS = IN.ase_texcoord.xyz;
				float2 appendResult19 = (float2(ase_positionWS.x , ase_positionWS.y));
				float2 temp_output_21_0 = ( appendResult19 * _NoiseTiling );
				float2 panner22 = ( 1.0 * _Time.y * speedDir209 + temp_output_21_0);
				float2 _ExternalNoiseOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_ExternalNoiseOffset);
				#if defined( _NOISESPEEDCONTROL_STATIC )
				float2 staticSwitch258 = panner22;
				#elif defined( _NOISESPEEDCONTROL_DYNAMIC )
				float2 staticSwitch258 = ( temp_output_21_0 + _ExternalNoiseOffset_Instance );
				#else
				float2 staticSwitch258 = panner22;
				#endif
				float2 noiseTiling136 = staticSwitch258;
				float smoothstepResult25 = smoothstep( _NoiseMin , _NoiseMax , tex2D( _NoiseTex, noiseTiling136 ).r);
				float2 uv_MainTex = IN.ase_texcoord1.xyz.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 break12 = tex2D( _MainTex, uv_MainTex );
				float3 appendResult13 = (float3(break12.r , break12.g , break12.b));
				float2 uv_NoiseTex = IN.ase_texcoord1.xyz.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float _LifeTime_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_LifeTime);
				#if defined( _INVERSELIFETIMESOURCE_UV_W )
				float staticSwitch152 = IN.ase_texcoord1.xyz.z;
				#elif defined( _INVERSELIFETIMESOURCE_VALUE )
				float staticSwitch152 = _LifeTime_Instance;
				#else
				float staticSwitch152 = IN.ase_texcoord1.xyz.z;
				#endif
				float _NoiseStrength_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_NoiseStrength);
				float2 texCoord35 = IN.ase_texcoord1.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 radiusUV139 = ( texCoord35 - float2( 0.5,0.5 ) );
				float time166 = 1.0;
				float2 voronoiSmoothId166 = 0;
				float2 coords166 = noiseTiling136 * _VoronoidScale;
				float2 id166 = 0;
				float2 uv166 = 0;
				float voroi166 = voronoi166( coords166, time166, id166, uv166, 0, voronoiSmoothId166 );
				float smoothstepResult174 = smoothstep( 0.0 , 1.0 , voroi166);
				float2 texCoord208 = IN.ase_texcoord1.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 break231 = speedDir209;
				float cos226 = cos( atan2( break231.y , break231.x ) );
				float sin226 = sin( atan2( break231.y , break231.x ) );
				float2 rotator226 = mul( texCoord208 - float2( 0.5,0.5 ) , float2x2( cos226 , -sin226 , sin226 , cos226 )) + float2( 0.5,0.5 );
				float smoothstepResult236 = smoothstep( _DirectionalFade , ( _DirectionalFade + _DirectionalFadeSmooth ) , (rotator226).x);
				#ifdef _USEVORONOID_ON
				float staticSwitch244 = smoothstepResult236;
				#else
				float staticSwitch244 = 0.0;
				#endif
				float directionalFade240 = staticSwitch244;
				float lerpResult250 = lerp( ( smoothstepResult174 * _VoronoidStrength ) , 0.0 , directionalFade240);
				#ifdef _USEVORONOID_ON
				float staticSwitch162 = lerpResult250;
				#else
				float staticSwitch162 = 0.0;
				#endif
				float voronoidSplit197 = staticSwitch162;
				float temp_output_205_0 = ( length( radiusUV139 ) + voronoidSplit197 );
				float _OutterRingRadius_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_OutterRingRadius);
				float temp_output_21_0_g24 = _OutterRingWidth;
				float temp_output_5_0_g24 = ( ( temp_output_205_0 - ( _OutterRingRadius_Instance - temp_output_21_0_g24 ) ) / temp_output_21_0_g24 );
				float temp_output_183_0 = saturate( ( ( tex2D( _NoiseTex, ( 1.0 * noiseTiling136 ) ).r * _NoiseStrength_Instance ) + -temp_output_5_0_g24 ) );
				float _InnerRingRadius_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_InnerRingRadius);
				float temp_output_21_0_g25 = _InnerRingWidth;
				float temp_output_5_0_g25 = ( ( temp_output_205_0 - ( _InnerRingRadius_Instance - temp_output_21_0_g25 ) ) / temp_output_21_0_g25 );
				float smoothstepResult47 = smoothstep( _Min , _Max , ( temp_output_183_0 - saturate( ( ( tex2D( _NoiseTex, ( 1.0 * noiseTiling136 ) ).r * _NoiseStrength_Instance ) + -temp_output_5_0_g25 ) ) ));
				float smoothstepResult143 = smoothstep( _Min , _Max , temp_output_183_0);
				float2 appendResult108 = (float2(ase_positionWS.x , ase_positionWS.y));
				float temp_output_21_0_g23 = _FadeLength;
				float temp_output_5_0_g23 = ( ( length( radiusUV139 ) - ( _FadeRadius - temp_output_21_0_g23 ) ) / temp_output_21_0_g23 );
				float smoothstepResult89 = smoothstep( 0.0 , -_FadeSmooth , ( ( tex2D( _FadeNoiseTex, ( _FadePatternScale * appendResult108 ) ).r * 1.0 ) + -temp_output_5_0_g23 ));
				float fadeNoise164 = ( saturate( ( smoothstepResult143 * smoothstepResult89 ) ) * _FadeStrength );
				float temp_output_53_0 = ( break12.a * saturate( ( tex2D( _NoiseTex, uv_NoiseTex ).r + staticSwitch152 ) ) * saturate( ( smoothstepResult47 + fadeNoise164 ) ) );
				float temp_output_10_0_g26 = 1.0;
				float lerpResult8_g26 = lerp( ( ( WHISPER_VALUE * temp_output_10_0_g26 ) + ( 1.0 - temp_output_10_0_g26 ) ) , ( WHISPER_VALUE * temp_output_10_0_g26 ) , WHISPER_ON);
				#ifdef _WHISPERONLY_ON
				float staticSwitch153 = ( temp_output_53_0 * saturate( lerpResult8_g26 ) );
				#else
				float staticSwitch153 = temp_output_53_0;
				#endif
				float4 appendResult155 = (float4(( ( ( _EmissionScale * lerpResult147 ) * smoothstepResult25 ) * appendResult13 ) , staticSwitch153));
				

				float4 Color = ( IN.ase_color * appendResult155 );
				float AlphaClipThreshold = 0.5;

				#if defined( ALPHA_CLIP_THRESHOLD )
					clip( Color.a - AlphaClipThreshold );
				#endif

				return half4(_ObjectId, _PassValue, 1.0, 1.0);
			}

            ENDHLSL
        }

		
        Pass
        {
			
            Name "ScenePickingPass"
            Tags { "LightMode"="Picking" }

			Cull Off

            HLSLPROGRAM

			#define _DISABLE_COLOR_TINT
			#define ASE_VERSION 19909
			#define ASE_SRP_VERSION 170300


			#if ( UNITY_VERSION >= 60010000 )
			#pragma multi_compile_instancing
			#endif

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ DEBUG_DISPLAY SKINNED_SPRITE

            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define FEATURES_GRAPH_VERTEX_NORMAL_OUTPUT
            #define FEATURES_GRAPH_VERTEX_TANGENT_OUTPUT
            #define FEATURES_GRAPH_VERTEX

            #define SHADERPASS SHADERPASS_DEPTHONLY
			#define SCENEPICKINGPASS 1

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
			#include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/DebugMipmapStreamingMacros.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        	#define ASE_NEEDS_TEXTURE_COORDINATES0
        	#define ASE_NEEDS_FRAG_TEXTURE_COORDINATES0
        	#pragma shader_feature_local _NOISESPEEDCONTROL_STATIC _NOISESPEEDCONTROL_DYNAMIC
        	#pragma shader_feature_local _WHISPERONLY_ON
        	#pragma shader_feature_local _INVERSELIFETIMESOURCE_UV_W _INVERSELIFETIMESOURCE_VALUE
        	#pragma shader_feature_local _USEVORONOID_ON


			sampler2D _NoiseTex;
			sampler2D _MainTex;
			sampler2D _FadeNoiseTex;
			float WHISPER_VALUE;
			float WHISPER_ON;
			UNITY_INSTANCING_BUFFER_START(AmplifyShadersParticlesRadiusWave)
				UNITY_DEFINE_INSTANCED_PROP(float2, _ExternalNoiseOffset)
				UNITY_DEFINE_INSTANCED_PROP(float, _LifeTime)
				UNITY_DEFINE_INSTANCED_PROP(float, _NoiseStrength)
				UNITY_DEFINE_INSTANCED_PROP(float, _OutterRingRadius)
				UNITY_DEFINE_INSTANCED_PROP(float, _InnerRingRadius)
			UNITY_INSTANCING_BUFFER_END(AmplifyShadersParticlesRadiusWave)
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _NoiseTex_ST;
			float2 _NoiseSpeed;
			float _EmissionScale;
			float _FadeRadius;
			float _FadePatternScale;
			float _FadeSmooth;
			float _InnerRingWidth;
			float _OutterRingWidth;
			float _DirectionalFadeSmooth;
			float _DirectionalFade;
			float _VoronoidStrength;
			float _Max;
			float _FadeLength;
			float _Min;
			float _NoiseTiling;
			float _NoiseMax;
			float _NoiseMin;
			float _EmissionShimering;
			float _ShimeringFreq;
			float _VoronoidScale;
			float _FadeStrength;
			CBUFFER_END


            struct VertexInput
			{
				float3 positionOS : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_SKINNED_VERTEX_INPUTS
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 positionCS : SV_POSITION;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

            float4 _SelectionID;

			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash166( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi166( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0; int i, j;
						for ( j = -1; j <= 1; j++ )
						{
							for ( i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash166( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			VertexOutput vert(VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_SKINNED_VERTEX_COMPUTE(v);

				SetUpSpriteInstanceProperties();
				v.positionOS = UnityFlipSprite( v.positionOS, unity_SpriteProps.xy );

				float3 ase_positionWS = TransformObjectToWorld( ( v.positionOS ).xyz );
				o.ase_texcoord.xyz = ase_positionWS;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord1.xyz = v.ase_texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;

				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.positionOS;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.positionOS = vertexValue;
				#else
					v.positionOS += vertexValue;
				#endif

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.positionOS);

				o.positionCS = vertexInput.positionCS;
				return o;
			}

			half4 frag(VertexOutput IN ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				float mulTime146 = _TimeParameters.x * _ShimeringFreq;
				float2 temp_cast_0 = (mulTime146).xx;
				float simplePerlin2D145 = snoise( temp_cast_0 );
				simplePerlin2D145 = simplePerlin2D145*0.5 + 0.5;
				float lerpResult147 = lerp( 1.0 , simplePerlin2D145 , _EmissionShimering);
				float2 speedDir209 = _NoiseSpeed;
				float3 ase_positionWS = IN.ase_texcoord.xyz;
				float2 appendResult19 = (float2(ase_positionWS.x , ase_positionWS.y));
				float2 temp_output_21_0 = ( appendResult19 * _NoiseTiling );
				float2 panner22 = ( 1.0 * _Time.y * speedDir209 + temp_output_21_0);
				float2 _ExternalNoiseOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_ExternalNoiseOffset);
				#if defined( _NOISESPEEDCONTROL_STATIC )
				float2 staticSwitch258 = panner22;
				#elif defined( _NOISESPEEDCONTROL_DYNAMIC )
				float2 staticSwitch258 = ( temp_output_21_0 + _ExternalNoiseOffset_Instance );
				#else
				float2 staticSwitch258 = panner22;
				#endif
				float2 noiseTiling136 = staticSwitch258;
				float smoothstepResult25 = smoothstep( _NoiseMin , _NoiseMax , tex2D( _NoiseTex, noiseTiling136 ).r);
				float2 uv_MainTex = IN.ase_texcoord1.xyz.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 break12 = tex2D( _MainTex, uv_MainTex );
				float3 appendResult13 = (float3(break12.r , break12.g , break12.b));
				float2 uv_NoiseTex = IN.ase_texcoord1.xyz.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
				float _LifeTime_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_LifeTime);
				#if defined( _INVERSELIFETIMESOURCE_UV_W )
				float staticSwitch152 = IN.ase_texcoord1.xyz.z;
				#elif defined( _INVERSELIFETIMESOURCE_VALUE )
				float staticSwitch152 = _LifeTime_Instance;
				#else
				float staticSwitch152 = IN.ase_texcoord1.xyz.z;
				#endif
				float _NoiseStrength_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_NoiseStrength);
				float2 texCoord35 = IN.ase_texcoord1.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 radiusUV139 = ( texCoord35 - float2( 0.5,0.5 ) );
				float time166 = 1.0;
				float2 voronoiSmoothId166 = 0;
				float2 coords166 = noiseTiling136 * _VoronoidScale;
				float2 id166 = 0;
				float2 uv166 = 0;
				float voroi166 = voronoi166( coords166, time166, id166, uv166, 0, voronoiSmoothId166 );
				float smoothstepResult174 = smoothstep( 0.0 , 1.0 , voroi166);
				float2 texCoord208 = IN.ase_texcoord1.xyz.xy * float2( 1,1 ) + float2( 0,0 );
				float2 break231 = speedDir209;
				float cos226 = cos( atan2( break231.y , break231.x ) );
				float sin226 = sin( atan2( break231.y , break231.x ) );
				float2 rotator226 = mul( texCoord208 - float2( 0.5,0.5 ) , float2x2( cos226 , -sin226 , sin226 , cos226 )) + float2( 0.5,0.5 );
				float smoothstepResult236 = smoothstep( _DirectionalFade , ( _DirectionalFade + _DirectionalFadeSmooth ) , (rotator226).x);
				#ifdef _USEVORONOID_ON
				float staticSwitch244 = smoothstepResult236;
				#else
				float staticSwitch244 = 0.0;
				#endif
				float directionalFade240 = staticSwitch244;
				float lerpResult250 = lerp( ( smoothstepResult174 * _VoronoidStrength ) , 0.0 , directionalFade240);
				#ifdef _USEVORONOID_ON
				float staticSwitch162 = lerpResult250;
				#else
				float staticSwitch162 = 0.0;
				#endif
				float voronoidSplit197 = staticSwitch162;
				float temp_output_205_0 = ( length( radiusUV139 ) + voronoidSplit197 );
				float _OutterRingRadius_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_OutterRingRadius);
				float temp_output_21_0_g24 = _OutterRingWidth;
				float temp_output_5_0_g24 = ( ( temp_output_205_0 - ( _OutterRingRadius_Instance - temp_output_21_0_g24 ) ) / temp_output_21_0_g24 );
				float temp_output_183_0 = saturate( ( ( tex2D( _NoiseTex, ( 1.0 * noiseTiling136 ) ).r * _NoiseStrength_Instance ) + -temp_output_5_0_g24 ) );
				float _InnerRingRadius_Instance = UNITY_ACCESS_INSTANCED_PROP(AmplifyShadersParticlesRadiusWave,_InnerRingRadius);
				float temp_output_21_0_g25 = _InnerRingWidth;
				float temp_output_5_0_g25 = ( ( temp_output_205_0 - ( _InnerRingRadius_Instance - temp_output_21_0_g25 ) ) / temp_output_21_0_g25 );
				float smoothstepResult47 = smoothstep( _Min , _Max , ( temp_output_183_0 - saturate( ( ( tex2D( _NoiseTex, ( 1.0 * noiseTiling136 ) ).r * _NoiseStrength_Instance ) + -temp_output_5_0_g25 ) ) ));
				float smoothstepResult143 = smoothstep( _Min , _Max , temp_output_183_0);
				float2 appendResult108 = (float2(ase_positionWS.x , ase_positionWS.y));
				float temp_output_21_0_g23 = _FadeLength;
				float temp_output_5_0_g23 = ( ( length( radiusUV139 ) - ( _FadeRadius - temp_output_21_0_g23 ) ) / temp_output_21_0_g23 );
				float smoothstepResult89 = smoothstep( 0.0 , -_FadeSmooth , ( ( tex2D( _FadeNoiseTex, ( _FadePatternScale * appendResult108 ) ).r * 1.0 ) + -temp_output_5_0_g23 ));
				float fadeNoise164 = ( saturate( ( smoothstepResult143 * smoothstepResult89 ) ) * _FadeStrength );
				float temp_output_53_0 = ( break12.a * saturate( ( tex2D( _NoiseTex, uv_NoiseTex ).r + staticSwitch152 ) ) * saturate( ( smoothstepResult47 + fadeNoise164 ) ) );
				float temp_output_10_0_g26 = 1.0;
				float lerpResult8_g26 = lerp( ( ( WHISPER_VALUE * temp_output_10_0_g26 ) + ( 1.0 - temp_output_10_0_g26 ) ) , ( WHISPER_VALUE * temp_output_10_0_g26 ) , WHISPER_ON);
				#ifdef _WHISPERONLY_ON
				float staticSwitch153 = ( temp_output_53_0 * saturate( lerpResult8_g26 ) );
				#else
				float staticSwitch153 = temp_output_53_0;
				#endif
				float4 appendResult155 = (float4(( ( ( _EmissionScale * lerpResult147 ) * smoothstepResult25 ) * appendResult13 ) , staticSwitch153));
				

				float4 Color = ( IN.ase_color * appendResult155 );
				float AlphaClipThreshold = 0.5;

				#if defined( ALPHA_CLIP_THRESHOLD )
					clip( Color.a - AlphaClipThreshold );
				#endif

				return unity_SelectionID;
			}

            ENDHLSL
        }
		
	}
	

	

	CustomEditor "AmplifyShaderEditor.MaterialInspector"
	FallBack "Hidden/Shader Graph/FallbackError"
	
	Fallback "Hidden/InternalErrorShader"
}
/*ASEBEGIN
Version=19909
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;210;-5536,2144;Inherit;False;209;speedDir;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;231;-5360,2144;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;208;-5312,1984;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ATan2OpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;251;-5248,2144;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;226;-4992,1984;Inherit;True;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;238;-4816,2256;Inherit;False;Property;_DirectionalFadeSmooth;DirectionalFadeSmooth;29;0;Create;True;0;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;237;-4752,2176;Inherit;False;Property;_DirectionalFade;DirectionalFade;28;0;Create;True;0;0;0;False;0;False;0;-0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;239;-4464,2224;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;235;-4736,1984;Inherit;True;True;False;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;236;-4288,2048;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0.4;False;2;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;245;-4256,1968;Inherit;False;Constant;_Float2;Float 2;31;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;168;-4432,128;Inherit;False;136;noiseTiling;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;171;-4432,208;Inherit;False;Property;_VoronoidScale;VoronoidScale;26;0;Create;True;0;0;0;False;0;False;1;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;166;-4240,128;Inherit;False;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;10;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;244;-4080,2032;Inherit;False;Property;_UseVoronoid1;UseVoronoid;25;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Reference;162;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;174;-4048,128;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;207;-4080,256;Inherit;False;Property;_VoronoidStrength;VoronoidStrength;27;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;240;-3824,2032;Inherit;False;directionalFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;35;-4448.839,1566.141;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;206;-3856,176;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;241;-3920,336;Inherit;False;240;directionalFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;36;-4179.772,1567.221;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;250;-3680,240;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;196;-3680,128;Inherit;False;Constant;_Float1;Float 1;28;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;139;-3920,1568;Inherit;False;radiusUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;162;-3504,128;Inherit;False;Property;_UseVoronoid;UseVoronoid;25;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;109;-2448,2608;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;140;-3600,1360;Inherit;False;139;radiusUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;197;-3248,128;Inherit;False;voronoidSplit;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;94;-1456,2976;Inherit;False;Property;_FadeSmooth;FadeSmooth;19;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;141;-2176,3008;Inherit;False;Property;_FadeRadius;FadeRadius;17;0;Create;True;0;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;93;-2144,3088;Inherit;False;Property;_FadeLength;FadeLength;18;0;Create;True;0;0;0;False;0;False;0.1;0.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;106;-2240,2848;Inherit;False;Property;_FadePatternScale;FadePatternScale;21;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;107;-2496,2752;Inherit;True;Property;_FadeNoiseTex;FadeNoiseTex;16;0;Create;True;0;0;0;False;0;False;e77e50664873809498937c7f032f4658;d87b3c91ca4427d43b327de44ec644b6;False;white;Auto;Texture2D;False;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;108;-2272,2624;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;161;-2192,2928;Inherit;False;139;radiusUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;201;-1280,1840;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;18;-5728,672;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LengthOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;180;-3408,1360;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;202;-3488,1504;Inherit;False;197;voronoidSplit;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;195;-2528,128;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.NegateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;95;-1280,2976;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;157;-1744,2720;Inherit;True;SRadiusDissolve;-1;;22;a318621bd65470b43ac73e4fd821eb8a;0;7;27;FLOAT;1;False;22;FLOAT2;0,0;False;17;SAMPLER2D;;False;18;FLOAT;1;False;19;FLOAT2;0,0;False;20;FLOAT;0.5;False;21;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;199;-1520,2400;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;200;-1360,2304;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;198;-1424,2352;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;24;-5616,976;Inherit;False;Property;_NoiseSpeed;NoiseSpeed;6;0;Create;True;0;0;0;False;0;False;1,1;0,-0.15;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;19;-5520,688;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;20;-5568,832;Inherit;False;Property;_NoiseTiling;NoiseTiling;5;0;Create;True;0;0;0;False;0;False;1;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;138;-3344,1232;Inherit;False;136;noiseTiling;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;160;-3328,1152;Inherit;False;InstancedProperty;_NoiseStrength;NoiseStrength;4;0;Create;True;0;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;194;-2416,144;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;205;-3184,1360;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;191;-2720,1424;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SmoothstepOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;89;-1168,2704;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;-26.35;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;143;-1152,2416;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;209;-5424,976;Inherit;False;speedDir;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;21;-5360,736;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;262;-5408,1072;Inherit;False;InstancedProperty;_ExternalNoiseOffset;ExternalNoiseOffset;31;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;33;-3088,880;Inherit;False;InstancedProperty;_OutterRingRadius;OutterRingRadius;10;0;Create;True;0;0;0;False;0;False;0.2;0.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;34;-3120,976;Inherit;False;Property;_OutterRingWidth;OutterRingWidth;11;0;Create;True;0;0;0;False;0;False;0.03;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;187;-2464,912;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;188;-2464,992;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;193;-2464,1040;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;248;-2464,944;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;101;-720,2416;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;186;-2528,1408;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;190;-2528,1344;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;192;-2528,1440;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;134;-2992,1568;Inherit;False;InstancedProperty;_InnerRingRadius;InnerRingRadius;12;0;Create;True;0;0;0;False;0;False;0.2;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;135;-3008,1648;Inherit;False;Property;_InnerRingWidth;InnerRingWidth;13;0;Create;True;0;0;0;False;0;False;0.2;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;249;-2528,1376;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;22;-4800,736;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.2,0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;261;-4800,896;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;179;-2192,976;Inherit;True;SGradientDissolve;-1;;24;e27ad990a2429b34d8f406fd90200dba;1,26,0;7;29;FLOAT;1;False;24;FLOAT;0;False;22;FLOAT2;0,0;False;17;SAMPLER2D;;False;18;FLOAT;1;False;20;FLOAT;0.5;False;21;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;102;-352,2416;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;104;-368,2544;Inherit;False;Property;_FadeStrength;FadeStrength;20;0;Create;True;0;0;0;False;0;False;1;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;181;-2192,1200;Inherit;True;SGradientDissolve;-1;;25;e27ad990a2429b34d8f406fd90200dba;1,26,0;7;29;FLOAT;1;False;24;FLOAT;0;False;22;FLOAT2;0,0;False;17;SAMPLER2D;;False;18;FLOAT;1;False;20;FLOAT;0.5;False;21;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;258;-4576,816;Inherit;False;Property;_NoiseSpeedControl;NoiseSpeedControl;30;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;2;Static;Dynamic;Create;True;True;All;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;183;-1872,976;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;103;-112,2416;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;184;-1888,1200;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;150;-1664.906,-677.972;Inherit;False;Property;_ShimeringFreq;ShimeringFreq;2;0;Create;True;0;0;0;False;0;False;5;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;136;-4288,816;Inherit;False;noiseTiling;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;51;-1344,1312;Inherit;False;Property;_Max;Max;14;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;52;-1360,1232;Inherit;False;Property;_Min;Min;15;0;Create;True;0;0;0;False;0;False;0;0.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;83;-1594.027,505.7242;Inherit;False;0;3;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;151;-1664,688;Inherit;False;InstancedProperty;_LifeTime;LifeTime;23;0;Create;True;0;0;0;False;0;False;0;0;-1;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;164;80,2416;Inherit;False;fadeNoise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;129;-1536,1072;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;146;-1469.906,-675.9719;Inherit;False;1;0;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;30;-2832,-288;Inherit;True;Property;_NoiseTex;NoiseTex;9;0;Create;True;0;0;0;False;0;False;e77e50664873809498937c7f032f4658;97a921d248636704ead1a9dcc3b43211;False;white;Auto;Texture2D;False;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;116;-1664,256;Inherit;True;Property;_TextureSample1;Texture Sample 1;18;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;152;-1341.93,625.2985;Inherit;False;Property;_InverseLifeTimeSource;InverseLifeTimeSource;22;0;Create;True;0;0;0;False;0;False;0;0;0;True;;KeywordEnum;2;UV_W;Value;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;165;-976,1328;Inherit;False;164;fadeNoise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;47;-976,1072;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;145;-1267.345,-676.3576;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;148;-1189.906,-745.9719;Inherit;False;Constant;_Float0;Float 0;20;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;144;-1331.845,-419.9575;Inherit;False;Property;_EmissionShimering;EmissionShimering;1;0;Create;True;0;0;0;False;0;False;0;0.2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;137;-1840,-256;Inherit;False;136;noiseTiling;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;5;-887.7037,7.445729;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;111;-944,288;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;98;-720,1200;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;9;-1206.762,-967.7441;Inherit;False;Property;_EmissionScale;EmissionScale;3;0;Create;True;0;0;0;False;0;False;1;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;147;-1006.906,-699.9719;Inherit;False;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;50;-1584,-320;Inherit;True;Property;_TextureSample0;Texture Sample 0;10;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;26;-1040,-224;Inherit;False;Property;_NoiseMin;NoiseMin;7;0;Create;True;0;0;0;False;0;False;-1.25;-1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;27;-1008,-144;Inherit;False;Property;_NoiseMax;NoiseMax;8;0;Create;True;0;0;0;False;0;False;1;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;12;-593.4457,7.649353;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;112;-704,288;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;105;-496,1200;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;25;-736.7677,-285.6512;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;149;-863.9055,-772.9719;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;163;1520,272;Inherit;False;GetWhisperFade;-1;;26;9440a6bfaa7d2294582938da56347776;0;1;10;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;53;-320,272;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;28;-439.2122,-296.0515;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;13;-474.8456,7.649352;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;156;1856,144;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;11;-275.4457,-113.3506;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;153;2048,16;Inherit;False;Property;_WhisperOnly;WhisperOnly;24;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;155;2336,-128;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VertexColorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;263;2304,-304;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;264;2544,-128;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;1;0,0;Float;False;False;-1;2;AmplifyShaderEditor.MaterialInspector;0;15;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit Forward;0;1;Sprite Unlit Forward;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;UniversalMaterialType=Unlit;ShaderGraphShader=true;True;0;True;14;all;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;False;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;2;0,0;Float;False;False;-1;2;AmplifyShaderEditor.MaterialInspector;0;15;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;SceneSelectionPass;0;2;SceneSelectionPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;UniversalMaterialType=Unlit;ShaderGraphShader=true;True;0;True;14;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=SceneSelectionPass;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;3;0,0;Float;False;False;-1;2;AmplifyShaderEditor.MaterialInspector;0;15;New Amplify Shader;cf964e524c8e69742b1d21fbe2ebcc4a;True;ScenePickingPass;0;3;ScenePickingPass;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;UniversalMaterialType=Unlit;ShaderGraphShader=true;True;0;True;14;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Picking;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;0;2704,-128;Float;False;True;-1;2;AmplifyShaderEditor.MaterialInspector;0;18;AmplifyShaders/Particles RadiusWave;cf964e524c8e69742b1d21fbe2ebcc4a;True;Sprite Unlit;0;0;Sprite Unlit;5;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;5;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;UniversalMaterialType=Unlit;ShaderGraphShader=true;True;0;True;14;all;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;False;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;5;Alpha Clipping;0;0;Disable Color Tint;1;0;Vertex Position;1;0;Debug Display;0;0;External Alpha;0;0;0;4;True;True;True;True;False;;False;0
WireConnection;231;0;210;0
WireConnection;251;0;231;1
WireConnection;251;1;231;0
WireConnection;226;0;208;0
WireConnection;226;2;251;0
WireConnection;239;0;237;0
WireConnection;239;1;238;0
WireConnection;235;0;226;0
WireConnection;236;0;235;0
WireConnection;236;1;237;0
WireConnection;236;2;239;0
WireConnection;166;0;168;0
WireConnection;166;2;171;0
WireConnection;244;1;245;0
WireConnection;244;0;236;0
WireConnection;174;0;166;0
WireConnection;240;0;244;0
WireConnection;206;0;174;0
WireConnection;206;1;207;0
WireConnection;36;0;35;0
WireConnection;250;0;206;0
WireConnection;250;2;241;0
WireConnection;139;0;36;0
WireConnection;162;1;196;0
WireConnection;162;0;250;0
WireConnection;197;0;162;0
WireConnection;108;0;109;1
WireConnection;108;1;109;2
WireConnection;201;0;183;0
WireConnection;180;0;140;0
WireConnection;195;0;30;0
WireConnection;95;0;94;0
WireConnection;157;22;108;0
WireConnection;157;17;107;0
WireConnection;157;18;106;0
WireConnection;157;19;161;0
WireConnection;157;20;141;0
WireConnection;157;21;93;0
WireConnection;199;0;51;0
WireConnection;200;0;201;0
WireConnection;198;0;52;0
WireConnection;19;0;18;1
WireConnection;19;1;18;2
WireConnection;194;0;30;0
WireConnection;205;0;180;0
WireConnection;205;1;202;0
WireConnection;191;0;195;0
WireConnection;89;0;157;0
WireConnection;89;2;95;0
WireConnection;143;0;200;0
WireConnection;143;1;198;0
WireConnection;143;2;199;0
WireConnection;209;0;24;0
WireConnection;21;0;19;0
WireConnection;21;1;20;0
WireConnection;187;0;160;0
WireConnection;188;0;138;0
WireConnection;193;0;194;0
WireConnection;248;0;205;0
WireConnection;101;0;143;0
WireConnection;101;1;89;0
WireConnection;186;0;138;0
WireConnection;190;0;160;0
WireConnection;192;0;191;0
WireConnection;249;0;205;0
WireConnection;22;0;21;0
WireConnection;22;2;209;0
WireConnection;261;0;21;0
WireConnection;261;1;262;0
WireConnection;179;29;187;0
WireConnection;179;24;248;0
WireConnection;179;22;188;0
WireConnection;179;17;193;0
WireConnection;179;20;33;0
WireConnection;179;21;34;0
WireConnection;102;0;101;0
WireConnection;181;29;190;0
WireConnection;181;24;249;0
WireConnection;181;22;186;0
WireConnection;181;17;192;0
WireConnection;181;20;134;0
WireConnection;181;21;135;0
WireConnection;258;1;22;0
WireConnection;258;0;261;0
WireConnection;183;0;179;0
WireConnection;103;0;102;0
WireConnection;103;1;104;0
WireConnection;184;0;181;0
WireConnection;136;0;258;0
WireConnection;164;0;103;0
WireConnection;129;0;183;0
WireConnection;129;1;184;0
WireConnection;146;0;150;0
WireConnection;116;0;30;0
WireConnection;152;1;83;3
WireConnection;152;0;151;0
WireConnection;47;0;129;0
WireConnection;47;1;52;0
WireConnection;47;2;51;0
WireConnection;145;0;146;0
WireConnection;111;0;116;1
WireConnection;111;1;152;0
WireConnection;98;0;47;0
WireConnection;98;1;165;0
WireConnection;147;0;148;0
WireConnection;147;1;145;0
WireConnection;147;2;144;0
WireConnection;50;0;30;0
WireConnection;50;1;137;0
WireConnection;12;0;5;0
WireConnection;112;0;111;0
WireConnection;105;0;98;0
WireConnection;25;0;50;1
WireConnection;25;1;26;0
WireConnection;25;2;27;0
WireConnection;149;0;9;0
WireConnection;149;1;147;0
WireConnection;53;0;12;3
WireConnection;53;1;112;0
WireConnection;53;2;105;0
WireConnection;28;0;149;0
WireConnection;28;1;25;0
WireConnection;13;0;12;0
WireConnection;13;1;12;1
WireConnection;13;2;12;2
WireConnection;156;0;53;0
WireConnection;156;1;163;0
WireConnection;11;0;28;0
WireConnection;11;1;13;0
WireConnection;153;1;53;0
WireConnection;153;0;156;0
WireConnection;155;0;11;0
WireConnection;155;3;153;0
WireConnection;264;0;263;0
WireConnection;264;1;155;0
WireConnection;0;0;264;0
ASEEND*/
//CHKSM=D582529D4A718CC35721A8D51EB8616C690329A5