#version 330

layout(location=0) out vec4 FragColor;
in vec2 v_TPos;
const float c_PI = 3.1415926535897932384626433832795;

uniform float u_Time;

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
    float time = -u_Time * 20;
    vec2 center = vec2(0.5, 0.5);
    vec2 currentPos = v_TPos.xy;
    float dist = distance(center, currentPos);
    float value = abs(sin(dist * 2 * c_PI * 4 + time));
    FragColor = vec4(pow(value, 256));

    // float radius = 0.4;
    // if (dist > radius) {
    //     discard;
    // }
    // 
    // float lineCount = 5;
    // float lineWidth = 1;
    // lineWidth = 100 / lineWidth;
    // float per = -0.5 * c_PI;
    // 
    // float grey = pow(abs(sin((dist * 2 * c_PI +per) * lineCount)), lineWidth);
    // 
    // FragColor  = vec4(grey);
}
void FractalMosaic() {
    // 1. 좌표 정규화 (0~1 범위를 -1~1 범위로 변경하고 비율 조정)
    vec2 uv = (v_TPos - 0.5) * 2.0;
    vec2 uv0 = uv; // 원래의 좌표 저장 (색상 그라데이션용)
    vec3 finalColor = vec3(0.0);

    // 2. 프랙탈 반복 루프 (반복 횟수가 많아질수록 무늬가 복잡해집니다)
    for (float i = 0.0; i < 4.0; i++) {
        // 공간 분할: 좌표를 계속 배수로 곱하고 소수점만 남겨(fract) 타일링 반복
        uv = fract(uv * 1.5) - 0.5;

        // 현재 타일의 중심으로부터의 거리 계산
        float d = length(uv) * exp(-length(uv0));

        // 사용자 스타일의 파형 계산: abs(sin)과 u_Time을 이용한 애니메이션
        // 여기에 pow를 써서 선을 아주 날카롭게 만듭니다.
        float v = sin(d * 8.0 + u_Time) / 8.0;
        v = abs(v);
        
        // 역수를 취하고 pow를 적용해 네온 사인 같은 광선 효과 생성
        v = pow(0.01 / v, 1.2);

        // 루프 단계별로 다른 색상 부여 (점점 밝아지도록 중첩)
        vec3 col = vec3(0.0, 1.0, 1.0) * v; // 사이언/블루 계열
        finalColor += col * (1.0 / (i + 1.0)); // 단계별 감쇄 중첩
    }

    FragColor = vec4(finalColor, 1.0);
}
void main()
{
	FractalMosaic();
}
