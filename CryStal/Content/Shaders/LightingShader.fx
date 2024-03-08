#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

cbuffer world
{
     float4x4 World;
     float4x4 Model;
};
cbuffer LightingConstants
{
    float2 lightPosition;

    float constantT;
    float linearT;
    float quadraticT;

    float3 lightAmbient;
};

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
    float2 WorldPos : TEXCOORD1;
};
VertexShaderOutput VS(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
        VertexShaderOutput output;
        
        output.Position = mul(position, Model);
        output.Color = color;
        output.TextureCoordinates = texCoord;
        output.WorldPos = mul(position, World).xy;
        
        return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float3 result;
    //ambient
    float3 ambient = lightAmbient * tex2D(SpriteTextureSampler, input.TextureCoordinates).rgb;
    //attenuation
    float distance = length(lightPosition - input.WorldPos);
    float attenuation = 1.0 / (constantT + linearT * distance + quadraticT * (distance * distance));
    
    result = ambient * attenuation;
    
    return float4((result), 1.0);
}

technique SpriteDrawing
{
	pass P0
	{
        VertexShader = compile VS_SHADERMODEL VS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};