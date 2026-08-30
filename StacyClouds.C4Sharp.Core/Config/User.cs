using System;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp.Config
{
    
    /// <summary>
    /// Represents a configured workspace user and the role assigned to that user.
    /// </summary>
    [DataContract]
    public sealed class User : IEquatable<User>
    {
        
        /// <summary>
        /// Identifies the user in Structurizr access control settings.
        /// </summary>
        [DataMember(Name = "username", EmitDefaultValue = false)]
        public string Username { get; internal set; }
        
        /// <summary>
        /// Determines the permissions granted to the user.
        /// </summary>
        [DataMember(Name = "role", EmitDefaultValue = true)]
        public Role Role { get; internal set; }

        /// <summary>
        /// Initializes a user placeholder for serializers.
        /// </summary>
        internal User()
        {
        }

        /// <summary>
        /// Initializes a user definition.
        /// </summary>
        /// <param name="username">The username to grant access to.</param>
        /// <param name="role">The access level assigned to the user.</param>
        internal User(string username, Role role)
        {
            Username = username;
            Role = role;
        }
        
        /// <summary>
        /// Determines whether another user definition refers to the same username.
        /// </summary>
        /// <param name="other">The user to compare with this instance.</param>
        /// <returns><c>true</c> when both users have the same username; otherwise, <c>false</c>.</returns>
        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Username, other.Username);
        }

        /// <summary>
        /// Determines whether another object represents the same configured user.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><c>true</c> when <paramref name="obj"/> is a <see cref="User"/> with the same username; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        /// <summary>
        /// Computes a hash code based on the username.
        /// </summary>
        /// <returns>A hash code suitable for user set membership.</returns>
        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

    }
    
}