using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Effects
{
    /// <summary>
    /// A simple implementation of the <see cref="IEffectMatrices"/> interface that
    /// assumes the recieving <see cref="Effect"/> contains a WorldViewProjection
    /// <see cref="Matrix"/> <see cref="EffectParameter"/>.
    /// </summary>
    public abstract class EffectMatricesEffect : Effect, IEffectMatrices
    {
        #region Private Fields
        private Matrix _projection;
        private Matrix _view;
        private Matrix _world;
        private Boolean _dirty;

        private EffectParameter _worldViewProjectionParam;
        #endregion

        #region Public Properties
        public Matrix Projection
        {
            get => _projection;
            set
            {
                _projection = value;
                _dirty = true;
            }
        }
        public Matrix View
        {
            get => _view;
            set
            {
                _view = value;
                _dirty = true;
            }
        }
        public Matrix World
        {
            get => _world;
            set
            {
                _world = value;
                _dirty = true;
            }
        }
        #endregion

        #region Constructor
        public EffectMatricesEffect(GraphicsDevice graphicsDevice, Byte[] effectCode) : base(graphicsDevice, effectCode)
        {
            _worldViewProjectionParam = this.Parameters["WorldViewProjection"];
        }
        #endregion

        #region Helper Methods
        protected override void OnApply()
        {
            if (_dirty)
            {
                _worldViewProjectionParam.SetValue(
                    Matrix.Multiply(
                        Matrix.Multiply(
                            this.World,
                            this.View),
                        this.Projection));

                _dirty = false;
            }
        }
        #endregion
    }
}
