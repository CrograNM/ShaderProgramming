#version 330

layout(location=0) out vec4 FragColor;
in vec2 v_TPos;
const float c_PI = 3.1415926535897932384626433832795;

void Simple() {
    vec3 color;
    
    if (v_TPos.x + v_TPos.y < 0.5) { 
    	color = vec3(1.0, 0.0, 0.0); // Red
    } 
    else {
    	color = vec3(0.0, 0.0, 1.0); // Blue
    }
    FragColor = vec4(color, 1.0);
}
void Pattern() {
    // FragColor = vec4(v_TPos, 0.0, 1.0);
    float lineCountH = 2;
    float lineCountV = 2;
    float lineWidth = 1;
    lineCountH = lineCountH / 2;
    lineCountV = lineCountV / 2;
    lineWidth = 100 / lineWidth;
    float per = -0.5 * c_PI;
    float Xgrey = pow(
                    abs(sin((v_TPos.x * 2 * c_PI +per) * lineCountH))
                        , lineWidth);
    float Ygrey = pow(
                    abs(sin((v_TPos.y * 2 * c_PI +per) * lineCountV))
                        , lineWidth);
    float grey = Xgrey + Ygrey;
    FragColor = vec4(grey);
}
void Circle() {
    vec2 center = vec2(0.5, 0.5);
    vec2 currentPos = v_TPos.xy;
    float dist = distance(center, currentPos);

    float radius = 0.4;
    float lineWidth = 0.01;
    lineWidth = lineWidth / 2;

    if (dist < radius - lineWidth || dist > radius + lineWidth) {
        discard;
    }
    FragColor = vec4(0.0, 1.0, 1.0, 1.0); // Cyan
}
void CircleSin() {
    vec2 center = vec2(0.5, 0.5);
    vec2 currentPos = v_TPos.xy;
    float dist = distance(center, currentPos);

    float lineCount = 5;
    float lineWidth = 1;
    lineWidth = 100 / lineWidth;
    float per = -0.5 * c_PI;

    float grey = pow(abs(sin((dist * 2 * c_PI +per) * lineCount)), lineWidth);
    FragColor  = vec4(grey);
}
void SixAngleStar() {
    
} 
void main()
{
	CircleSin();
}
