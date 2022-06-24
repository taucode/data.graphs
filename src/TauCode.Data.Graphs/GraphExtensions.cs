using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Data.Graphs
{
    public static class GraphExtensions
    {
        public static IEnumerable<IArc> GetOutgoingArcsLyingInGraph(this IVertex vertex, IGraph graph)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (!graph.Contains(vertex))
            {
                throw new InvalidOperationException("Graph does not contain this vertex.");
            }

            foreach (var outgoingArc in vertex.OutgoingArcs)
            {
                var head = outgoingArc.Head;

                if (graph.Contains(head))
                {
                    yield return outgoingArc;
                }
            }
        }

        public static IEnumerable<IArc> GetIncomingArcsLyingInGraph(this IVertex vertex, IGraph graph)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException(nameof(vertex));
            }

            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (!graph.Contains(vertex))
            {
                throw new InvalidOperationException("Graph does not contain this vertex.");
            }

            foreach (var incomingArc in vertex.IncomingArcs)
            {
                var tail = incomingArc.Tail;

                if (graph.Contains(tail))
                {
                    yield return incomingArc;
                }
            }
        }

        public static IEnumerable<IArc> GetArcs(this IGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            return graph.SelectMany(x => x.GetOutgoingArcsLyingInGraph(graph));
        }

        public static void CaptureVerticesFrom(
            this IGraph graph,
            IGraph otherGraph,
            IEnumerable<IVertex> otherGraphVertices)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            if (otherGraph == null)
            {
                throw new ArgumentNullException(nameof(otherGraph));
            }

            if (otherGraphVertices == null)
            {
                throw new ArgumentNullException(nameof(otherGraphVertices));
            }

            var idx = 0;

            foreach (var otherGraphVertex in otherGraphVertices)
            {
                if (otherGraphVertex == null)
                {
                    throw new ArgumentException($"'{nameof(otherGraphVertices)}' cannot contain nulls.");
                }

                if (graph.Contains(otherGraphVertex))
                {
                    // todo: 'index' is not applicable to ISet<T>.
                    throw new ArgumentException($"Arc with index {idx} already belongs to '{nameof(graph)}'.");
                }

                var captured = otherGraph.Remove(otherGraphVertex);
                if (!captured)
                {
                    throw new ArgumentException($"Arc with index {idx} does not belong to '{nameof(otherGraph)}'.");
                }

                graph.Add(otherGraphVertex);

                idx++;
            }
        }
    }
}
