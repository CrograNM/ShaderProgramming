#include "stdafx.h"
#include "Renderer.h"

#include <vector>
#include <ctime>

Renderer::Renderer(int windowSizeX, int windowSizeY)
{
	Initialize(windowSizeX, windowSizeY);
}


Renderer::~Renderer()
{
}

void Renderer::Initialize(int windowSizeX, int windowSizeY)
{
	//Set window size
	m_WindowSizeX = windowSizeX;
	m_WindowSizeY = windowSizeY;

	//Load shaders
	m_SolidRectShader = CompileShaders("./Shaders/SolidRect.vs", "./Shaders/SolidRect.fs");
	// m_TriangleShader = CompileShaders("./Shaders/Triangle.vs", "./Shaders/Triangle.fs");
	m_ParticleShader = CompileShaders("./Shaders/Triangle.vs", "./Shaders/Triangle.fs");

	//Create VBOs
	CreateVertexBufferObjects();

	GenParticle(300);
	
	if (m_SolidRectShader > 0 && m_VBORect > 0)
	{
		m_Initialized = true;
	}
}

bool Renderer::IsInitialized()
{
	return m_Initialized;
}

void Renderer::CreateVertexBufferObjects()
{
	float rect[]
		=
	{
		-1.f / m_WindowSizeX, -1.f / m_WindowSizeY, 0.f, -1.f / m_WindowSizeX, 1.f / m_WindowSizeY, 0.f, 1.f / m_WindowSizeX, 1.f / m_WindowSizeY, 0.f, //Triangle1
		-1.f / m_WindowSizeX, -1.f / m_WindowSizeY, 0.f,  1.f / m_WindowSizeX, 1.f / m_WindowSizeY, 0.f, 1.f / m_WindowSizeX, -1.f / m_WindowSizeY, 0.f, //Triangle2
	};

	glGenBuffers(1, &m_VBORect);
	glBindBuffer(GL_ARRAY_BUFFER, m_VBORect);
	glBufferData(GL_ARRAY_BUFFER, sizeof(rect), rect, GL_STATIC_DRAW);


	float centerX = 0;
	float centerY = 0;
	float size = 0.1f;
	
	float mass = 1;
	float vx = 1;
	float vy = 1;
	float rv = 1;
	
	float particle []
		=
	{
		centerX - size/2, centerY - size/2,		0,
		mass, vx, vy, rv,	// v0
		centerX + size/2, centerY - size/2,		0,
		mass, vx, vy, rv,	// v1
		centerX + size/2, centerY + size/2,		0,	// Triangle1	
		mass, vx, vy, rv,	// v2
		
		centerX - size/2, centerY - size/2, 	0,
		mass, vx, vy, rv,	// v3
		centerX - size/2, centerY + size/2,		0,
		mass, vx, vy, rv,	// v4
		centerX + size/2, centerY + size/2,		0,	// Triangle2
		mass, vx, vy, rv	// v5
	};
	glGenBuffers(1, &m_VBOParticle);
	glBindBuffer(GL_ARRAY_BUFFER, m_VBOParticle);
	glBufferData(GL_ARRAY_BUFFER, sizeof(particle), particle, GL_STATIC_DRAW);

	// 동기 : 버퍼 데이터가 GPU로 전송되고 나서 리턴
	// 비동기 : 바로 리턴
}

void Renderer::AddShader(GLuint ShaderProgram, const char* pShaderText, GLenum ShaderType)
{
	//쉐이더 오브젝트 생성
	GLuint ShaderObj = glCreateShader(ShaderType);

	if (ShaderObj == 0) {
		fprintf(stderr, "Error creating shader type %d\n", ShaderType);
	}

	const GLchar* p[1];
	p[0] = pShaderText;
	GLint Lengths[1];
	Lengths[0] = strlen(pShaderText);
	//쉐이더 코드를 쉐이더 오브젝트에 할당
	glShaderSource(ShaderObj, 1, p, Lengths);

	//할당된 쉐이더 코드를 컴파일
	glCompileShader(ShaderObj);

	GLint success;
	// ShaderObj 가 성공적으로 컴파일 되었는지 확인
	glGetShaderiv(ShaderObj, GL_COMPILE_STATUS, &success);
	if (!success) {
		GLchar InfoLog[1024];

		//OpenGL 의 shader log 데이터를 가져옴
		glGetShaderInfoLog(ShaderObj, 1024, NULL, InfoLog);
		fprintf(stderr, "Error compiling shader type %d: '%s'\n", ShaderType, InfoLog);
		printf("%s \n", pShaderText);
	}

	// ShaderProgram 에 attach!!
	glAttachShader(ShaderProgram, ShaderObj);
}

bool Renderer::ReadFile(char* filename, std::string *target)
{
	std::ifstream file(filename);
	if (file.fail())
	{
		std::cout << filename << " file loading failed.. \n";
		file.close();
		return false;
	}
	std::string line;
	while (getline(file, line)) {
		target->append(line.c_str());
		target->append("\n");
	}
	return true;
}

GLuint Renderer::CompileShaders(char* filenameVS, char* filenameFS)
{
	GLuint ShaderProgram = glCreateProgram(); //빈 쉐이더 프로그램 생성

	if (ShaderProgram == 0) { //쉐이더 프로그램이 만들어졌는지 확인
		fprintf(stderr, "Error creating shader program\n");
	}

	std::string vs, fs;

	//shader.vs 가 vs 안으로 로딩됨
	if (!ReadFile(filenameVS, &vs)) {
		printf("Error compiling vertex shader\n");
		return -1;
	};

	//shader.fs 가 fs 안으로 로딩됨
	if (!ReadFile(filenameFS, &fs)) {
		printf("Error compiling fragment shader\n");
		return -1;
	};

	// ShaderProgram 에 vs.c_str() 버텍스 쉐이더를 컴파일한 결과를 attach함
	AddShader(ShaderProgram, vs.c_str(), GL_VERTEX_SHADER);

	// ShaderProgram 에 fs.c_str() 프레그먼트 쉐이더를 컴파일한 결과를 attach함
	AddShader(ShaderProgram, fs.c_str(), GL_FRAGMENT_SHADER);

	GLint Success = 0;
	GLchar ErrorLog[1024] = { 0 };

	//Attach 완료된 shaderProgram 을 링킹함
	glLinkProgram(ShaderProgram);

	//링크가 성공했는지 확인
	glGetProgramiv(ShaderProgram, GL_LINK_STATUS, &Success);

	if (Success == 0) {
		// shader program 로그를 받아옴
		glGetProgramInfoLog(ShaderProgram, sizeof(ErrorLog), NULL, ErrorLog);
		std::cout << filenameVS << ", " << filenameFS << " Error linking shader program\n" << ErrorLog;
		return -1;
	}

	glValidateProgram(ShaderProgram);
	glGetProgramiv(ShaderProgram, GL_VALIDATE_STATUS, &Success);
	if (!Success) {
		glGetProgramInfoLog(ShaderProgram, sizeof(ErrorLog), NULL, ErrorLog);
		std::cout << filenameVS << ", " << filenameFS << " Error validating shader program\n" << ErrorLog;
		return -1;
	}

	glUseProgram(ShaderProgram);
	std::cout << filenameVS << ", " << filenameFS << " Shader compiling is done.";

	return ShaderProgram;
}

void Renderer::DrawSolidRect(float x, float y, float z, float size, float r, float g, float b, float a)
{
	float newX, newY;

	GetGLPosition(x, y, &newX, &newY);

	//Program select
	glUseProgram(m_SolidRectShader);

	glUniform4f(glGetUniformLocation(m_SolidRectShader, "u_Trans"), newX, newY, 0, size);
	glUniform4f(glGetUniformLocation(m_SolidRectShader, "u_Color"), r, g, b, a);

	int attribPosition = glGetAttribLocation(m_SolidRectShader, "a_Position");
	glEnableVertexAttribArray(attribPosition);
	glBindBuffer(GL_ARRAY_BUFFER, m_VBORect);
	glVertexAttribPointer(attribPosition, 3, GL_FLOAT, GL_FALSE, sizeof(float) * 3, 0);

	glDrawArrays(GL_TRIANGLES, 0, 6);

	glDisableVertexAttribArray(attribPosition);

	glBindFramebuffer(GL_FRAMEBUFFER, 0);
}

float g_Time = 0;

void Renderer::DrawParticle()
{
	g_Time += 0.0001f; // 테스트용

	//Program select
	glUseProgram(m_ParticleShader);

	int u_Time = glGetUniformLocation(
		m_ParticleShader, "u_Time");
	glUniform1f(u_Time, g_Time);

	// Attribute
	int attribPosition = glGetAttribLocation(m_ParticleShader, "a_Position");
	int attribMass = glGetAttribLocation(m_ParticleShader, "a_Mass");
	int attribVel = glGetAttribLocation(m_ParticleShader, "a_Vel");		// 속도
	int attribRv = glGetAttribLocation(m_ParticleShader, "a_RV");			// Random Value
	int attribRv1 = glGetAttribLocation(m_ParticleShader, "a_RV1");		// Random Value 1
	int attribRv2 = glGetAttribLocation(m_ParticleShader, "a_RV2");		// Random Value 2
	glEnableVertexAttribArray(attribPosition);
	glEnableVertexAttribArray(attribMass);
	glEnableVertexAttribArray(attribVel);
	glEnableVertexAttribArray(attribRv);
	glEnableVertexAttribArray(attribRv1);
	glEnableVertexAttribArray(attribRv2);

	glBindBuffer(GL_ARRAY_BUFFER, m_VBOParticle);
	
	// Stride : 7 * sizeof(float) 정점 데이터 크기 (x, y, z, mass, vx, vy, RV)
	glVertexAttribPointer(attribPosition, 3, GL_FLOAT, GL_FALSE, 9 * sizeof(float), 0);
	glVertexAttribPointer(attribMass, 1, GL_FLOAT, GL_FALSE, 9 * sizeof(float), (GLvoid*)(3 * sizeof(float)));
	glVertexAttribPointer(attribVel, 2, GL_FLOAT, GL_FALSE, 9 * sizeof(float), (GLvoid*)(4 * sizeof(float)));
	glVertexAttribPointer(attribRv, 1, GL_FLOAT, GL_FALSE, 9 * sizeof(float), (GLvoid*)(6 * sizeof(float)));
	glVertexAttribPointer(attribRv1, 1, GL_FLOAT, GL_FALSE, 9 * sizeof(float), (GLvoid*)(7 * sizeof(float)));
	glVertexAttribPointer(attribRv2, 1, GL_FLOAT, GL_FALSE, 9 * sizeof(float), (GLvoid*)(8 * sizeof(float)));
	
	glDrawArrays(GL_TRIANGLES, 0, m_ParticleCount * 6);
	
	glDisableVertexAttribArray(attribPosition);
	glDisableVertexAttribArray(attribMass);
	glDisableVertexAttribArray(attribVel);
	glDisableVertexAttribArray(attribRv);
	glDisableVertexAttribArray(attribRv1);
	glDisableVertexAttribArray(attribRv2);
}

void Renderer::GetGLPosition(float x, float y, float *newX, float *newY)
{
	*newX = x * 2.f / m_WindowSizeX;
	*newY = y * 2.f / m_WindowSizeY;
}

void Renderer::GenParticle(int num)
{
	m_ParticleCount = num;
	srand((unsigned int)time(NULL)); 

	std::vector<float> particles;
	float size = 0.02f; 
	float mass = 1.0f;

	for (int i = 0; i < num; i++)
	{
		float vx = 0;
		float vy = ((float)rand() / RAND_MAX);

		float rv = (float)rand() / RAND_MAX; // 랜덤 값 (0.0 ~ 1.0)
		float rv1 = (float)rand() / RAND_MAX; // 랜덤 값 (0.0 ~ 1.0)
		float rv2 = 3.0;

		float centerX = 0.0f;
		float centerY = 0.0f;

		float p[] = {
			centerX - size / 2, centerY - size / 2, 0, mass, vx, vy, rv, rv1, rv2,
			centerX + size / 2, centerY - size / 2, 0, mass, vx, vy, rv, rv1, rv2,
			centerX + size / 2, centerY + size / 2, 0, mass, vx, vy, rv, rv1, rv2,

			centerX - size / 2, centerY - size / 2, 0, mass, vx, vy, rv, rv1, rv2,
			centerX + size / 2, centerY + size / 2, 0, mass, vx, vy, rv, rv1, rv2,
			centerX - size / 2, centerY + size / 2, 0, mass, vx, vy, rv, rv1, rv2
		};

		int pSize = sizeof(p) / sizeof(float); // p 배열의 요소 개수 계산
		for (int j = 0; j < pSize; j++) { 
			particles.push_back(p[j]);
		}
	}

	// VBO                    
	if (m_VBOParticle == 0) {
		glGenBuffers(1, &m_VBOParticle);
	}
	glBindBuffer(GL_ARRAY_BUFFER, m_VBOParticle);
	glBufferData(GL_ARRAY_BUFFER, particles.size() * sizeof(float), particles.data(), GL_STATIC_DRAW);
}