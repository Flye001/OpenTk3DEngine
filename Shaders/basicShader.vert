#version 330 core
layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec3 inColor;
layout (location = 3) in vec2 inTexCoord;

out vec4 vertexColor;
out vec3 Normal;
out vec3 FragPos;
out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat3 modelNormals;

void main()
{
    gl_Position = vec4(inPos, 1.0) * model * view * projection;
    FragPos = vec3(vec4(inPos, 1.0) * model);
    Normal = inNormal * modelNormals;

    vertexColor = vec4(inColor, 1.0);
    texCoord = inTexCoord;
}