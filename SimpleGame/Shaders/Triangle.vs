#version 330

uniform float u_Time;

in vec3 a_Position;
in float a_Mass;
in vec2 a_Vel;
in float a_RV;
in float a_RV1;
in float a_RV2; // LifeTime

const float c_PI = 3.141592; 
const vec2 c_G = vec2(0, -9.8);     // Gravity

float random(float n) {
    return fract(sin(n) * 43758.5453123);
}

void Sin1() {
    float startTime = a_RV1 * 2;
    float newTime = u_Time - startTime;
    
    if (newTime > 0) 
    { 
        float t = mod(newTime * 2.0, 1.0f);
        float amp = (1-t) * 0.2 * (a_RV - 0.5) * 2.0;
        float period = a_RV1;
       
	    vec4 newPosition;
	    newPosition.x = a_Position.x * a_RV2 * 0.2 + t;
	    newPosition.y = a_Position.y * a_RV2 * 0.2 
	        + amp * sin(t * 2 * c_PI * period);
	    newPosition.z = 0;
	    newPosition.w = 1;
	    
	    gl_Position = newPosition;
    }
    else 
    {
        gl_Position = vec4(-1000, 0, 0, 0); 
    }
}

void Falling()
{
    // 초기 a_Position은 어차피 센터(0,0) +- size 에서 시작하고, 여기서 원으로 위치시키는거임.
    // 정점마다 항상 같은 a_RV를 가지므로, 이를 Seed로 한 랜덤 Scale 계산.
    
    float scale = 0.5 + (random(a_RV + 2.0) * 1.5);
    vec2 scaledOffset = a_Position.xy * scale;
    
    float startTime = random(a_RV);
    float newTime = u_Time - startTime;
    
    if (newTime > 0) 
    {
        float lifeScale = 2.0;
        float lifeTime = 0.5 + a_RV2 * lifeScale;
        float t = lifeTime * fract(newTime / lifeTime); // 0 ~ lifeTime 구간 반복
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
    else 
    {
        gl_Position = vec4(-1000, 0, 0, 0); 
    }
}

void main()
{
    Sin1();
}
