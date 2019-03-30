#ifndef SHADER_PIXEL_INC
#define SHADER_PIXEL_INC

struct Pixel
{
    float4 vertex : SV_POSITION;
    float4 color : COLOR;
};

fixed4 frag(Pixel i) : SV_Target { return i.color; }

#endif //SHADER_PIXEL_INC
