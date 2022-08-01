using System.Diagnostics;

namespace TauCode.Data.Graphs
{
    [DebuggerDisplay("{Name} ({Tail} -> {Head})")]
    public class Arc : IArc
    {
        #region ctor

        public Arc()
        {
        }

        public Arc(string? name)
        {
            this.Name = name;
        }

        #endregion

        #region IArc Members

        public string? Name { get; set; }

        public IDictionary<string, object>? Properties { get; set; }

        public IVertex? Tail { get; internal set; }

        public IVertex? Head { get; internal set; }

        public void Connect(IVertex tail, IVertex head)
        {
            if (tail == null)
            {
                throw new ArgumentNullException(nameof(tail));
            }

            if (head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

            if (this.Tail != null || this.Head != null)
            {
                throw new InvalidOperationException("Arc is not free.");
            }

            if (!(tail is Vertex tailImpl))
            {
                throw GraphHelper.CreateUnexpectedTypeException(nameof(tail), tail.GetType(), typeof(Vertex));
            }

            if (!(head is Vertex headImpl))
            {
                throw GraphHelper.CreateUnexpectedTypeException(nameof(head), tail.GetType(), typeof(Vertex));
            }

            tailImpl.AddOutgoingArc(this);
            headImpl.AddIncomingArc(this);

            this.Tail = tail;
            this.Head = head;
        }

        public void AttachTail(IVertex tail)
        {
            if (tail == null)
            {
                throw new ArgumentNullException(nameof(tail));
            }

            if (this.Tail != null)
            {
                throw new InvalidOperationException("Arc already has attached tail.");
            }

            if (!(tail is Vertex tailImpl))
            {
                throw GraphHelper.CreateUnexpectedTypeException(nameof(tail), tail.GetType(), typeof(Vertex));
            }

            tailImpl.AddOutgoingArc(this);
            this.Tail = tail;
        }

        public bool DetachTail()
        {
            if (this.Tail != null)
            {
                if (!(this.Tail is Vertex tailVertex))
                {
                    throw GraphHelper.CreateUnexpectedTypeException(
                        nameof(Tail),
                        this.Tail.GetType(),
                        typeof(Vertex));
                }

                tailVertex.RemoveOutgoingArc(this);
                this.Tail = null;

                return true;
            }

            return false;
        }

        public void AttachHead(IVertex head)
        {
            if (head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

            if (this.Head != null)
            {
                throw new InvalidOperationException("Arc already has attached head.");
            }

            if (!(head is Vertex headImpl))
            {
                throw GraphHelper.CreateUnexpectedTypeException(nameof(head), head.GetType(), typeof(Vertex));
            }

            headImpl.AddOutgoingArc(this);
            this.Head = head;
        }

        public bool DetachHead()
        {
            if (this.Head != null)
            {
                if (!(this.Head is Vertex headVertex))
                {
                    throw GraphHelper.CreateUnexpectedTypeException(
                        nameof(Head),
                        this.Head.GetType(),
                        typeof(Vertex));
                }

                headVertex.RemoveIncomingArc(this);
                this.Head = null;

                return true;
            }

            return false;
        }

        public void Disconnect()
        {
            this.DetachHead();
            this.DetachTail();
        }

        #endregion
    }
}
