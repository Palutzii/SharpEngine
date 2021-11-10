using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    struct Vector
    {
        public float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector operator *(Vector v, float f)
        {
            return new Vector(v.x * f, v.y * f, v.z * f);
        }

        public static Vector operator +(Vector v, Vector v2)
        {
            return new Vector(v.x + v2.x, v.y + v2.y, v.z + v2.z);
        }

        public static Vector operator -(Vector v, Vector v2)
        {
            return new Vector(v.x - v2.x, v.y - v2.y, v.z - v2.z);
        }

        public static Vector operator /(Vector v, float f)
        {
            return new Vector(v.x / f, v.y / f, v.z / f);
        }
    }
    
    class Program
    {
        static Vector[] vertices = new Vector[] {
            new Vector(-.1f, -.1f),
            new Vector(.1f, -.1f),
            new Vector(0f, .1f),
            new Vector(.4f, .4f),
            new Vector(.6f, .4f),
            new Vector(.5f, .6f),
            
        };
        
        
        static void Main(string[] args) {
            var window = CreateWindow();

            LoadTriangleIntoBuffer();

            CreateShaderProgram();
            
            // engine rendering loop
            while (!Glfw.WindowShouldClose(window)) {
                Glfw.PollEvents(); // react to window changes (position etc.)
                ClearScreen();
                Render(window);
                
                var moveX = .001f;
                var moveY = 0f;
                var moveZ = 0f;
                
                for (var i = 0; i < vertices.Length; i ++ )
                {
                    vertices[i].x += moveX;
                    
                    for (int j = 0; j < vertices.Length; j++)
                    {
                        if (vertices[j].x > 1)
                        {
                            moveX = -moveX; break;
                        }
                    }
                }

                
                UpdateTriangleBuffer();
            }
        }

        private static void ClearScreen()
        {
            glClearColor(.2f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        private static void Render(Window window)
        {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
            Glfw.SwapBuffers(window);
        }

        static void CreateShaderProgram() {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/screen-coordinates.vert"));
            glCompileShader(vertexShader);

            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/green.frag"));
            glCompileShader(fragmentShader);

            // create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }

        static unsafe void LoadTriangleIntoBuffer() {

            // load the vertices into a buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            UpdateTriangleBuffer();
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vector), NULL);

            glEnableVertexAttribArray(0);
        }
        
        static unsafe void UpdateTriangleBuffer() {
            fixed (Vector* vertex = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
        }

        static Window CreateWindow() {
            // initialize and configure
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.True);

            // create and launch a window
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
    }
}