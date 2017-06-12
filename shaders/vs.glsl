#version 330
 
// shader input
in vec2 vUV;				// vertex uv coordinate
in vec3 vNormal;			// untransformed vertex normal
in vec3 vPosition;			// untransformed vertex position

// shader output
out vec4 normal;			// transformed vertex normal
out vec2 uv;	
out vec3 worldPos;

uniform mat4 toWorld;
uniform mat4 mCamera;

// vertex shader
void main()
{
	// transform vertex using supplied matrix
	worldPos = (vec4(vPosition, 0) * toWorld).xyz;
	gl_Position = (mCamera * toWorld) * vec4(vPosition, 1.0);

	// forward normal and uv coordinate; will be interpolated over triangle
	normal = toWorld * vec4( vNormal, 0.0f );
	uv = vUV;
}