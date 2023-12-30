using OpenTK.Mathematics;

namespace OpenTkEngine
{
    internal struct Mesh
    {
        public List<Triangle> Triangles;

        public Mesh()
        {
            Triangles = new List<Triangle>();
        }

        public bool LoadFromObjectFile(string path)
        {
            if (!File.Exists(path)) return false;

            var lines = File.ReadAllLines(path);
            if (lines.Length <= 0) return false;

            List<Vector3> tempVectors = new();

            try
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    switch (line[0])
                    {
                        case 'v':
                            // Vertex
                            var vertex = line.Split(' ');
                            tempVectors.Add(new Vector3(float.Parse(vertex[1]), float.Parse(vertex[2]),
                                float.Parse(vertex[3])));
                            break;
                        case 'f':
                            var index = line.Split(' ');
                            var vectors = new Vector3[3]
                            {
                                tempVectors[int.Parse(index[1]) - 1],
                                tempVectors[int.Parse(index[2]) - 1],
                                tempVectors[int.Parse(index[3]) - 1]
                            };
                            Triangles.Add(new Triangle(vectors));
                            break;
                    }
                }
            }
            catch
            {
                return false;
            }


            return true;
        }
    }
}
