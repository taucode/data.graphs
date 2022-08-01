using System.Collections;

namespace TauCode.Data.Graphs
{
    public class Graph : IGraph
    {
        #region Fields

        private readonly HashSet<IVertex> _vertices;

        #endregion

        #region ctor

        public Graph()
        {
            _vertices = new HashSet<IVertex>();
        }

        public Graph(IEnumerable<IVertex> vertices)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            _vertices = new HashSet<IVertex>(vertices);
        }

        #endregion

        #region Pvivate

        private void AddPrivate(IVertex vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            if (_vertices.Contains(vertex))
            {
                throw new InvalidOperationException("Graph already contains this vertex.");
            }

            _vertices.Add(vertex);
        }

        #endregion

        #region ISet<IVertex> Members

        bool ISet<IVertex>.Add(IVertex vertex)
        {
            this.AddPrivate(vertex);
            return true;
        }

        public void ExceptWith(IEnumerable<IVertex> other) => _vertices.ExceptWith(other);

        public void IntersectWith(IEnumerable<IVertex> other) => _vertices.IntersectWith(other);

        public bool IsProperSubsetOf(IEnumerable<IVertex> other) => _vertices.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<IVertex> other) => _vertices.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<IVertex> other) => _vertices.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<IVertex> other) => _vertices.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<IVertex> other) => _vertices.Overlaps(other);

        public bool SetEquals(IEnumerable<IVertex> other) => _vertices.SetEquals(other);

        public void SymmetricExceptWith(IEnumerable<IVertex> other) => _vertices.SymmetricExceptWith(other);

        public void UnionWith(IEnumerable<IVertex> other) => _vertices.UnionWith(other);

        #endregion

        #region ICollection<IVertex> Members

        public int Count => _vertices.Count;

        public bool IsReadOnly => ((ICollection<IVertex>)_vertices).IsReadOnly;

        public void Add(IVertex vertex) => this.AddPrivate(vertex);

        public void Clear() => _vertices.Clear();

        public bool Contains(IVertex vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            return _vertices.Contains(vertex);
        }

        public void CopyTo(IVertex[] array, int arrayIndex) => _vertices.CopyTo(array, arrayIndex);

        public bool Remove(IVertex vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            return _vertices.Remove(vertex);
        }

        #endregion

        #region IEnumerable<IVertex> Members

        public IEnumerator<IVertex> GetEnumerator() => _vertices.GetEnumerator();

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
