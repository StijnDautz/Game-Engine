using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{

    class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        SceneGraphObject teapot, floor;
        const float PI = 3.1415926535f;         // PI
        float a = 0;                            // teapot rotation angle
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Shader postproc;                        // shader to use for post processing
        RenderTarget target;                    // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing
        bool useRenderTarget = true;
        SceneGraph sceneGraph;

        // initialize
        public void Init()
        {
            // create SceneGraph
            sceneGraph = new SceneGraph();
            // create sceneGraphObjects
            floor = new SceneGraphObject();
            teapot = new SceneGraphObject();
            // load meshes and add them to actors
            floor.setMesh("assets/floor.obj");
            teapot.setMesh("assets/teapot.obj");
            // load a texture
            teapot.Mesh.Texture = "assets/wood.jpg";
            floor.Mesh.Texture = "assets/wood.jpg";
            // create camera
            Camera camera = new Camera();
            // add scenegraphobjects to scenegraph
            sceneGraph.Add(floor);
            sceneGraph.Add(teapot);
            sceneGraph.Add(camera);
            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            // prepare matrix for vertex shader
            floor.transform.Clear();
            floor.transform.Rotate(new Vector3(0, -1, 0), a);
            teapot.transform.Clear();
            teapot.transform.Translate(new Vector3(0, 10, 5));
            teapot.transform.Rotate(new Vector3(0, -1, 0), a);

            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            if (useRenderTarget)
            {
                // enable render target
                target.Bind();

                // render scene to render target
                sceneGraph.Render(shader);

                // render quad
                target.Unbind();
                quad.Render(postproc, target.GetTextureID());
            }
            else
            {
                // render scene directly to the screen
                sceneGraph.Render(shader);
            }
        }
    }
}