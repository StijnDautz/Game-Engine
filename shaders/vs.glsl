#version 330
 
// input
in vec2 vUV;					// vertex uv coordinate
in vec3 vNormal;				// untransformed vertex normal
in vec3 vPosition;				// untransformed vertex position
uniform mat4 toWorld;			// toWorldSpace matrix
uniform mat4 toScreen;			// toScreenSpace matrix (equal to toWorld * perspective)
uniform vec3 camerapos;			// cameraPosition in world space
uniform float[12] Alightpos;	// light positions in world space
uniform int lightcount;			// light count

// output
out vec4 normal;				// transformed vertex normal
out vec2 uv;					// uv texture coordinates
out vec3 worldPos;				// vertex position
out vec3 cameraPosition;		// camera position
out vec3[4] lightpositions;		// light positions

// vertex shader
void main()
{
	// transfrom objects to world space and pass them to the fragment shader
	/// lights
	int c = 0;
	for(int i = 0; i < lightcount * 3; i+=3, c++)
	{
			// reconstruct vector3 from float[3]
			lightpositions[c] = vec3(Alightpos[i], Alightpos[i+1], Alightpos[i+2]);
	}
	/// vertex
	worldPos = (vec4(vPosition, 0) * toWorld).xyz;

	/// normal
	normal = toWorld * vec4( vNormal, 0.0f );

	/// pass cameraposition to the fragment shader
	cameraPosition = camerapos;

	// pass uv to the fragment shader
	uv = vUV;

	// compute the pixel on the screen to draw to
	gl_Position = toScreen * vec4(vPosition, 1.0);

}