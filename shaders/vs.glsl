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
out vec3 tangent;				// normalized tangent vector

// vertex shader
void main()
{
	// transfrom objects to world space and pass them to the fragment shader

	/// tangent
	tangent = normalize(vec3(toWorld * vec4(vTangent, 0)).xyz);

	/// normal
	vec3 normal = normalize(vec3(toWorld * vec4( vNormal, 0.0f )).xyz);

	/// bitangent
	vec3 bitangent = normalize(cross(normal, tangent));

	mat3 tangentSpace = mat3
					( tangent.x, tangent.y, tangent.z,
					bitangent.x, bitangent.y, bitangent.z,
					normal.x, normal.y, normal.z );

	/// vertex
	worldPos = (toWorld * vec4(vPosition, 0)).xyz * tangentSpace;


	/// pass cameraposition to the fragment shader
	cameraPosition = camerapos * tangentSpace;

	/// lights
	int c = 0;
	for(int i = 0; i < lightcount * 3; i+=3, c++)
	{
			// reconstruct vector3 from float[3]
			lightpositions[c] = tangentSpace * vec3(Alightpos[i], Alightpos[i+1], Alightpos[i+2]);
	}	

	// pass uv to the fragment shader
	uv = vUV;

	// compute the pixel on the screen to draw to
	gl_Position = toScreen * vec4(vPosition, 1.0);
}