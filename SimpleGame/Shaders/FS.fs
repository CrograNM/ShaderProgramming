#version 330

layout(location=0) out vec4 FragColor;
in vec2 v_Tex;
const float c_PI = 3.1415926535897932384626433832795;
uniform float u_Time;

void main()
{
    float amp = 0.4;
    float sinInput = v_Tex.x * c_PI * 2 - u_Time;
    float sinValue = v_Tex.x * amp * (((sin(sinInput) + 1) / 2) - 0.5) + 0.5;
    float width =  0.3 * (1 - v_Tex.x);  
    float grey = 0;
    
    if (v_Tex.y < sinValue + width/2 && v_Tex.y > sinValue - width/2) {
        grey = 1;
    }
    else {
        grey = 0;
        discard;
    }
    
    FragColor = vec4(grey);
}
