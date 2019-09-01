using Guppy.Collections;
using Guppy.Network.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Collections
{
    public class UserCollection : CreatableCollection<User>
    {
        #region Private Fields
        private UserFactory _factory;
        #endregion

        #region Constructor
        public UserCollection(UserFactory factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
        }
        #endregion

        #region Create Methods
        public User Create(String name, Action<User> setup = null)
        {
            return this.Create(u =>
            {
                u.Name = name;

                setup?.Invoke(u);
            });
        }

        public User Create(Action<User> setup = null)
        {
            var user = _factory.Build<User>(setup);

            this.Add(user);

            return user;
        }

        public User GetOrCreateById(Guid id)
        {
            User user;

            if ((user = this.GetById<User>(id)) == default(User))
                user = this.Create("", u => u.SetId(id));

            return user;
        }
        #endregion
    }
}
