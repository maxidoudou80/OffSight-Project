// Shader created with Shader Forge Beta 0.34 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.34;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5955882,fgcg:0.5955882,fgcb:0.5955882,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32523,y:32696|diff-902-OUT,diffpow-55-OUT,spec-727-OUT,gloss-61-OUT,normal-8-RGB,emission-734-OUT,transm-680-RGB,lwrap-676-RGB,amdfl-678-RGB,olwid-705-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:32878,y:32354,ptlb:diffuse,ptin:_diffuse,tex:c44ee14d6dd3e2c48a986fe0caa577fe,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:8,x:33265,y:32864,ptlb:normal,ptin:_normal,tex:4a43baa6a7ad20746ace47da8f2e6ad6,ntxv:1,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:16,x:33501,y:32555,ptlb:specular,ptin:_specular,tex:b853215d95805884e9ee737b6523d5ca,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Slider,id:55,x:33307,y:32349,ptlb:diffuse power,ptin:_diffusepower,min:0,cur:9.228553,max:20;n:type:ShaderForge.SFN_Slider,id:61,x:33422,y:32453,ptlb:gloss,ptin:_gloss,min:0,cur:0.6122221,max:1;n:type:ShaderForge.SFN_Tex2d,id:67,x:33425,y:33073,ptlb:emissive,ptin:_emissive,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Color,id:676,x:33242,y:33274,ptlb:light wrapping,ptin:_lightwrapping,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_Color,id:678,x:33425,y:33274,ptlb:diffuse ambient light,ptin:_diffuseambientlight,glob:False,c1:0.4705882,c2:0.3921569,c3:0.3176471,c4:1;n:type:ShaderForge.SFN_Color,id:680,x:33057,y:33274,ptlb:transmission,ptin:_transmission,glob:False,c1:0,c2:0,c3:0,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:705,x:32838,y:33343,ptlb:outline width,ptin:_outlinewidth,glob:False,v1:0.004;n:type:ShaderForge.SFN_Multiply,id:711,x:33026,y:32572|A-16-RGB,B-713-RGB;n:type:ShaderForge.SFN_Color,id:713,x:33640,y:32733,ptlb:specular color,ptin:_specularcolor,glob:False,c1:1,c2:0.620284,c3:0.1397059,c4:1;n:type:ShaderForge.SFN_Slider,id:725,x:33203,y:32748,ptlb:specular power,ptin:_specularpower,min:0,cur:6.692996,max:20;n:type:ShaderForge.SFN_Multiply,id:727,x:32830,y:32622|A-711-OUT,B-725-OUT;n:type:ShaderForge.SFN_Multiply,id:734,x:32932,y:32946|A-67-RGB,B-735-OUT;n:type:ShaderForge.SFN_Slider,id:735,x:33075,y:33124,ptlb:emissive power,ptin:_emissivepower,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:753,x:33288,y:32157,ptlb:couleur1,ptin:_couleur1,glob:False,c1:0.4558824,c2:0.4558824,c3:0.4558824,c4:1;n:type:ShaderForge.SFN_ChannelBlend,id:902,x:32779,y:32170,chbt:1|M-903-RGB,R-920-OUT,G-909-RGB,B-910-RGB,BTM-2-RGB;n:type:ShaderForge.SFN_Tex2d,id:903,x:33041,y:31732,ptlb:masque,ptin:_masque,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Color,id:909,x:33154,y:31982,ptlb:couleur2,ptin:_couleur2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:910,x:33288,y:31982,ptlb:couleur3,ptin:_couleur3,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:919,x:33447,y:32157,ptlb:motif,ptin:_motif,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:920,x:33047,y:32157|A-753-RGB,B-919-RGB;proporder:2-903-753-919-909-910-55-678-8-16-713-725-61-67-735-676-680-705;pass:END;sub:END;*/

Shader "Shader Forge/opaque" {
    Properties {
        _diffuse ("diffuse", 2D) = "gray" {}
        _masque ("masque", 2D) = "black" {}
        _couleur1 ("couleur1", Color) = (0.4558824,0.4558824,0.4558824,1)
        _motif ("motif", 2D) = "white" {}
        _couleur2 ("couleur2", Color) = (0.5,0.5,0.5,1)
        _couleur3 ("couleur3", Color) = (0.5,0.5,0.5,1)
        _diffusepower ("diffuse power", Range(0, 20)) = 9.228553
        _diffuseambientlight ("diffuse ambient light", Color) = (0.4705882,0.3921569,0.3176471,1)
        _normal ("normal", 2D) = "gray" {}
        _specular ("specular", 2D) = "black" {}
        _specularcolor ("specular color", Color) = (1,0.620284,0.1397059,1)
        _specularpower ("specular power", Range(0, 20)) = 6.692996
        _gloss ("gloss", Range(0, 1)) = 0.6122221
        _emissive ("emissive", 2D) = "black" {}
        _emissivepower ("emissive power", Range(0, 1)) = 0
        _lightwrapping ("light wrapping", Color) = (0,0,0,1)
        _transmission ("transmission", Color) = (0,0,0,1)
        _outlinewidth ("outline width", Float ) = 0.004
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float _outlinewidth;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal*_outlinewidth,1));
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                return fixed4(float3(0,0,0),0);
            }
            ENDCG
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform sampler2D _normal; uniform float4 _normal_ST;
            uniform sampler2D _specular; uniform float4 _specular_ST;
            uniform float _diffusepower;
            uniform float _gloss;
            uniform sampler2D _emissive; uniform float4 _emissive_ST;
            uniform float4 _lightwrapping;
            uniform float4 _diffuseambientlight;
            uniform float4 _transmission;
            uniform float4 _specularcolor;
            uniform float _specularpower;
            uniform float _emissivepower;
            uniform float4 _couleur1;
            uniform sampler2D _masque; uniform float4 _masque_ST;
            uniform float4 _couleur2;
            uniform float4 _couleur3;
            uniform sampler2D _motif; uniform float4 _motif_ST;
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
                float2 node_1020 = i.uv0;
                float3 node_8 = UnpackNormal(tex2D(_normal,TRANSFORM_TEX(node_1020.rg, _normal)));
                float3 normalLocal = node_8.rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 w = _lightwrapping.rgb*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = pow(max(float3(0.0,0.0,0.0), NdotLWrap + w ), _diffusepower);
                float3 backLight = pow(max(float3(0.0,0.0,0.0), -NdotLWrap + w ), _diffusepower) * _transmission.rgb;
                float3 diffuse = (forwardLight+backLight) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;
////// Emissive:
                float3 emissive = (tex2D(_emissive,TRANSFORM_TEX(node_1020.rg, _emissive)).rgb*_emissivepower);
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = ((tex2D(_specular,TRANSFORM_TEX(node_1020.rg, _specular)).rgb*_specularcolor.rgb)*_specularpower);
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                diffuseLight += _diffuseambientlight.rgb; // Diffuse Ambient Light
                float4 node_903 = tex2D(_masque,TRANSFORM_TEX(node_1020.rg, _masque));
                finalColor += diffuseLight * (lerp( lerp( lerp( tex2D(_diffuse,TRANSFORM_TEX(node_1020.rg, _diffuse)).rgb, (_couleur1.rgb*tex2D(_motif,TRANSFORM_TEX(node_1020.rg, _motif)).rgb), node_903.rgb.r ), _couleur2.rgb, node_903.rgb.g ), _couleur3.rgb, node_903.rgb.b ));
                finalColor += specular;
                finalColor += emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform sampler2D _normal; uniform float4 _normal_ST;
            uniform sampler2D _specular; uniform float4 _specular_ST;
            uniform float _diffusepower;
            uniform float _gloss;
            uniform sampler2D _emissive; uniform float4 _emissive_ST;
            uniform float4 _lightwrapping;
            uniform float4 _transmission;
            uniform float4 _specularcolor;
            uniform float _specularpower;
            uniform float _emissivepower;
            uniform float4 _couleur1;
            uniform sampler2D _masque; uniform float4 _masque_ST;
            uniform float4 _couleur2;
            uniform float4 _couleur3;
            uniform sampler2D _motif; uniform float4 _motif_ST;
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
                float2 node_1021 = i.uv0;
                float3 node_8 = UnpackNormal(tex2D(_normal,TRANSFORM_TEX(node_1021.rg, _normal)));
                float3 normalLocal = node_8.rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 w = _lightwrapping.rgb*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = pow(max(float3(0.0,0.0,0.0), NdotLWrap + w ), _diffusepower);
                float3 backLight = pow(max(float3(0.0,0.0,0.0), -NdotLWrap + w ), _diffusepower) * _transmission.rgb;
                float3 diffuse = (forwardLight+backLight) * attenColor;
///////// Gloss:
                float gloss = _gloss;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = ((tex2D(_specular,TRANSFORM_TEX(node_1021.rg, _specular)).rgb*_specularcolor.rgb)*_specularpower);
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_903 = tex2D(_masque,TRANSFORM_TEX(node_1021.rg, _masque));
                finalColor += diffuseLight * (lerp( lerp( lerp( tex2D(_diffuse,TRANSFORM_TEX(node_1021.rg, _diffuse)).rgb, (_couleur1.rgb*tex2D(_motif,TRANSFORM_TEX(node_1021.rg, _motif)).rgb), node_903.rgb.r ), _couleur2.rgb, node_903.rgb.g ), _couleur3.rgb, node_903.rgb.b ));
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
