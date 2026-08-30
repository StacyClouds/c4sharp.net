using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace StacyClouds.C4Sharp.Config
{
    
    /// <summary>
    /// Stores workspace access control settings such as the allowed users and their roles.
    /// </summary>
    [DataContract]
    public sealed class WorkspaceConfiguration
    {

        private HashSet<User> _users;
    
        /// <summary>
        /// Returns the configured users as a defensive copy.
        /// </summary>
        /// <remarks>
        /// Changes made to the returned set do not update the underlying configuration.
        /// </remarks>
        [DataMember(Name = "users", EmitDefaultValue = false)]
        public ISet<User> Users
        {
            get
            {
                return new HashSet<User>(_users);
            }

            internal set
            {
                _users = new HashSet<User>(value);
            }
        }

        /// <summary>
        /// Initializes an empty configuration for serializers and new workspaces.
        /// </summary>
        [JsonConstructor]
        internal WorkspaceConfiguration()
        {
            _users = new HashSet<User>();
        }
        
        /// <summary>
        /// Adds or replaces a workspace user with the supplied role.
        /// </summary>
        /// <param name="username">The username to configure.</param>
        /// <param name="role">The role to assign to the user.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="username"/> is missing or whitespace.</exception>
        public void AddUser(string username, Role role)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("A username must be specified.");
            }

            _users.Add(new User(username, role));
        }
   
    }
    
}