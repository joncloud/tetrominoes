﻿#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float A;
float F;
float S;
float t;
sampler TexSampler : register(s0);

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float xp(float r)
{
    return (A / 1000) * (t - cos((F / 1000) * t + S * r));
}
float yp(float r)
{
    return (A / 1000) * (t - sin((F / 1000) * t + S * r));
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float r = sqrt(pow(input.TextureCoordinates.x, 2) + pow(input.TextureCoordinates.y, 2));
    input.TextureCoordinates.x += xp(r);
    input.TextureCoordinates.y += yp(r);

    float4 result = tex2D(TexSampler, input.TextureCoordinates) * input.Color;
    return result;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};