﻿#version 330 core
layout(location=0) in vec3 pos;
layout(location=1) in vec3 vcolor;
out vec3 fcolor;
uniform mat4 MVP;
void main()
{
    gl_Position = MVP*vec4(pos,1);
    fcolor = vcolor;
}

