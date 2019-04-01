#ifndef SHADER_VERTEX_INC
#define SHADER_VERTEX_INC

struct Vertex
{
    float4 vertex : POSITION;
};

inline float4 ToScreenPos(float4 pos)
{
    float4 o = pos * 0.5f;
    o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
    o.zw = pos.zw;
    o.xy /= o.w;
    return o;
}

inline float4 FromScreenPos(float4 pos)
{
    float4 o = pos;
    o.xy *= o.w;
    o.xy = o.xy - pos.w * 0.5;
    o.xy = o.xy * float2(1, _ProjectionParams.x);
    o.xy = o.xy * 2.0;
    o.zw = pos.zw;
    return o;
}

int maxVertexShown;
float lineThickness;
float borderThickness;

Pixel vert(Vertex v)
{
    Pixel o = (Pixel)0;
    if (maxVertexShown - v.vertex.x <= 0)
        return o;

    float4 p0 = positionBuffer[(int)v.vertex.x];
    float4 p1 = positionBuffer[(int)v.vertex.z];
    p0 = UnityObjectToClipPos(p0.xyz);
    p1 = UnityObjectToClipPos(p1.xyz);

    //-----------

    //float2 dir = p1.xy - p0.xy;
    //dir = normalize(dir);
    //dir = float2(-dir.y, dir.x);
    //float4 offset1 = float4(dir * abs(p0.w) * 0.1 / 2.0f, 0, 0);
    //p0 = p0 + offset1 * v.vertex.y;

    //-----------

    p0 = ToScreenPos(p0);
    p1 = ToScreenPos(p1);
    float3 dir = normalize(p1.xyz - p0.xyz) * sign(p0.w);
    float3 norm = float3(-dir.y, dir.x, dir.z);
    float sThick = abs(v.vertex.y);
    float y = abs(v.vertex.y);
    float thickness = lerp(lineThickness,
                      lerp(lineThickness,
                           lineThickness + borderThickness,
                           saturate(max(0, y - 3))),
                           saturate(max(0, y - 2)));
    p0.xyz += norm * (_ScreenParams.z - 1.0) * thickness * clamp(v.vertex.y, -1, 1);
    p0 = FromScreenPos(p0);

    //----------

    o.vertex = p0;
    o.color = half4(lerp(colorBuffer[(int)v.vertex.x] * DIMMER,
                         BORDER,
                         saturate(abs(v.vertex.y) - 2)), 1);

    return o;
}

#endif //SHADER_VERTEX_INC
//(187, 187, 1.005348, 1.005348)