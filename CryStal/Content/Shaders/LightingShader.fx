#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
struct Material
{
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct PointLight
{
    float3 position;
	
    float constant;
	float line;
    float quadratic;
	
    float3 ambient;
    float3 diffuse;
    float3 specular;
};
#define NR_POINT_LIGHTS 2
PointLight pointLights[NR_POINT_LIGHTS];

float3 viewPos;
Texture2D SpriteTexture;
Material material;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float3 CalcPointLight(VertexShaderOutput input, PointLight light, float3 normal, float3 fragPos, float3 viewDir);

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 result;
    
    float3 norm = (0, 0, 1);
    float3 viewDir = viewPos - input.Position.xyz;
    
    for (int i = 0; i < NR_POINT_LIGHTS; i++)
    {
        result += CalcPointLight(input, pointLights[i], norm, input.Position.xyz, viewDir);
    }
    
    return (result, 0);
}
float3 CalcPointLight(VertexShaderOutput input, PointLight light, float3 normal, float3 fragPos, float3 viewDir)
{
    float3 lightDir = (normalize(light.position - fragPos), 1.0);
	// diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    float3 reflectDir = reflect(-lightDir, normal);
	// specular
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.line * distance + light.quadratic * (distance * distance));
    // combine
    float3 ambient = light.ambient * float3(tex2D(material.diffuse, input.TextureCoordinates).xyz);
    float3 diffuse = light.diffuse * diff * float3(tex2D(material.diffuse, input.TextureCoordinates).xyz);
    float3 specular = light.specular * spec * float3(tex2D(material.specular, input.TextureCoordinates).xyz);
    
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};