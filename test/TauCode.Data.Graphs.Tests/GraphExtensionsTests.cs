using NUnit.Framework;

namespace TauCode.Data.Graphs.Tests;

[TestFixture]
public class GraphExtensionsTests
{
    [Test]
    public void DrawArcTo_ValidArguments_DrawsArc()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        // Act

        // Assert
        var arc = a.DrawArcTo(b);

        Assert.That(arc.Tail, Is.SameAs(a));
        Assert.That(arc.Head, Is.SameAs(b));

        Assert.That(a.OutgoingArcs, Has.Count.EqualTo(1));
        Assert.That(a.OutgoingArcs.Single(), Is.SameAs(arc));
        Assert.That(a.IncomingArcs, Is.Empty);

        Assert.That(b.IncomingArcs, Has.Count.EqualTo(1));
        Assert.That(b.IncomingArcs.Single(), Is.SameAs(arc));
        Assert.That(b.OutgoingArcs, Is.Empty);

        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
a --> b
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    [Test]
    public void DrawArcFrom_ValidArguments_DrawsArc()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        // Act

        // Assert
        var arc = a.DrawArcFrom(b);

        Assert.That(arc.Tail, Is.SameAs(b));
        Assert.That(arc.Head, Is.SameAs(a));

        Assert.That(b.OutgoingArcs, Has.Count.EqualTo(1));
        Assert.That(b.OutgoingArcs.Single(), Is.SameAs(arc));
        Assert.That(b.IncomingArcs, Is.Empty);

        Assert.That(a.IncomingArcs, Has.Count.EqualTo(1));
        Assert.That(a.IncomingArcs.Single(), Is.SameAs(arc));
        Assert.That(a.OutgoingArcs, Is.Empty);

        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
b --> a
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }
}