#version 330 core
out vec4 FragColor;

in vec4 vertexColor;
in vec3 Normal;
in vec3 FragPos;

uniform vec3 lightPos;

void main()
{
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);

    float diff = max(dot(norm, lightDir), 0.1);

    FragColor = vec4(vertexColor.xyz * diff, 1.0);
}