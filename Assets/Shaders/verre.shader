// Shader created with Shader Forge Beta 0.34 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.34;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:2,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5955882,fgcg:0.5955882,fgcb:0.5955882,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32485,y:32787|diff-19-RGB,diffpow-426-OUT,spec-25-RGB,gloss-409-OUT,normal-656-RGB,transm-316-RGB,lwrap-332-RGB,amdfl-193-RGB,alpha-662-OUT;n:type:ShaderForge.SFN_Tex2d,id:19,x:32934,y:32467,ptlb:diffuse,ptin:_diffuse,tex:16abe4e4088805a4ba75e83f0b5b8266,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:25,x:32934,y:32710,ptlb:specular,ptin:_specular,tex:05c7888ea950fcf4db7ab9ec05e0c6c7,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Color,id:193,x:32934,y:33294,ptlb:diffuse ambient light,ptin:_diffuseambientlight,glob:False,c1:0.4705882,c2:0.3921569,c3:0.3176471,c4:1;n:type:ShaderForge.SFN_Color,id:316,x:33099,y:33232,ptlb:transmission,ptin:_transmission,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:332,x:32963,y:33127,ptlb:light wrapping,ptin:_lightwrapping,glob:False,c1:0.2132353,c2:0.1318555,c3:0.1019139,c4:1;n:type:ShaderForge.SFN_Slider,id:409,x:33047,y:32887,ptlb:gloss,ptin:_gloss,min:0,cur:0.7586716,max:1;n:type:ShaderForge.SFN_Slider,id:426,x:33073,y:32632,ptlb:Diffuse power,ptin:_Diffusepower,min:0,cur:5,max:10;n:type:ShaderForge.SFN_TexCoord,id:648,x:32179,y:32862,uv:0;n:type:ShaderForge.SFN_TexCoord,id:650,x:32179,y:32863,uv:0;n:type:ShaderForge.SFN_Tex2d,id:656,x:33275,y:33119,ptlb:normal,ptin:_normal,tex:25b6aa97d460d224ca38043eb231146c,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Power,id:662,x:32757,y:32859|VAL-19-A,EXP-668-OUT;n:type:ShaderForge.SFN_Slider,id:668,x:32934,y:33009,ptlb:alpha power,ptin:_alphapower,min:0,cur:0,max:10;proporder:19-25-193-316-332-409-426-656-668;pass:END;sub:END;*/

Shader "Shader Forge/verre" {
    Properties {
        _diffuse ("diffuse", 2D) = "gray" {}
        _specular ("specular", 2D) = "gray" {}
        _diffuseambientlight ("diffuse ambient light", Color) = (0.4705882,0.3921569,0.3176471,1)
        _transmission ("transmission", Color) = (0,0,0,1)
        _lightwrapping ("light wrapping", Color) = (0.2132353,0.1318555,0.1019139,1)
        _gloss ("gloss", Range(0, 1)) = 0.7586716
        _Diffusepower ("Diffuse power", Range(0, 10)) = 5
        _normal ("normal", 2D) = "bump" {}
        _alphapower ("alpha power", Range(0, 10)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform sampler2D _specular; uniform float4 _specular_ST;
            uniform float4 _diffuseambientlight;
            uniform float4 _transmission;
            uniform float4 _lightwrapping;
            uniform float _gloss;
            uniform float _Diffusepower;
            uniform sampler2D _normal; uniform float4 _normal_ST;
            uniform float _alphapower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_673 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_normal,TRANSFORM_TEX(node_673.rg, _normal))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 w = _lightwrapping.rgb*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = pow(max(float3(0.0,0.0,0.0), NdotLWrap + w ), _Diffusepower);
                float3 backLight = pow(max(float3(0.0,0.0,0.0), -NdotLWrap + w ), _Diffusepower) * _transmission.rgb;
                float3 diffuse = (forwardLight+backLight) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb*2;
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = tex2D(_specular,TRANSFORM_TEX(node_673.rg, _specular)).rgb;
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(reflect(-lightDirection, normalDirection),viewDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight += _diffuseambientlight.rgb; // Diffuse Ambient Light
                float4 node_19 = tex2D(_diffuse,TRANSFORM_TEX(node_673.rg, _diffuse));
                finalColor += diffuseLight * node_19.rgb;
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor,pow(node_19.a,_alphapower));
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform sampler2D _specular; uniform float4 _specular_ST;
            uniform float4 _transmission;
            uniform float4 _lightwrapping;
            uniform float _gloss;
            uniform float _Diffusepower;
            uniform sampler2D _normal; uniform float4 _normal_ST;
            uniform float _alphapower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_674 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_normal,TRANSFORM_TEX(node_674.rg, _normal))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 w = _lightwrapping.rgb*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = pow(max(float3(0.0,0.0,0.0), NdotLWrap + w ), _Diffusepower);
                float3 backLight = pow(max(float3(0.0,0.0,0.0), -NdotLWrap + w ), _Diffusepower) * _transmission.rgb;
                float3 diffuse = (forwardLight+backLight) * attenColor;
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = tex2D(_specular,TRANSFORM_TEX(node_674.rg, _specular)).rgb;
                float3 specular = attenColor * pow(max(0,dot(reflect(-lightDirection, normalDirection),viewDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_19 = tex2D(_diffuse,TRANSFORM_TEX(node_674.rg, _diffuse));
                finalColor += diffuseLight * node_19.rgb;
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * pow(node_19.a,_alphapower),0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
