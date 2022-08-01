using System.Text;

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

                if (head == null)
                {
                    continue;
                }

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

                if (tail == null)
                {
                    continue;
                }

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

            foreach (var otherGraphVertex in otherGraphVertices)
            {
                if (otherGraphVertex == null)
                {
                    throw new ArgumentException($"'{nameof(otherGraphVertices)}' cannot contain nulls.");
                }

                if (graph.Contains(otherGraphVertex))
                {
                    throw new ArgumentException($"Arc already belongs to '{nameof(graph)}'.");
                }

                var captured = otherGraph.Remove(otherGraphVertex);
                if (!captured)
                {
                    throw new ArgumentException($"Arc does not belong to '{nameof(otherGraph)}'.");
                }

                graph.Add(otherGraphVertex);
            }
        }

        public static string PrintGraph(this IGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            var sb = new StringBuilder();

            var vertexNames = graph
                .Select(x => x.Name ?? "<null-name>")
                .OrderBy(x => x)
                .ToList();

            for (var i = 0; i < vertexNames.Count; i++)
            {
                var vertexName = vertexNames[i];
                sb.Append(vertexName);
                if (i < vertexNames.Count - 1)
                {
                    sb.AppendLine();
                }
            }

            var arcTexts = graph
                .SelectMany(x =>
                {
                    var list = new List<IArc>();
                    list.AddRange(x.OutgoingArcs);
                    list.AddRange(x.IncomingArcs);

                    return list;
                })
                .ToHashSet()
                .Select(x => x.PrintArc())
                .OrderBy(x => x)
                .ToList();

            if (arcTexts.Count > 0)
            {
                sb.AppendLine();

                for (var i = 0; i < arcTexts.Count; i++)
                {
                    var arcText = arcTexts[i];
                    sb.Append(arcText);

                    if (i < arcTexts.Count - 1)
                    {
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();
        }

        public static string PrintArc(this IArc arc)
        {
            var sb = new StringBuilder();

            if (arc.Tail != null)
            {
                sb.Append(arc.Tail.Name ?? "<null-name>");
                sb.Append(" ");
            }

            sb.Append("-");
            if (arc.Name != null)
            {
                sb.Append(arc.Name);
            }

            sb.Append("->");

            if (arc.Head != null)
            {
                sb.Append(" ");
                sb.Append(arc.Head.Name ?? "<null-name>");
            }

            return sb.ToString();
        }

        public static IArc DrawArcTo(this IVertex tail, IVertex head)
        {
            var arc = new Arc();
            arc.Connect(tail, head);

            return arc;
        }

        public static IArc DrawArcFrom(this IVertex head, IVertex tail)
        {
            var arc = new Arc();
            arc.Connect(tail, head);

            return arc;
        }
    }
}
