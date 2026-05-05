#version 330

layout(location=0) out vec4 FragColor;
in vec2 v_Tex;
const float c_PI = 3.1415926535897932384626433832795;
uniform float u_Time;
uniform sampler2D u_RGBTex;
uniform sampler2D u_CurrNumTex;
uniform sampler2D u_NumsTex;
uniform int u_InputNum;

void Flag()
{
    float amp = 0.4;
    float speed = 1.0;
    float sinInput = v_Tex.x * c_PI * 2 - u_Time * speed;
    float sinValue = v_Tex.x * amp * (((sin(sinInput) + 1) / 2) - 0.5) + 0.5;
    float fWidth = 0.0;
    float width =  0.5 * mix(1, fWidth, v_Tex.x);  
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

void Flame()
{
    float amp = 0.4;
    float speed = 1.0;
    float newY = 1 - v_Tex.y;
    float sinInput = newY * c_PI * 2 - u_Time * speed;
    float sinValue = newY * amp * (((sin(sinInput) + 1) / 2) - 0.5) + 0.5;

    float fWidth = 0.0;
    float width =  0.5 * mix(fWidth, 1, newY);  
    float grey = 0;

    if (v_Tex.x < sinValue + width/2 && v_Tex.x > sinValue - width/2) {
        grey = 1;
    }
    else {
        grey = 0;
        discard;
    }
    FragColor = vec4(grey);
}

void TextureSampling()
{
    vec4 c0;
    vec4 c1;
    vec4 c2;    
    vec4 c3;
    vec4 c4; 

    float offsetX = 0.01;
    c0 = texture(u_RGBTex, vec2(v_Tex.x - offsetX * 2, v_Tex.y));
    c1 = texture(u_RGBTex, vec2(v_Tex.x - offsetX * 1, v_Tex.y));
    c2 = texture(u_RGBTex, vec2(v_Tex.x - offsetX * 0, v_Tex.y));
    c3 = texture(u_RGBTex, vec2(v_Tex.x + offsetX * 1, v_Tex.y));
    c4 = texture(u_RGBTex, vec2(v_Tex.x + offsetX * 2, v_Tex.y));

    vec4 sum = c0 + c1 + c2 + c3 + c4;
    sum = sum / 5.0;
    
    FragColor = sum;
}

void TextureQ1() 
{
    float tx = v_Tex.x;
    float ty = 1 - 2 * abs(v_Tex.y - 0.5);

    FragColor = texture(u_RGBTex, vec2(tx, ty));
    //FragColor = vec4(ty); // -> y값이 0에서 1로 증가하다가 다시 0으로 감소하는 패턴을 확인 가능
}

void TextureQ2()
{
    float tx = fract(v_Tex.x * 3);  // tx 값이 0에서 1로 증가하는게 반복
    float ty = v_Tex.y / 3;         // (2/3, 1), (1/3, 2/3), (0, 1/3)

    float offsetX = 0;
    float offsetY = (2 - floor(v_Tex.x * 3)) / 3;

    FragColor = texture(u_RGBTex, vec2(tx + offsetX, ty + offsetY));
    //FragColor = vec4(ty);
}

void TextureQ3()
{
    float tx = fract(v_Tex.x * 3);
    float ty = v_Tex.y / 3;

    float offsetX = 0;
    float offsetY = (floor(v_Tex.x * 3)) / 3;

    FragColor = texture(u_RGBTex, vec2(tx + offsetX, ty + offsetY));
}

void TextureQ4()
{
    float resolX = 3;
    float resolY = 3;
    float shear = 0.5 * u_Time; 

    float offsetX = fract(ceil(v_Tex.y * resolY) * shear); 
    float offsetY = 0;

    float tx = fract(v_Tex.x * resolX + offsetX);
    float ty = fract(v_Tex.y * resolY + offsetY);

    vec2 newTex = vec2(tx, ty);
    FragColor = texture(u_RGBTex, newTex);
}

void Num()
{
    // 1. 설정
    int numberToDisplay = 213;   // 출력하고 싶은 전체 숫자
    int maxDigits = 3;           // 표시할 총 자릿수 (예: 3자리면 000~999)
    
    // 2. 현재 픽셀이 몇 번째 자릿수 칸에 있는지 계산
    // v_Tex.x가 0~1이므로, 3을 곱하고 floor를 취하면 0, 1, 2번 칸이 나옵니다.
    float digitIndex = floor(v_Tex.x * float(maxDigits)); 
    
    // 3. 해당 칸에서 사용할 개별적인 0~1 범위의 좌표 (fract 이용)
    float localTx = fract(v_Tex.x * float(maxDigits));
    
    // 4. 표시할 숫자에서 현재 자릿수의 구체적인 숫자(0~9) 추출
    // 예: 213에서 0번 칸(백의 자리)이면 2, 1번 칸(십의 자리)이면 1...
    // 오른쪽부터 계산하는 것이 수학적으로 편하므로 뺀 값을 사용합니다.
    float powerOfTen = pow(10.0, float(maxDigits) - 1.0 - digitIndex);
    int currentDigit = int(mod(float(numberToDisplay) / powerOfTen, 10.0));

    // 5. 텍스처 시트(5x2)에서 해당 숫자의 위치 계산
    float tx = localTx / 5.0;
    float ty = v_Tex.y / 2.0;

    // 숫자 시트 배치에 따른 오프셋 (기존 코드 로직 유지)
    float offsetX = float(int(mod(float(currentDigit), 5.0))) / 5.0;
    float offsetY = float(int(float(currentDigit) / 5.0)) / 2.0;

    vec2 newTex = vec2(tx + offsetX, ty + offsetY);
    
    // 6. 출력
    FragColor = texture(u_NumsTex, newTex);
}

void main()
{
    Num();
}