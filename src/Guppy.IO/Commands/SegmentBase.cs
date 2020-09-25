using Guppy.Lists;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.IO.Commands.Interfaces;
using System.Data;
using System.Linq;
using Guppy.Interfaces;
using Guppy.IO.Commands.Delegates;

namespace Guppy.IO.Commands
{
    /// <summary>
    /// An instance of the data defined by a SegmentContext
    /// object. This will manage sub-segments, parse incoming
    /// command strings, and build Command instances.
    /// </summary>
    public abstract partial class SegmentBase : Service
    {
        #region Private Fields
        private Dictionary<String, Segment> _subSegments;
        private ServiceProvider _provider;
        #endregion

        #region Public Properties
        /// <summary>
        /// The full identifier for the current segment.
        /// </summary>
        public abstract String FullIdentifier { get; }

        /// <summary>
        /// A list of all sub segments contained within
        /// </summary>
        public IReadOnlyDictionary<String, Segment> SubSegments => _subSegments;

        /// <summary>
        /// Public segment getter.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public Segment this[String identifier] => this.SubSegments[identifier];
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _provider = provider;
            _subSegments = new Dictionary<String, Segment>();
        }

        protected override void Release()
        {
            base.Release();

            while (_subSegments.Any())
                this.TryRemove(_subSegments.First().Value);

            _subSegments = null;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Add a new segment context into the current segment.
        /// </summary>
        /// <param name="context"></param>
        public Segment TryAdd(SegmentContext context)
        {
            if (this.SubSegments.ContainsKey(context.Identifier))
                throw new DuplicateNameException($"Unable to create SubSegment '{context.Identifier}' into '{this.FullIdentifier}'. SubSegment alreayd defined.");

            var sub = _provider.GetService<Segment>((s, p, d) =>
            {
                s.Identifier = context.Identifier;
                s.CommandContext = context.Command;
                s.Parent = this;

                context.SubSegments?.ForEach(ss => s.TryAdd(ss));
            });

            _subSegments.Add(sub.Identifier, sub);
            sub.OnReleased += this.HandleSubSegmentReleased;

            return sub;
        }

        /// <summary>
        /// Remove a subsegment from
        /// the current segment
        /// </summary>
        /// <param name="segment"></param>
        public void TryRemove(String segment)
            => this.TryRemove(this.SubSegments[segment]);
        /// <summary>
        /// Remove a subsegment from
        /// the current segment
        /// </summary>
        public void TryRemove(Segment segment)
        {
            if (_subSegments.Remove(segment.Identifier))
            {
                segment.OnReleased -= this.HandleSubSegmentReleased;
                segment.TryRelease();
            }
        }

        /// <summary>
        /// Parse the incoming command array and build
        /// a new CommandInstance based on the recieved
        /// data.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        abstract internal Command TryBuild(String[] input, Int32 position);

        /// <summary>
        /// Attempt to excecute a recieved command instance
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        abstract internal void Execute(Command command);
        #endregion

        #region Event Handlers
        private void HandleSubSegmentReleased(IService sender)
            => this.TryRemove(sender as Segment);
        #endregion
    }
}
