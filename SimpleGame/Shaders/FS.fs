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
    int numberToDisplay = 1222; // 출력할 숫자
    if (numberToDisplay < 0) numberToDisplay = abs(numberToDisplay); // 음수 처리

    // 1. 현재 숫자가 몇 자리인지 계산 (가변 자릿수 핵심)
    // log10(0)은 정의되지 않으므로 0일 때 예외처리가 필요합니다.
    int numDigits = (numberToDisplay == 0) ? 1 : int(floor(log(float(numberToDisplay)) / log(10.0))) + 1;

    // 2. 현재 픽셀이 몇 번째 자릿수 칸에 있는지 계산
    // v_Tex.x (0~1)를 현재 자릿수만큼 곱합니다.
    float digitIndex = floor(v_Tex.x * float(numDigits)); 
    
    // 3. 해당 칸 안에서의 로컬 좌표 (0~1)
    float localTx = fract(v_Tex.x * float(numDigits));
    
    // 4. 표시할 숫자에서 현재 자릿수의 구체적인 숫자(0~9) 추출
    // 왼쪽부터 추출하기 위해 지수 계산
    float powerOfTen = pow(10.0, float(numDigits) - 1.0 - digitIndex);
    int currentDigit = int(mod(float(numberToDisplay) / powerOfTen, 10.0));

    // 5. 텍스처 시트(5x2) 내 좌표 계산
    // (이미지 시트가 0~4가 윗줄, 5~9가 아랫줄이라고 가정할 때의 로직)
    float tx = localTx / 5.0;
    float ty = v_Tex.y / 2.0;

    float offsetX = float(currentDigit % 5) / 5.0;
    float offsetY = float(currentDigit / 5) / 2.0;

    // 만약 숫자가 아래위로 뒤집혀 있다면 1.0 - (offsetY + ty) 등으로 조정 필요
    vec2 newTex = vec2(tx + offsetX, offsetY + ty);
    
    FragColor = texture(u_NumsTex, newTex);
}

void main()
{
    Num();
}