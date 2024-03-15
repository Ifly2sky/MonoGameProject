#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float2 WorldSize;

cbuffer cb
{
    float2 lightPosition;

    float constantT;
    float linearT;
    float quadraticT;

    float3 lightAmbient;
    float3 lightDiffuse;
};

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_Position;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};
//VertexShaderOutput VS(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
//{
//        VertexShaderOutput output;
        
//        output.Position = position;
//        output.Color = color;
//        output.TextureCoordinates = texCoord;
        
//        return output;
//}

float4 MainPS(VertexShaderOutput input) : COLOR0
{
    float3 result;
    
    float2 pxPos;
    pxPos.x = input.Position.x;
    pxPos.y = WorldSize.y - input.Position.y;
    
    //Pixel Color
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    
    //ambient
    float3 ambient = lightAmbient * color.rgb;
    
    float3 diffuse = lightDiffuse * color.rgb;
    
    //attenuation
    float distance = length(lightPosition - pxPos);
    float attenuation = 1.0 / (constantT + linearT * distance + quadraticT * (distance * distance));
    
    result = color.rgb * ambient + diffuse * attenuation;
    
    return float4(result, color.a);
}

technique SpriteDrawing
{
	pass P0
	{
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};