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

        public bool LoadFromObjectFile(string path, bool hasTexture = false)
        {
            if (!File.Exists(path)) return false;

            var lines = File.ReadAllLines(path);
            if (lines.Length <= 0) return false;

            List<Vector3> tempVectors = new();
            List<Vector2> tempTextures = new();

            try
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    switch (line[0])
                    {
                        case 'v':
                            if (line[1] == 't')
                            {
                                // Texture
                                var vertex = line.Split(' ');
                                tempTextures.Add(new Vector2(float.Parse(vertex[1]), float.Parse(vertex[2])));
                            }
                            else
                            {
                                // Vertex
                                var vertex = line.Split(' ');
                                tempVectors.Add(new Vector3(float.Parse(vertex[1]), float.Parse(vertex[2]),
                                    float.Parse(vertex[3])));
                            }
                            break;
                        case 'f':
                            if (hasTexture)
                            {
                                var vectors = new Vector3[3];
                                var texts = new Vector2[3];

                                var index = line.Split(' ');
                                var temp = index.ToList();
                                temp.RemoveAt(0);
                                index = temp.ToArray();
                                for (var i = 0; i < 3; i++)
                                {
                                    var coords = index[i].Split('/');
                                    vectors[i] = tempVectors[int.Parse(coords[0]) - 1];
                                    texts[i] = tempTextures[int.Parse(coords[1]) - 1];
                                }

                                Triangles.Add(new Triangle(vectors, texts));
                            }
                            else
                            {
                                var index = line.Split(' ');
                                var vectors = new Vector3[3]
                                {
                                    tempVectors[int.Parse(index[1]) - 1],
                                    tempVectors[int.Parse(index[2]) - 1],
                                    tempVectors[int.Parse(index[3]) - 1]
                                };
                                Triangles.Add(new Triangle(vectors));
                            }
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
