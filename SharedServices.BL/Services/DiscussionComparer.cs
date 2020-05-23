using SharedServices.BL.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SharedServices.BL.Services
{
    public class DiscussionComparer : IEqualityComparer<Discussion>
    {
        public bool Equals([AllowNull] Discussion x, [AllowNull] Discussion y)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (x is null || y is null) return false;

            //Check whether the products' properties are equal.

            return (x.Emitter.Equals(y.Emitter) || x.Emitter.Equals(y.Receiver)) 
               && (x.Receiver.Equals(y.Emitter) || x.Receiver.Equals(y.Receiver));
        }

        public int GetHashCode([DisallowNull] Discussion discussion)
        {
            //Check whether the object is null
            if (discussion is null) return 0;

            //Get hash code for the Name field if it is not null.
            int hashDiscussionEmiter = discussion.Emitter == null ? 0 : discussion.Emitter.GetHashCode();

            //Get hash code for the Code field.
            int hashDiscussionReceiver = discussion.Receiver == null ? 0 : discussion.Receiver.GetHashCode();

            //Calculate the hash code for the product.
            return hashDiscussionEmiter ^ hashDiscussionReceiver;
        }
    }
}