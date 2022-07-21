using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace TauCode.Data.Graphs.Tests;

[TestFixture]
public class ArcTests
{
    [Test]
    public void Constructor_NoArguments_CreatesArc()
    {
        // Arrange

        // Act
        IArc arc = new Arc();

        // Assert
        Assert.That(arc.Tail, Is.Null);
        Assert.That(arc.Head, Is.Null);
        Assert.That(arc.Name, Is.Null);
        Assert.That(arc.Properties, Is.Null);
    }

    #region Connect

    [Test]
    public void Connect_ValidArguments_ConnectsVertices()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        // Act
        IArc arc = new Arc();
        arc.Connect(a, b);

        // Assert
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
    public void Connect_CalledTwice_ThrowsInvalidOperationException()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        IArc arc = new Arc();
        arc.Connect(a, b);

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => arc.Connect(a, b));

        // Assert
        Assert.That(ex.Message, Is.EqualTo("Arc is not free."));
    }

    [Test]
    public void Connect_TwoArcs_ConnectsTwice()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");


        // Act
        IArc arc1 = new Arc();
        arc1.Connect(a, b);

        IArc arc2 = new Arc();
        arc2.Connect(a, b);

        // Assert
        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
a --> b
a --> b
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    [Test]
    public void Connect_MutualConnect_ConnectsMutually()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");


        // Act
        IArc arc1 = new Arc();
        arc1.Connect(a, b);

        IArc arc2 = new Arc();
        arc2.Connect(b, a);

        // Assert
        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
a --> b
b --> a
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }


    #endregion

    #region AttachTail

    [Test]
    public void AttachTail_ValidArgument_AttachesTail()
    {
        // Arrange
        var a = new Vertex("a");


        // Act
        IArc arc = new Arc();
        arc.AttachTail(a);


        // Assert
        var graph = new Graph(new IVertex[] { a, });
        var text = graph.PrintGraph();

        var expectedText = @"
a
a -->
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    [Test]
    public void AttachTail_ArgumentIsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        IArc arc = new Arc();

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => arc.AttachTail(null));

        // Assert
        Assert.That(ex.ParamName, Is.EqualTo("tail"));
    }

    [Test]
    public void AttachTail_TailAlreadyAttached_ThrowsInvalidOperationException()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        IArc arc = new Arc();
        arc.AttachTail(a);

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => arc.AttachTail(b));

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Arc already has attached tail."));
    }

    [Test]
    public void AttachTail_TailIsSame_ThrowsInvalidOperationException()
    {
        // Arrange
        var a = new Vertex("a");

        IArc arc = new Arc();
        arc.AttachTail(a);

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => arc.AttachTail(a));

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Arc already has attached tail."));
    }

    [Test]
    public void AttachTail_TailIsOfWrongType_ThrowsInvalidCastException()
    {
        // Arrange
        var mock = new Mock<IVertex>();
        var a = mock.Object;

        IArc arc = new Arc();

        // Act
        var ex = Assert.Throws<InvalidCastException>(() => arc.AttachTail(a));

        // Assert
        Assert.That(ex, Has.Message.EqualTo("'tail' is of type 'Castle.Proxies.IVertexProxy'. Expected type: 'TauCode.Data.Graphs.Vertex'."));
    }


    #endregion

    #region AttachHead

    [Test]
    public void AttachHead_ValidArgument_AttachesHead()
    {
        // Arrange
        var a = new Vertex("a");


        // Act
        IArc arc = new Arc();
        arc.AttachHead(a);


        // Assert
        var graph = new Graph(new IVertex[] { a, });
        var text = graph.PrintGraph();

        var expectedText = @"
a
--> a
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    [Test]
    public void AttachHead_ArgumentIsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        IArc arc = new Arc();

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => arc.AttachHead(null));

        // Assert
        Assert.That(ex.ParamName, Is.EqualTo("head"));
    }

    [Test]
    public void AttachHead_HeadAlreadyAttached_ThrowsInvalidOperationException()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        IArc arc = new Arc();
        arc.AttachHead(a);

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => arc.AttachHead(b));

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Arc already has attached head."));
    }

    [Test]
    public void AttachHead_HeadIsSame_ThrowsInvalidOperationException()
    {
        // Arrange
        var a = new Vertex("a");

        IArc arc = new Arc();
        arc.AttachHead(a);

        // Act
        var ex = Assert.Throws<InvalidOperationException>(() => arc.AttachHead(a));

        // Assert
        Assert.That(ex, Has.Message.EqualTo("Arc already has attached head."));
    }

    [Test]
    public void AttachHead_HeadIsOfWrongType_ThrowsInvalidCastException()
    {
        // Arrange
        var mock = new Mock<IVertex>();
        var a = mock.Object;

        IArc arc = new Arc();

        // Act
        var ex = Assert.Throws<InvalidCastException>(() => arc.AttachHead(a));

        // Assert
        Assert.That(ex, Has.Message.EqualTo("'head' is of type 'Castle.Proxies.IVertexProxy'. Expected type: 'TauCode.Data.Graphs.Vertex'."));
    }


    #endregion

    #region DetachTail

    [Test]
    public void DetachTail_NoArguments_DetachesTail()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        IArc arc = new Arc();
        arc.Connect(a, b);

        // Act
        var detached = arc.DetachTail();

        // Assert
        Assert.That(detached, Is.True);

        Assert.That(arc.Tail, Is.Null);
        Assert.That(arc.Head, Is.SameAs(b));

        Assert.That(a.OutgoingArcs, Is.Empty);
        Assert.That(a.IncomingArcs, Is.Empty);

        Assert.That(b.IncomingArcs, Has.Count.EqualTo(1));
        Assert.That(b.IncomingArcs.Single(), Is.SameAs(arc));
        Assert.That(b.OutgoingArcs, Is.Empty);

        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
--> b
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    [Test]
    public void DetachTail_CalledTwice_DoesNothing()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        IArc arc = new Arc();
        arc.Connect(a, b);
        arc.DetachTail();

        // Act
        var detached = arc.DetachTail();

        // Assert
        Assert.That(detached, Is.False);

        Assert.That(arc.Tail, Is.Null);
        Assert.That(arc.Head, Is.SameAs(b));

        Assert.That(a.OutgoingArcs, Is.Empty);
        Assert.That(a.IncomingArcs, Is.Empty);

        Assert.That(b.IncomingArcs, Has.Count.EqualTo(1));
        Assert.That(b.IncomingArcs.Single(), Is.SameAs(arc));
        Assert.That(b.OutgoingArcs, Is.Empty);

        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
--> b
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    #endregion

    #region DetachHead

    [Test]
    public void DetachHead_NoArguments_DetachesHead()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        IArc arc = new Arc();
        arc.Connect(a, b);

        // Act
        var detached = arc.DetachHead();

        // Assert
        Assert.That(detached, Is.True);

        Assert.That(arc.Tail, Is.SameAs(a));
        Assert.That(arc.Head, Is.Null);

        Assert.That(a.OutgoingArcs, Has.Count.EqualTo(1));
        Assert.That(a.OutgoingArcs.Single(), Is.SameAs(arc));
        Assert.That(a.IncomingArcs, Is.Empty);

        Assert.That(b.IncomingArcs, Is.Empty);
        Assert.That(b.OutgoingArcs, Is.Empty);

        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
a -->
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    [Test]
    public void DetachHead_CalledTwice_DoesNothing()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");

        IArc arc = new Arc();
        arc.Connect(a, b);
        arc.DetachHead();

        // Act
        var detached = arc.DetachHead();

        // Assert
        Assert.That(detached, Is.False);

        Assert.That(arc.Tail, Is.SameAs(a));
        Assert.That(arc.Head, Is.Null);

        Assert.That(a.OutgoingArcs, Has.Count.EqualTo(1));
        Assert.That(a.OutgoingArcs.Single(), Is.SameAs(arc));
        Assert.That(a.IncomingArcs, Is.Empty);

        Assert.That(b.IncomingArcs, Is.Empty);
        Assert.That(b.OutgoingArcs, Is.Empty);

        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
a -->
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    #endregion

    #region Disconnect

    [Test]
    public void Disconnect_NoArguments_DisconnectsVertices()
    {
        // Arrange
        var a = new Vertex("a");
        var b = new Vertex("b");
        IArc arc = new Arc();
        arc.Connect(a, b);

        // Act
        arc.Disconnect();

        // Assert
        Assert.That(arc.Tail, Is.Null);
        Assert.That(arc.Head, Is.Null);

        Assert.That(a.OutgoingArcs, Is.Empty);
        Assert.That(a.IncomingArcs, Is.Empty);

        Assert.That(b.IncomingArcs, Is.Empty);
        Assert.That(b.OutgoingArcs, Is.Empty);

        var graph = new Graph(new IVertex[] { a, b });
        var text = graph.PrintGraph();

        var expectedText = @"
a
b
";
        Assert.That(text, Is.EqualTo(expectedText.Trim()));
    }

    #endregion
}
