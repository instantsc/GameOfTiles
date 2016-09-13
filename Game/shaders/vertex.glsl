#version 330 core
in vec3 pos;
in vec3 vcolor;
out vec3 fcolor;
uniform mat4 MVP;
void main()
{
    gl_Position = MVP*vec4(pos,1);
    fcolor = vcolor;
}

