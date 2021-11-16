namespace SharpEngine
{
    public class Rectangle : Shape 
    {
        public Rectangle(Material material) : base(CreateRectangle(), material) 
        {
            
        }

        static Vertex[] CreateRectangle()
        {
            const float scale = .1f;
            return new Vertex[] 
            {
                new Vertex(new Vector(-scale, -scale), Color.Red), //Left bottom
                new Vertex(new Vector(scale, -scale), Color.Green), //RB
                new Vertex(new Vector(-scale, scale), Color.Blue), //Left Top
                new Vertex(new Vector(scale, -scale), Color.Green), //RB
                new Vertex(new Vector(scale, scale), Color.Red), //RT
                new Vertex(new Vector(-scale, scale), Color.Blue) //LT
            };
        }
    }
}