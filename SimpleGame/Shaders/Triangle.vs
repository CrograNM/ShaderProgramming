#version 330

uniform float u_Time;

in vec3 a_Position;
in float a_Mass;
in vec2 a_Vel;
in float a_RV;

const float c_PI = 3.141592; 
const vec2 c_G = vec2(0, -9.8);     // Gravity

float random(float n) {
    return fract(sin(n) * 43758.5453123);
}

void Falling()
{
    // 초기 a_Position은 어차피 센터(0,0) +- size 에서 시작하고, 여기서 원으로 위치시키는거임.
    // 정점마다 항상 같은 a_RV를 가지므로, 이를 Seed로 한 랜덤 Scale 계산.
    
    float scale = 0.5 + (random(a_RV + 2.0) * 1.5);
    vec2 scaledOffset = a_Position.xy * scale;
    
    float startTime = random(a_RV);
    float newTime = u_Time - startTime;
    
    if (newTime > 0) {
        float t = mod(newTime, 1.0);
        float tt = t*t;
        float vx = a_Vel.x;
        float vy = a_Vel.y;
        
        // Init Pos - Circular
        float initPosX = scaledOffset.x + sin(a_RV * 2 * c_PI);
        float initPosY = scaledOffset.y + cos(a_RV * 2 * c_PI);
        
        vec4 newPos;
        newPos.x = initPosX + vx * t + 0.5 * c_G.x * tt;
        newPos.y = initPosY + vy * t + 0.5 * c_G.y * tt;
        newPos.z = 0;
        newPos.w = 1;
        gl_Position = newPos;
    }
    else {
        gl_Position = vec4(-1000, 0, 0, 0); 
    }
}

void main()
{
    Falling();
}
