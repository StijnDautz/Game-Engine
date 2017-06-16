#version 330
 
// input
in vec2 vUV;					// vertex uv coordinate
in vec3 vNormal;				// untransformed normalized vertex normal
in vec3 vPosition;				// untransformed vertex position
in vec3 vTangent;				// untransformed normalized tangent vector
uniform mat4 toWorld;			// toWorldSpace matrix
uniform mat4 toScreen;			// toScreenSpace matrix (equal to toWorld * perspective)
uniform vec3 camerapos;			// cameraPosition in world space
uniform float[12] Alightpos;	// light positions in world space
uniform int lightcount;			// light count

// output
// out vec4 normal;				// transformed vertex normal
out vec2 uv;					// uv texture coordinates
out vec3 worldPos;				// vertex position
out vec3 cameraPosition;		// camera position
out vec3[4] lightpositions;		// light positions

// vertex shader
void main()
{
	// transfrom objects to world space and pass them to the fragment shader
	/// tangent
	vec3 tangent = normalize(vec3(toWorld * vec4(vTangent, 0)).xyz);

	/// normal
	vec3 normal = normalize(vec3(toWorld * vec4( vNormal, 0)).xyz);

	/// bitangent
	vec3 bitangent = normalize(cross(tangent, normal));

	mat3 toTangentSpace = transpose(mat3(tangent, bitangent, normal));

	/// vertex
	worldPos = (toWorld * vec4(vPosition, 0)).xyz * toTangentSpace;

	/// pass cameraposition to the fragment shader
	cameraPosition = camerapos * toTangentSpace;

	/// lights
	int c = 0;
	for(int i = 0; i < lightcount * 3; i+=3, c++)
	{
			// reconstruct vector3 from float[3]
			lightpositions[c] = vec3(Alightpos[i], Alightpos[i+1], Alightpos[i+2]) * toTangentSpace;
	}	

	// pass uv to the fragment shader
	uv = vUV;

	// compute the pixel on the screen to draw to
	gl_Position = toScreen * vec4(vPosition, 1.0);
}